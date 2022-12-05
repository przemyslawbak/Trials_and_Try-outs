import math

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers
from tensorflow.keras.utils import Sequence
from datetime import timedelta
from sklearn.preprocessing import MinMaxScaler
from sklearn.metrics import mean_squared_error

import numpy as np
import pandas as pd
import time

import os

#Loading data
# read the dataset into python
df = pd.read_csv('household_power_consumption.csv', delimiter=';')
df.head()

#Preprocessing the Dataset for Time Series Analysis
df['date_time'] = pd.to_datetime(df['Date'] + ' ' + df['Time'])
df['Global_active_power'] = pd.to_numeric(df['Global_active_power'], errors='coerce')
df = df.dropna(subset=['Global_active_power'])

df['date_time'] = pd.to_datetime(df['date_time'])

df = df.loc[:, ['date_time', 'Global_active_power']]
df.sort_values('date_time', inplace=True, ascending=True)
df = df.reset_index(drop=True)

print('Number of rows and columns after removing missing values:', df.shape)
print('The time series starts from: ', df['date_time'].min())
print('The time series ends on: ', df['date_time'].max())

#Split the dataset into training, validation, and test datasets.
# Since it's timeseries we should do it by date.
test_cutoff_date = df['date_time'].max() - timedelta(days=7)
val_cutoff_date = test_cutoff_date - timedelta(days=14)

df_test = df[df['date_time'] > test_cutoff_date]
df_val = df[(df['date_time'] > val_cutoff_date) & (df['date_time'] <= test_cutoff_date)]
df_train = df[df['date_time'] <= val_cutoff_date]

#check out the datasets
print('Test dates: {} to {}'.format(df_test['date_time'].min(), df_test['date_time'].max()))
print('Validation dates: {} to {}'.format(df_val['date_time'].min(), df_val['date_time'].max()))
print('Train dates: {} to {}'.format(df_train['date_time'].min(), df_train['date_time'].max()))

#Transforming the Dataset for TensorFlow Keras
def create_ts_files(dataset, 
                    start_index, 
                    end_index, 
                    history_length, 
                    step_size, 
                    target_step, 
                    num_rows_per_file, 
                    data_folder):
    assert step_size > 0
    assert start_index >= 0
    
    if not os.path.exists(data_folder):
        os.makedirs(data_folder)
    
    time_lags = sorted(range(target_step+1, target_step+history_length+1, step_size), reverse=True)
    col_names = [f'x_lag{i}' for i in time_lags] + ['y']
    start_index = start_index + history_length
    if end_index is None:
        end_index = len(dataset) - target_step
    
    rng = range(start_index, end_index)
    num_rows = len(rng)
    num_files = math.ceil(num_rows/num_rows_per_file)
    
    # for each file.
    print(f'Creating {num_files} files.')
    for i in range(num_files):
        filename = f'{data_folder}/ts_file{i}.pkl'
        
        if i % 10 == 0:
            print(f'{filename}')
            
        # get the start and end indices.
        ind0 = i*num_rows_per_file
        ind1 = min(ind0 + num_rows_per_file, end_index)
        data_list = []
        
        # j in the current timestep. Will need j-n to j-1 for the history. And j + target_step for the target.
        for j in range(ind0, ind1):
            indices = range(j-1, j-history_length-1, -step_size)
            data = dataset[sorted(indices) + [j+target_step]]
            
            # append data to the list.
            data_list.append(data)

        df_ts = pd.DataFrame(data=data_list, columns=col_names)
        df_ts.to_pickle(filename)
            
    return len(col_names)-1

global_active_power = df_train['Global_active_power'].values

# Scaled to work with Neural networks.
scaler = MinMaxScaler(feature_range=(0, 1))
global_active_power_scaled = scaler.fit_transform(global_active_power.reshape(-1, 1)).reshape(-1, )

history_length = 7*24*60  # The history length in minutes.
step_size = 10  # The sampling rate of the history. Eg. If step_size = 1, then values from every minute will be in the history.
                #                                       If step size = 10 then values every 10 minutes will be in the history.
target_step = 10  # The time step in the future to predict. Eg. If target_step = 0, then predict the next timestep after the end of the history period.
                  #                                             If target_step = 10 then predict 10 timesteps the next timestep (11 minutes after the end of history).

# The csv creation returns the number of rows and number of features. We need these values below.
num_timesteps = create_ts_files(global_active_power_scaled,
                                start_index=0,
                                end_index=None,
                                history_length=history_length,
                                step_size=step_size,
                                target_step=target_step,
                                num_rows_per_file=128*100,
                                data_folder='ts_data')

# I found that the easiest way to do time series with tensorflow is by creating pandas files with the lagged time steps (eg. x{t-1}, x{t-2}...) and 
# the value to predict y = x{t+n}. We tried doing it using TFRecords, but that API is not very intuitive and lacks working examples for time series.
# The resulting file using these parameters is over 17GB. If history_length is increased, or  step_size is decreased, it could get much bigger.
# Hard to fit into laptop memory, so need to use other means to load the data from the hard drive.

#
# So we can handle loading the data in chunks from the hard drive instead of having to load everything into memory.
# 
# The reason we want to do this is so we can do custom processing on the data that we are feeding into the LSTM.
# LSTM requires a certain shape and it is tricky to get it right.
#
class TimeSeriesLoader:
    def __init__(self, ts_folder, filename_format):
        self.ts_folder = ts_folder
        
        # find the number of files.
        i = 0
        file_found = True
        while file_found:
            filename = self.ts_folder + '/' + filename_format.format(i)
            file_found = os.path.exists(filename)
            if file_found:
                i += 1
                
        self.num_files = i
        self.files_indices = np.arange(self.num_files)
        self.shuffle_chunks()
        
    def num_chunks(self):
        return self.num_files
    
    def get_chunk(self, idx):
        assert (idx >= 0) and (idx < self.num_files)
        
        ind = self.files_indices[idx]
        filename = self.ts_folder + '/' + filename_format.format(ind)
        df_ts = pd.read_pickle(filename)
        num_records = len(df_ts.index)
        
        features = df_ts.drop('y', axis=1).values
        target = df_ts['y'].values
        
        # reshape for input into LSTM. Batch major format.
        features_batchmajor = np.array(features).reshape(num_records, -1, 1)
        return features_batchmajor, target
    
    # this shuffles the order the chunks will be outputted from get_chunk.
    def shuffle_chunks(self):
        np.random.shuffle(self.files_indices)

ts_folder = 'ts_data'
filename_format = 'ts_file{}.pkl'
tss = TimeSeriesLoader(ts_folder, filename_format)

# Create the Keras model.
# Use hyperparameter optimization if you have the time.

ts_inputs = tf.keras.Input(shape=(num_timesteps, 1))

# units=10 -> The cell and hidden states will be of dimension 10.
#             The number of parameters that need to be trained = 4*units*(units+2)
x = layers.LSTM(units=10)(ts_inputs)
x = layers.Dropout(0.2)(x)
outputs = layers.Dense(1, activation='linear')(x)
model = tf.keras.Model(inputs=ts_inputs, outputs=outputs)

# Specify the training configuration.
model.compile(optimizer=tf.keras.optimizers.SGD(learning_rate=0.01),
              loss=tf.keras.losses.MeanSquaredError(),
              metrics=['mse'])

# train in batch sizes of 128.
BATCH_SIZE = 128
NUM_EPOCHS = 1
NUM_CHUNKS = tss.num_chunks()

for epoch in range(NUM_EPOCHS):
    print('epoch #{}'.format(epoch))
    for i in range(NUM_CHUNKS):
        X, y = tss.get_chunk(i)
        
        # model.fit does train the model incrementally. ie. Can call multiple times in batches.
        # https://github.com/keras-team/keras/issues/4446
        model.fit(x=X, y=y, batch_size=BATCH_SIZE)
        
    # shuffle the chunks so they're not in the same order next time around.
    tss.shuffle_chunks()

# evaluate the model on the validation set.
#
# Create the validation CSV like we did before with the training.
global_active_power_val = df_val['Global_active_power'].values
global_active_power_val_scaled = scaler.transform(global_active_power_val.reshape(-1, 1)).reshape(-1, )

history_length = 7*24*60  # The history length in minutes.
step_size = 10  # The sampling rate of the history. Eg. If step_size = 1, then values from every minute will be in the history.
                #                                       If step size = 10 then values every 10 minutes will be in the history.
target_step = 10  # The time step in the future to predict. Eg. If target_step = 0, then predict the next timestep after the end of the history period.
                  #                                             If target_step = 10 then predict 10 timesteps the next timestep (11 minutes after the end of history).

# The csv creation returns the number of rows and number of features. We need these values below.
num_timesteps = create_ts_files(global_active_power_val_scaled,
                                start_index=0,
                                end_index=None,
                                history_length=history_length,
                                step_size=step_size,
                                target_step=target_step,
                                num_rows_per_file=128*100,
                                data_folder='ts_val_data')

# If we assume that the validation dataset can fit into memory we can do this.
df_val_ts = pd.read_pickle('ts_val_data/ts_file0.pkl')


features = df_val_ts.drop('y', axis=1).values
features_arr = np.array(features)

# reshape for input into LSTM. Batch major format.
num_records = len(df_val_ts.index)
features_batchmajor = features_arr.reshape(num_records, -1, 1)


y_pred = model.predict(features_batchmajor).reshape(-1, )
y_pred = scaler.inverse_transform(y_pred.reshape(-1, 1)).reshape(-1 ,)

y_act = df_val_ts['y'].values
y_act = scaler.inverse_transform(y_act.reshape(-1, 1)).reshape(-1 ,)

print('validation mean squared error: {}'.format(mean_squared_error(y_act, y_pred)))

#baseline
y_pred_baseline = df_val_ts['x_lag11'].values
y_pred_baseline = scaler.inverse_transform(y_pred_baseline.reshape(-1, 1)).reshape(-1 ,)
print('validation baseline mean squared error: {}'.format(mean_squared_error(y_act, y_pred_baseline)))

