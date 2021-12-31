#https://www.simplilearn.com/tutorials/deep-learning-tutorial/rnn

import pandas as pd
import matplotlib.pyplot as plt
import numpy as np
from sklearn.preprocessing import MinMaxScaler
from tensorflow.keras import Sequential
from tensorflow.keras.layers import Dense, LSTM, Dropout

#Import the training dataset
filename = "AAPL.csv"
dataset_train = pd.read_csv(filename)
training_set = dataset_train[['close', 'high', 'low', 'open', 'volume']]

#Perform feature scaling to transform the data
sc = MinMaxScaler(feature_range = (0, 1))
training_set_scaled = sc.fit_transform(training_set)

#Variables
time_step = 60
split_percent = 0.80
split = int(split_percent*len(df))

#Create a data structure with 60-time steps and 1 output
X = []
y = []
for i in range(time_step, len(training_set_scaled)):
    X.append(training_set_scaled[i-time_step: i, 0])
    y.append(training_set_scaled[i, 0])

X_train_arr, y_train_arr = np.array(X), np.array(y)

#Reshaping
X_train_arr = np.reshape(X_train_arr, (X_train_arr.shape[0], X_train_arr.shape[1], 1))

print(X_train_arr.shape)
print(y_train_arr.shape)

#Split data
X_train_splitted = X_train_arr[:split]
y_train_splitted = y_train_arr[:split]
X_test_splitted = X_train_arr[split:]
y_test_splitted = y_train_arr[split:]

print(X_train_arr.shape)
print(y_train_arr.shape)

#Initialize the RNN
model = Sequential()

#Add the LSTM layers and some dropout regularization
model.add(LSTM(units= 50, activation = 'relu', return_sequences = True, input_shape = (X_train_arr.shape[1], 1)))
model.add(Dropout(0.2))
model.add(LSTM(units= 40, activation = 'relu', return_sequences = True))
model.add(Dropout(0.2))
model.add(LSTM(units= 80, activation = 'relu', return_sequences = True))
model.add(Dropout(0.2))

#Add the output layer.
model.add(Dense(units = 1))

#Compile the RNN
model.compile(optimizer='adam', loss = 'mean_squared_error')

#Fit to the training set
model.fit(X_train_splitted, y_train_splitted, epochs=10, batch_size=32)

#plot
plt.plot(day_new,sc.inverse_transform(df1[1158:]))
plt.plot(day_pred,sc.inverse_transform(lst_output))

#todo: future prediction