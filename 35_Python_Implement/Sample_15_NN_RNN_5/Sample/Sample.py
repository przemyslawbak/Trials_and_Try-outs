import pandas as pd
import matplotlib.pyplot as plt
import numpy as np
from sklearn.preprocessing import MinMaxScaler
from tensorflow.keras import Sequential
from tensorflow.keras.layers import Dense, LSTM, Dropout, TimeDistributed, Activation

#Import the training dataset
filename = "GPW_DLY WIG20, 15.csv"
dataset_train = pd.read_csv(filename)
training_set = dataset_train[['close', 'high', 'low', 'open', 'Volume']]

#Perform feature scaling to transform the data
scaler = MinMaxScaler(feature_range = (0, 1))
training_set_scaled = scaler.fit_transform(training_set)

#Variables
future_prediction = 30
time_step = 60
split_percent = 0.80
split = int(split_percent*len(training_set_scaled))

#Create a data structure with n-time steps
X = []
y = []
for i in range(time_step + 1, len(training_set_scaled)):
    X.append(training_set_scaled[i-time_step-1:i-1, 0:len(training_set.columns)]) #take all columns into the set
    y.append(training_set_scaled[i, 0:len(training_set.columns)]) #take all columns into the set

X_train_arr, y_train_arr = np.array(X), np.array(y)

print(X_train_arr.shape) #(2494, 60, 5)
print(y_train_arr.shape) #(2494, 5)

#Split data
X_train_splitted = X_train_arr[:split]
y_train_splitted = y_train_arr[:split]
X_test_splitted = X_train_arr[split:]
y_test_splitted = y_train_arr[split:]

#Reshaping to rows/time_step/columns
X_train_arr = np.reshape(X_train_arr, (X_train_arr.shape[0], X_train_arr.shape[1], X_train_arr.shape[2])) #(samples, time-steps, features)
y_train_splitted = np.reshape(y_train_splitted, (y_train_splitted.shape[0], 1, y_train_splitted.shape[1]))  #(samples, time-steps, features)
X_test_splitted = np.reshape(X_test_splitted, (X_test_splitted.shape[0], X_test_splitted.shape[1], X_test_splitted.shape[2])) #(samples, time-steps, features)
y_test_splitted = np.reshape(y_test_splitted, (y_test_splitted.shape[0], 1, y_test_splitted.shape[1]))  #(samples, time-steps, features)

print(X_train_arr.shape) #(2494, 60, 5)
print(y_train_arr.shape) #(2494, 1, 5)
print(X_test_splitted.shape) #(2494, 60, 5)
print(y_test_splitted.shape) #(2494, 1, 5)

#Initialize the RNN
model = Sequential()

#Add Stacked LSTM
model.add(LSTM(units= 200, activation = 'relu', return_sequences = True, input_shape = (X_train_splitted.shape[1], X_train_splitted.shape[2]))) #time_step/columns
model.add(LSTM(100, activation='relu', return_sequences=True))
model.add(LSTM(50, activation='relu', return_sequences=True))
model.add(LSTM(25, activation='relu'))
model.add(Dense(20, activation='relu'))
model.add(Dense(10, activation='relu'))
model.add(Dense(1))

#Compile the RNN
model.compile(optimizer='adam', loss = 'mean_squared_error')

#Fit to the training set
model.fit(X_train_splitted, y_train_splitted, epochs=3, batch_size=32, validation_split=0.2, verbose=1)

#Test results
y_pred = model.predict(X_test_splitted, verbose=1)

#Reverse scale resuts and reshaping data to display
y_test_splitted = scaler.inverse_transform(y_test_splitted)
y_pred = scaler.inverse_transform(y_pred)

#plot
plt.figure(figsize=(14,5))
plt.plot(y_test_splitted[-time_step:], label = "Real values") #time_step +/- 1
plt.plot(y_pred[-time_step:], label = 'Predicted values') #time_step +/- 1
plt.title('WIG20 prediction test')
plt.xlabel('time')
plt.ylabel('Close price')
plt.legend()
plt.show()

#todo: future prediction