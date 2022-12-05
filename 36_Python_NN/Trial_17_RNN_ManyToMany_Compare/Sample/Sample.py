import pandas as pd
import matplotlib.pyplot as plt
import numpy as np
from sklearn.preprocessing import MinMaxScaler
from tensorflow.keras import Sequential
import tensorflow as tf
from tensorflow.keras.layers import Dense, LSTM, Dropout, TimeDistributed, RepeatVector, Bidirectional

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
    y.append(training_set_scaled[i-time_step:i, 0:len(training_set.columns)]) #take all columns into the set

X_train_arr, y_train_arr = np.array(X), np.array(y)

print(X_train_arr.shape) #(2494, 60, 5) <-- train data, having now 2494 rows, with 60 time steps, each row has 5 features (MANY)
print(y_train_arr.shape) #(2494, 5) <-- target data, having now 2494 rows, with 1 time step, but 5 features (TO MANY)

#Split data
X_train_splitted = X_train_arr[:split] #(80%) model train input data
y_train_splitted = y_train_arr[:split] #80%) model train target data
X_test_splitted = X_train_arr[split:] #(20%) test prediction input data
y_test_splitted = y_train_arr[split:] #(20%) test prediction compare data

#Reshaping to rows/time_step/columns
X_train_splitted = np.reshape(X_train_splitted, (X_train_splitted.shape[0], X_train_splitted.shape[1], X_train_splitted.shape[2])) #(samples, time-steps, features), by default should be already
y_train_splitted = np.reshape(y_train_splitted, (y_train_splitted.shape[0], y_train_splitted.shape[1], y_train_splitted.shape[2]))  #(samples, time-steps, features)
X_test_splitted = np.reshape(X_test_splitted, (X_test_splitted.shape[0], X_test_splitted.shape[1], X_test_splitted.shape[2])) #(samples, time-steps, features), by default should be already
y_test_splitted = np.reshape(y_test_splitted, (y_test_splitted.shape[0], y_test_splitted.shape[1], y_test_splitted.shape[2]))  #(samples, time-steps, features)

print(X_train_arr.shape) #(2494, 60, 5)
print(y_train_arr.shape) #(2494, 1, 5)
print(X_test_splitted.shape) #(450, 60, 5)
print(y_test_splitted.shape) #(450, 1, 5)

base_results = []
update_results = []
repeats = 5

for x in range(repeats):
    print('Trial No: ' + str(x) + ' of ' + str(repeats))
    #model2 - with bidirectional 2
    model2 = Sequential()
    #model2.add(Bidirectional(LSTM(100, activation='relu', input_shape = (X_train_splitted.shape[1], X_train_splitted.shape[2]), return_sequences=True)))
    #model2.add(Bidirectional(LSTM(100, activation='relu', return_sequences=True)))
    model2.add(LSTM(100, activation='relu', input_shape = (X_train_splitted.shape[1], X_train_splitted.shape[2]), return_sequences=True))
    model2.add(LSTM(100, activation='relu', return_sequences=True))
    model2.add(Dense(4))
    model2.compile(optimizer='adam', loss='mse', metrics=['mae'])
    model2.fit(X_train_splitted, y_train_splitted, epochs=5, validation_split=0.2, verbose=2, batch_size=64)

    #results1 = model1.evaluate(X_test_splitted, y_test_splitted, batch_size=128, verbose=2)
    results2 = model2.evaluate(X_test_splitted, y_test_splitted, batch_size=128, verbose=2)
    #print("test loss 1", results1)
    print("test loss 2", results2)
    #base_results.append(results1)
    update_results.append(results2)

#mean_res1 = np.mean(base_results, axis=0)
mean_res2 = np.mean(update_results, axis=0)

#many-to-one LSTM + RepeatVector + LSTM + Dense(1):#3.252190799685195e-05, 0.004242862109094858
print('MSE & MAE 2 (comp.): ' + str(mean_res2)) #2.308580224053003e-05, 0.002931134542450309

#CONCLUSION: many-to-many wins