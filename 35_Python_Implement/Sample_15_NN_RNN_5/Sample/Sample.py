import pandas as pd
import matplotlib.pyplot as plt
import numpy as np
from sklearn.preprocessing import MinMaxScaler
from tensorflow.keras import Sequential
from tensorflow.keras.layers import Dense, LSTM, Dropout

#Import the training dataset
filename = "GPW_DLY WIG20, 15.csv"
dataset_train = pd.read_csv(filename)
training_set = dataset_train[['close', 'high', 'low', 'open', 'Volume']]

#Perform feature scaling to transform the data
scaler = MinMaxScaler(feature_range = (0, 1))
training_set_scaled = scaler.fit_transform(training_set)
print(training_set_scaled)

#Variables
future_prediction = 30
time_step = 60
split_percent = 0.80
split = int(split_percent*len(training_set_scaled))

#Create a data structure with 60-time steps and 1 output
X = []
y = []
for i in range(time_step, len(training_set_scaled)):
    X.append(training_set_scaled[i-time_step:i, 0:len(training_set.columns)]) #take all columns into the set
    y.append(training_set_scaled[i, 0])

X_train_arr, y_train_arr = np.array(X), np.array(y)

#Reshaping
X_train_arr = np.reshape(X_train_arr, (X_train_arr.shape[0], X_train_arr.shape[1], X_train_arr.shape[2])) #rows/time_step/columns


#Split data
X_train_splitted = X_train_arr[:split]
y_train_splitted = y_train_arr[:split]
X_test_splitted = X_train_arr[split:]
y_test_splitted = y_train_arr[split:]
print(X_train_splitted)

#Initialize the RNN
model = Sequential()

#Add the LSTM layers and some dropout regularization
model.add(LSTM(units= 50, activation = 'relu', return_sequences = True, input_shape = (X_train_arr.shape[1], 1))) #todo: update
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
model.fit(X_train_splitted, y_train_splitted, epochs=3, batch_size=32)

#Test results
y_pred = model.predict(X_test_splitted)
print(y_pred.shape)
y_pred = y_pred.reshape(-1,1)
print(y_pred.reshape(-1,1))

#Reverse scale resuts
res_scaler = MinMaxScaler()
res_scaler.min_, res_scaler.scale_ = scaler.min_[0], scaler.scale_[0]
y_pred = res_scaler.inverse_transform(y_pred.reshape(-1,1))
y_test_splitted = res_scaler.inverse_transform(y_test_splitted.reshape(-1,1))

#plot
plt.figure(figsize=(14,5))
plt.plot(y_test_splitted[-time_step:], label = "Real values")
plt.plot(y_pred[-time_step:], label = 'Predicted values')
plt.title('WIG20 prediction test')
plt.xlabel('time')
plt.ylabel('Close price')
plt.legend()
plt.show()

#todo: future prediction