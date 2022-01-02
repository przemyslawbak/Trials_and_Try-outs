#https://wandb.ai/ayush-thakur/dl-question-bank/reports/One-to-Many-Many-to-One-and-Many-to-Many-LSTM-Examples-in-Keras--VmlldzoyMDIzOTM

import pandas as pd
import matplotlib.pyplot as plt
import numpy as np
from sklearn.preprocessing import MinMaxScaler
from tensorflow.keras import Sequential
from tensorflow.keras.layers import Dense, LSTM, Dropout

#ONE TO MANY
X = [1, 4, 7, 10, 13, 16, 19, 22, 25, 28, 31, 34, 37, 40, 43] #input data is a sequence of numbers
y = [[2, 3], [5, 6], [8, 9], [11, 12], [14, 15], [17, 18], [20, 21], [23, 24], [26, 27], [29, 30], [32, 33], [35, 36], [38, 39], [41, 42], [44, 45]] #output data is the sequence of the next two numbers after the input number

#Variables
time_step = 60
split_percent = 0.80
split = int(split_percent*len(X))

X_train_arr, y_train_arr = np.array(X), np.array(y)

print(X_train_arr.shape)
print(y_train_arr.shape)

#Split data
X_train_splitted = X_train_arr[:split].reshape(-1, 12, 1)
y_train_splitted = y_train_arr[:split].reshape(-1, 12, 2)
X_test_splitted = X_train_arr[split:]
y_test_splitted = y_train_arr[split:]

print(X_train_splitted.shape)
print(y_train_splitted.shape)

#Initialize the RNN
model = Sequential()

#Add the LSTM layers and some dropout regularization
model.add(LSTM(units= 50, activation = 'relu', input_shape = (15, 1)))
model.add(Dropout(0.2))

#Add the output layer.
model.add(Dense(units = 1))

#Compile the RNN
model.compile(optimizer='adam', loss = 'mean_squared_error')

#Fit to the training set
model.fit(X_train_splitted, y_train_splitted, epochs=1000, batch_size=3)

#Test results
y_pred = model.predict(X_test_splitted)

#plot
plt.figure(figsize=(14,5))
plt.plot(y_test_splitted[-60:], label = "Real values")
plt.plot(y_pred[-60:], label = 'Predicted values')
plt.title('AAPL prediction test')
plt.xlabel('time')
plt.ylabel('Close price')
plt.legend()
plt.show()