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

#Model variables
num_batch = 16
num_epochs = 2
num_validation=0.2
num_verbose=1

#Variables
features = len(training_set.columns)
future_steps = 33
time_step = 100 #learning step
split_percent = 0.80 #train/test daa split percent (80%)
split = int(split_percent*len(training_set_scaled)) #split percent multiplying by data rows

#Create a data structure with n-time steps
X = []
y = []
for i in range(time_step + 1, len(training_set_scaled)):
    X.append(training_set_scaled[i-time_step-1:i-1, 0:features]) #take all columns into the set, including time_step legth
    y.append(training_set_scaled[i-time_step:i, 0:features]) #take all columns into the set, including time_step legth

X_train_arr, y_train_arr = np.array(X), np.array(y)

print(X_train_arr.shape) #(2494, 100, 4) <-- train data, having now 2494 rows, with 100 time steps, each row has 4 features (MANY)
print(y_train_arr.shape) #(2494, 100, 4) <-- target data, having now 2494 rows, with 100 time step, but 4 features (TO MANY)

#Split data
X_train_splitted = X_train_arr[:split] #(80%) model train input data
y_train_splitted = y_train_arr[:split] #80%) model train target data
X_test_splitted = X_train_arr[split:] #(20%) test prediction input data
y_test_splitted = y_train_arr[split:] #(20%) test prediction compare data

#Reshaping to rows/time_step/columns
X_train_splitted = np.reshape(X_train_splitted, (X_train_splitted.shape[0], time_step, features))
y_train_splitted = np.reshape(y_train_splitted, (y_train_splitted.shape[0], time_step, features))
X_test_splitted = np.reshape(X_test_splitted, (X_test_splitted.shape[0], time_step, features))
y_test_splitted = np.reshape(y_test_splitted, (y_test_splitted.shape[0], time_step, features))

print(X_train_arr.shape) #(2494, 100, 4)
print(y_train_arr.shape) #(2494, 100, 4)
print(X_test_splitted.shape) #(450, 100, 4)
print(y_test_splitted.shape) #(450, 100, 4)

#Initialize the RNN
model = Sequential()

#Add Bidirectional LSTM, has better performance than stacked LSTM
model = Sequential()
model.add(Bidirectional(LSTM(128, activation='relu', input_shape = (time_step, features), return_sequences=True))) #todo: tune qty o layers
model.add(Dense(4)) #4 outputs, gives output shape (x, 100, 4)

#Compile many-to-many
model.compile(optimizer='adam', loss = 'mean_squared_error', metrics=['accuracy'])

#Fit to the training set
model.fit(X_train_splitted, y_train_splitted, epochs=num_epochs, batch_size=num_batch, validation_split=num_validation, verbose=num_verbose)

#Predicting future
def predict_future():
    pred = model.predict(X_test_splitted, verbose=num_verbose) #predict from updated X_test_splitted
    last_pred_item = pred[-1]
    X_test_splitted_shape = X_test_splitted.shape
    X_test_splitted = X_test_splitted = np.append(X_test_splitted, last_pred_item)
    X_test_splitted = np.reshape(X_test_splitted, (X_test_splitted_shape[0] + 1, time_step, features)) #reshape X_test_splitted to (x + 1, 100, 4)
    return pred

def nextFutureStep():
    pred = predict_future()
    return pred[-1] #return last prediction step

future_results = np.array([]) #initialize numpy array
y_pred = predict_future() #create initial result array

for next in range(future_steps):
    print('future step ' + str(next) + ' of ' + str(future_steps)) #display info
    new_item = nextFutureStep() #assign item
    future_results = np.append(future_results, new_item) #add last step to future_results prediction array

#Combine future time_step based prediction results with initial prediction based on test data
future_results = np.reshape(future_results, (future_steps, time_step, features)) #reshaping to (33, 100, 4)
y_pred_future = future_results[:, -1:, :] #from future_results copying an array, where we do not need 60 time_steps
y_pred = np.append(y_pred, y_pred_future) #combine initial y_pred result array with predicted y_pred_future

#Reshaping data for inverse transforming
y_test_splitted = np.reshape(y_test_splitted, (y_test_splitted.shape[0], features)) #reshaping to (450, 1, 4)
y_pred = np.reshape(y_pred, (y_test_splitted.shape[0] + future_steps, features)) #reshaping to (450 + 33, 1, 4)

#Reversing transform to get proper data values
y_test_splitted = scaler.inverse_transform(y_test_splitted)
y_pred = scaler.inverse_transform(y_pred)

#Plot data
plt.figure(figsize=(14,features))
plt.plot(y_test_splitted[-time_step:, 3], label = "Real values") #I am interested only with display of column index 3
plt.plot(y_pred[-time_step - future_steps:, 3], label = 'Predicted values') # #I am interested only with display of column index 3
plt.title('Prediction test')
plt.xlabel('Time')
plt.ylabel('Column index 3')
plt.legend()
plt.show()