#many-to-many according to this: https://stackoverflow.com/a/43047615/12603542

import pandas as pd
import matplotlib.pyplot as plt
import numpy as np
from sklearn.preprocessing import MinMaxScaler
from tensorflow.keras import Sequential
from tensorflow.keras.layers import Dense, LSTM, Dropout, TimeDistributed, Activation, RepeatVector, Bidirectional

#Import the training dataset
filename = "GPW_DLY WIG20, 15.csv"
dataset_train = pd.read_csv(filename)
training_set = dataset_train[['close', 'high', 'low', 'open']]

#Perform feature scaling to transform the data
scaler = MinMaxScaler(feature_range = (0, 1))
training_set_scaled = scaler.fit_transform(training_set)

#Variables
future_prediction = 30
time_step = 100 #learning step
split_percent = 0.80 #train/test daa split percent (80%)
split = int(split_percent*len(training_set_scaled)) #split percent multiplying by data rows

#Create a data structure with n-time steps
X = []
y = []
for i in range(time_step + 1, len(training_set_scaled)):
    X.append(training_set_scaled[i-time_step-1:i-1, 0:len(training_set.columns)]) #take all columns into the set, including time_step legth
    y.append(training_set_scaled[i-time_step:i, 0:len(training_set.columns)]) #take all columns into the set, including time_step legth

X_train_arr, y_train_arr = np.array(X), np.array(y)

print(X_train_arr.shape) #(2494, 100, 4) <-- train data, having now 2494 rows, with 60 time steps, each row has 4 features (MANY)
print(y_train_arr.shape) #(2494, 100, 4) <-- target data, having now 2494 rows, with 1 time step, but 4 features (TO MANY)

#Split data
X_train_splitted = X_train_arr[:split] #(80%) model train input data
y_train_splitted = y_train_arr[:split] #80%) model train target data
X_test_splitted = X_train_arr[split:] #(20%) test prediction input data
y_test_splitted = y_train_arr[split:] #(20%) test prediction compare data

#Reshaping to rows/time_step/columns
X_train_splitted = np.reshape(X_train_splitted, (X_train_splitted.shape[0], X_train_splitted.shape[1], X_train_splitted.shape[2])) #(samples, time-steps, features)
y_train_splitted = np.reshape(y_train_splitted, (y_train_splitted.shape[0], y_train_splitted.shape[1], y_train_splitted.shape[2]))  #(samples, time-steps, features)
X_test_splitted = np.reshape(X_test_splitted, (X_test_splitted.shape[0], X_test_splitted.shape[1], X_test_splitted.shape[2])) #(samples, time-steps, features)
y_test_splitted = np.reshape(y_test_splitted, (y_test_splitted.shape[0], y_test_splitted.shape[1], y_test_splitted.shape[2]))  #(samples, time-steps, features)

print(X_train_arr.shape) #(2494, 60, 4)
print(y_train_arr.shape) #(2494, 1, 4)
print(X_test_splitted.shape) #(450, 60, 4)
print(y_test_splitted.shape) #(450, 1, 4)

#Initialize the RNN
model = Sequential()

#Add Bidirectional LSTM, has better performance than stacked LSTM
model = Sequential()
model.add(Bidirectional(LSTM(128, activation='relu', input_shape = (X_train_splitted.shape[1], X_train_splitted.shape[2]), return_sequences=True))) #test no o layers
model.add(Dense(4)) #4 outputs, gives x, 100, 4

#Compile many-to-many
model.compile(optimizer='adam', loss = 'mean_squared_error', metrics=['accuracy'])

#Fit to the training set
model.fit(X_train_splitted, y_train_splitted, epochs=5, batch_size=64, validation_split=0.2, verbose=1)

#Test results
y_pred = model.predict(X_test_splitted, verbose=1)
print(y_pred.shape) 