import pandas as pd
import matplotlib.pyplot as plt
import numpy as np
from sklearn.preprocessing import MinMaxScaler
from tensorflow.keras import Sequential
from tensorflow.keras.layers import Dense, LSTM, Dropout, TimeDistributed, Activation, RepeatVector, Bidirectional
from tensorflow.keras.callbacks import EarlyStopping


pd.set_option('display.max_rows', 1000)
pd.set_option('display.max_columns', 10)
pd.set_option('display.width', 1000)

#based on: Tutorial_23_LSTM_ForTimeseries_MultipleInputMultipleOutputMultiStep2

#Model values !!!!!!!!!SAMPLE SETUP!!!!!!!!!!
dropout_rate=0.1
num_layers=2
future_steps = 100
time_steps = 100
lstm_units = 255 #allow to learn very long sequences
num_batch = 128 #number of samples to work through before updating the internal model parameters
num_epochs = 1 #number times that the learning algorithm will work through the entire training dataset (10, 100, 1000)
num_validation=0.2 #% split for validation set
num_verbose=1 #how to display model fit progress (0 = silent, 1 = progress bar, 2 = one line per epoch)
es_patinence = 5

#NEW
n_steps_out = time_steps

#Import the training dataset
filename = "GPW_DLY WIG20, 15.csv"
dataset_train = pd.read_csv(filename)
dataset_train['DateTime'] = pd.to_datetime(dataset_train['time'])
dataset_train['DateTime'] = dataset_train['DateTime'].dt.tz_localize('UTC').dt.tz_convert('Europe/Berlin')
training_set = dataset_train[['close', 'high', 'low', 'open']]

#scaling
scaler = MinMaxScaler(feature_range = (0, 1))
training_set_scaled = scaler.fit_transform(training_set)

features = len(training_set.columns)
split_percent = 0.80 #train/test daa split percent (80%)
split = int(split_percent*len(training_set_scaled)) #split percent multiplying by data rows

#Create a data structure with n-time steps
X = []
y = []
for i in range(time_steps + 1, len(training_set_scaled)):
    X.append(training_set_scaled[i-time_steps-1:i-1, 0:features]) #take all columns into the set, including time_steps legth
    y.append(training_set_scaled[i-time_steps:i, 0:features]) #take all columns into the set, including time_steps legth

X_train_arr, y_train_arr = np.array(X), np.array(y)

#Split data
X_train_splitted = X_train_arr[:split] #(80%) model train input data
y_train_splitted = y_train_arr[:split] #80%) model train target data
X_test_splitted = X_train_arr[split:] #(20%) test prediction input data
y_test_splitted = y_train_arr[split:] #(20%) test prediction compare data

#Reshaping to rows/time_steps/columns
X = np.reshape(X_train_splitted, (X_train_splitted.shape[0], time_steps, features))
y = np.reshape(y_train_splitted, (y_train_splitted.shape[0], time_steps, features))
X_test_splitted = np.reshape(X_test_splitted, (X_test_splitted.shape[0], time_steps, features))
y_test_splitted = np.reshape(y_test_splitted, (y_test_splitted.shape[0], time_steps, features))

# define model
model = Sequential()
model.add(Bidirectional(LSTM(300, activation='relu', input_shape=(time_steps, features))))
model.add(RepeatVector(n_steps_out))
model.add(Bidirectional(LSTM(300, activation='relu', return_sequences=True)))
model.add(TimeDistributed(Dense(features)))
model.compile(optimizer='adam', loss = 'mae', metrics=['mae', 'acc', 'mse'])

#Fit to the training set
es = EarlyStopping(monitor='val_mae', mode='min', patience=es_patinence)
model.fit(X, y, epochs=num_epochs, batch_size=num_batch, validation_split=num_validation, verbose=num_verbose, callbacks=[es])

x_input = X_test_splitted[-200:-100,:,:]
x_exp = X_test_splitted[-100:,:,:]
yhat = model.predict(x_input, verbose=0)
yhat = np.reshape(yhat[:, -1:, :], (yhat.shape[0], features))
xhat = np.reshape(x_exp[:, -1:, :], (x_exp.shape[0], features))
ihat = np.reshape(x_input[:, -1:, :], (x_input.shape[0], features))
y_pred = scaler.inverse_transform(yhat)
x_pred = scaler.inverse_transform(xhat)
i_inp = scaler.inverse_transform(ihat)
resy = pd.DataFrame(y_pred)
resx = pd.DataFrame(x_pred)
resi = pd.DataFrame(i_inp)

print('input:')
print(resi)
print('expected:')
print(resx)
print('result:')
print(resy)