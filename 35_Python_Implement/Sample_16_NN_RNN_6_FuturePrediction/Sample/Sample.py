import pandas as pd
import matplotlib.pyplot as plt
import numpy as np
from sklearn.preprocessing import MinMaxScaler
from tensorflow.keras import Sequential
from tensorflow.keras.layers import Dense, LSTM, Dropout, TimeDistributed, Activation, RepeatVector, Bidirectional

#Import the training dataset
filename = "GPW_DLY WIG20, 15.csv"
dataset_train = pd.read_csv(filename)
training_set = dataset_train[['close', 'high', 'low', 'open', 'Volume']]

#Perform feature scaling to transform the data
scaler = MinMaxScaler(feature_range = (0, 1))
training_set_scaled = scaler.fit_transform(training_set)

#Variables
future_prediction = 30
time_step = 60 #learning step
split_percent = 0.80 #train/test daa split percent (80%)
split = int(split_percent*len(training_set_scaled)) #split percent multiplying by data rows

#Create a data structure with n-time steps
X = []
y = []
for i in range(time_step + 1, len(training_set_scaled)):
    X.append(training_set_scaled[i-time_step-1:i-1, 0:len(training_set.columns)]) #take all columns into the set, including time_step legth decreased by 1
    y.append(training_set_scaled[i-time_step, 0:len(training_set.columns)]) #take all columns into the set, including time_step legth

X_train_arr, y_train_arr = np.array(X), np.array(y)

print(X_train_arr.shape) #(2494, 60, 5) <-- train data, having now 2494 rows, with 60 time steps, each row has 5 features (MANY)
print(y_train_arr.shape) #(2494, 60, 5) <-- target data, having now 2494 rows, with 1 time step, but 5 features (TO MANY)

#Split data
X_train_splitted = X_train_arr[:split] #(80%) model train input data
y_train_splitted = y_train_arr[:split] #80%) model train target data
X_test_splitted = X_train_arr[split:] #(20%) test prediction input data
y_test_splitted = y_train_arr[split:] #(20%) test prediction compare data

#Reshaping to rows/time_step/columns
X_train_splitted = np.reshape(X_train_splitted, (X_train_splitted.shape[0], X_train_splitted.shape[1], X_train_splitted.shape[2])) #(samples, time-steps, features), by default should be already
y_train_splitted = np.reshape(y_train_splitted, (y_train_splitted.shape[0], y_train_splitted.shape[1], y_train_splitted.shape[2]))  #(samples, time-steps, features), by default should be already
X_test_splitted = np.reshape(X_test_splitted, (X_test_splitted.shape[0], X_test_splitted.shape[1], X_test_splitted.shape[2])) #(samples, time-steps, features), by default should be already
y_test_splitted = np.reshape(y_test_splitted, (y_test_splitted.shape[0], y_test_splitted.shape[1], y_test_splitted.shape[2]))  #(samples, time-steps, features), by default should be already

print(X_train_arr.shape) #(2494, 60, 5)
print(y_train_arr.shape) #(2494, 60, 5)
print(X_test_splitted.shape) #(450, 60, 5)
print(y_test_splitted.shape) #(450, 60, 5)

#Initialize the RNN
model = Sequential()

#Add Bidirectional LSTM, has better performance than stacked LSTM
model = Sequential()
model.add(Bidirectional(LSTM(100, activation='relu', input_shape = (X_train_splitted.shape[1], X_train_splitted.shape[2])))) #input_shape will be (2494-size, 60-shape[1], 5-shape[2])
model.add(RepeatVector(5)) #for 5 column of features in output, in other cases used for time_step in output
model.add(Bidirectional(LSTM(100, activation='relu', return_sequences=True)))
model.add(TimeDistributed(Dense(1,activation='sigmoid')))

#Compile the RNN
model.compile(optimizer='adam', loss = 'mean_squared_error', metrics=['accuracy'])

#Fit to the training set
model.fit(X_train_splitted, y_train_splitted, epochs=5, batch_size=64, validation_split=0.2, verbose=1)

#Predicting future
def predict_future():
    return model.predict(X_test_splitted, verbose=1)

future_results = []
y_pred = predict_future()

def nextFutureStep():
    pred = predict_future()
    np.append(X_test_splitted, pred[-1], axis=0)
    return pred[-1]

for next in range(future_prediction):
    future_results.append(nextFutureStep())

y_pred.append(future_results)

#Reshaping data for inverse transforming #TODO RESHAPING TO 450,60,5
y_test_splitted = np.reshape(y_test_splitted, (y_test_splitted.shape[0], 5)) #reshaping for (450, 1, 5)
y_pred = np.reshape(y_pred, (y_pred.shape[0], 5)) #reshaping for (450, 1, 5)

#Reversing transform to get proper data values
y_test_splitted = scaler.inverse_transform(y_test_splitted)
y_pred = scaler.inverse_transform(y_pred)

#Plot data
plt.figure(figsize=(14,5))
plt.plot(y_test_splitted[-time_step:, 3], label = "Real values") #I am interested only with display of column index 3
plt.plot(y_pred[-time_step:, 3], label = 'Predicted values') # #I am interested only with display of column index 3
plt.title('Prediction test')
plt.xlabel('Time')
plt.ylabel('Column index 3')
plt.legend()
plt.show()

#todo: future prediction