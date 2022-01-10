import pandas as pd
import matplotlib.pyplot as plt
import numpy as np
from sklearn.preprocessing import MinMaxScaler
from tensorflow.keras import Sequential
import tensorflow as tf
from tensorflow.keras.layers import Dense, LSTM, Dropout, TimeDistributed, RepeatVector, Bidirectional, Input, Layer, Activation, BatchNormalization, dot, multiply, concatenate
from tensorflow.keras.optimizers import Adam
from tensorflow.keras.utils import plot_model
from tensorflow.keras.callbacks import EarlyStopping
import pydot as pyd
from keras.utils.vis_utils import plot_model, model_to_dot
from keras.models import Model
from keras import backend as K
import keras
from keras_self_attention import SeqSelfAttention, SeqWeightedAttention

keras.utils.vis_utils.pydot = pyd

#Import the training dataset
filename = "GPW_DLY WIG20, 15.csv"
dataset_train = pd.read_csv(filename)
training_set = dataset_train[['close', 'high', 'low', 'open']]
time_step = 60

from sklearn.preprocessing import MinMaxScaler
#Perform feature scaling to transform the data
scaler = MinMaxScaler(feature_range = (0, 1))
training_set_scaled = scaler.fit_transform(training_set)
#Variables
features = len(training_set.columns)
future_steps = 33
time_step = 60 #learning step
split_percent = 0.80 #train/test daa split percent (80%)
split = int(split_percent*len(training_set_scaled)) #split percent multiplying by data rows

#Create a data structure with n-time steps
X = []
y = []
for i in range(time_step + 1, len(training_set_scaled)):
    X.append(training_set_scaled[i-time_step-1:i-1, 0:features]) #take all columns into the set, including time_step legth
    y.append(training_set_scaled[i, 0:features]) #take all columns into the set

X_train_arr, y_train_arr = np.array(X), np.array(y)

#Split data
X_train_splitted = X_train_arr[:split] #(80%) model train input data
y_train_splitted = y_train_arr[:split] #(80%) model train target data
X_test_splitted = X_train_arr[split:] #(20%) test prediction input data
y_test_splitted = y_train_arr[split:] #(20%) test prediction compare data

#Reshaping to rows/time_step/columns
X_train_splitted = np.reshape(X_train_splitted, (X_train_splitted.shape[0], time_step, features)) #(samples, time-steps, features), by default should be already
y_train_splitted = np.reshape(y_train_splitted, (y_train_splitted.shape[0], 1, features))  #(samples, time-steps, features)
X_test_splitted = np.reshape(X_test_splitted, (X_test_splitted.shape[0], time_step, features)) #(samples, time-steps, features), by default should be already
y_test_splitted = np.reshape(y_test_splitted, (y_test_splitted.shape[0], 1, features))  #(samples, time-steps, features)

base_results = []
update_results = []
repeats = 5

for x in range(repeats):
    print('Trial No: ' + str(x) + ' of ' + str(repeats))

    #model1 - base
    model1 = Sequential()
    model1.add(LSTM(100, activation='relu', input_shape=(60, 4)))
    model1.add(RepeatVector(4))
    model1.add(LSTM(100, activation='relu', return_sequences=True))
    model1.add(TimeDistributed(Dense(1)))
    model1.compile(optimizer='adam', loss='mse', metrics=['mae'])
    model1.fit(X_train_splitted, y_train_splitted, epochs=5, validation_split=0.2, verbose=2, batch_size=64)
    
    #model2 - with Attention()
    n_hidden = 100
    input_train = Input(shape=(60, 4))
    output_train = Input(shape=(60, 4))
    encoder_stack_h, encoder_last_h, encoder_last_c = LSTM(n_hidden, activation='relu', return_state=True, return_sequences=True)(input_train)
    #encoder_last_h = BatchNormalization(momentum=0.6)(encoder_last_h)
    #encoder_last_c = BatchNormalization(momentum=0.6)(encoder_last_c)
    decoder_input = RepeatVector(output_train.shape[1])(encoder_last_h)
    decoder_stack_h = LSTM(n_hidden, activation='relu', return_state=False, return_sequences=True)(decoder_input, initial_state=[encoder_last_h, encoder_last_c])
    attention = dot([decoder_stack_h, encoder_stack_h], axes=[2, 2])
    #attention = Activation('softmax')(attention)
    context = dot([attention, encoder_stack_h], axes=[2,1])
    #context = BatchNormalization(momentum=0.6)(context)
    decoder_combined_context = concatenate([context, decoder_stack_h])
    out = TimeDistributed(Dense(output_train.shape[2]))(decoder_combined_context)
    model2 = Model(inputs=input_train, outputs=out)
    opt = Adam()
    model2.compile(optimizer='adam', loss='mse', metrics=['mae'])
    model2.fit(X_train_splitted, y_train_splitted, epochs=5, validation_split=0.2, verbose=2, batch_size=64)
    
    results1 = model1.evaluate(X_test_splitted, y_test_splitted, batch_size=128, verbose=2)
    results2 = model2.evaluate(X_test_splitted, y_test_splitted, batch_size=128, verbose=2)
    print("test loss 1", results1)
    print("test loss 2", results2)
    base_results.append(results1)
    update_results.append(results2)

mean_res1 = sum(base_results)/len(base_results)
mean_res2 = sum(update_results)/len(update_results)

print('Mean of results 1: ' + str(mean_res1)) #
print('Mean of results 2: ' + str(mean_res2)) #

#CONCLUSION: 