#many-to-many according to this: https://stackoverflow.com/a/43047615/12603542

import pandas as pd
import matplotlib.pyplot as plt
import numpy as np
from sklearn.preprocessing import MinMaxScaler
from tensorflow.keras import Sequential
from tensorflow.keras.layers import Dense, LSTM, Dropout, TimeDistributed, Activation, RepeatVector, Bidirectional
from tensorflow.keras.callbacks import EarlyStopping

#Import the training dataset
filename = "GPW_DLY WIG20, 15.csv"
dataset_train = pd.read_csv(filename)

dataset_train['DateTime'] = pd.to_datetime(dataset_train['time'])
dataset_train['DateTime'] = dataset_train['DateTime'].dt.tz_localize('UTC').dt.tz_convert('Europe/Berlin')
training_set = dataset_train[['close', 'high', 'low', 'open']]
training_set["day"] = dataset_train['DateTime'].map(lambda x: x.day)
training_set["month"] = dataset_train['DateTime'].map(lambda x: x.month)
training_set["year"] = dataset_train['DateTime'].map(lambda x: x.year)
training_set["hour"] = dataset_train['DateTime'].map(lambda x: x.hour)
#training_set["minute"] = dataset_train['DateTime'].map(lambda x: x.minute)

print(training_set)

#Perform feature scaling to transform the data
scaler = MinMaxScaler(feature_range = (0, 1))
training_set_scaled = scaler.fit_transform(training_set)

#Model values !!!!!!!!!SAMPLE SETUP!!!!!!!!!!
dropout_rate=0.1
num_layers=2
future_steps = 30
time_steps = 1000
lstm_units = 64
num_batch = 64
num_epochs = 1
num_validation=0.2
num_verbose=1
es_patinence = 5

#Variables
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

print(X_train_arr.shape) #(2494, 100, 4) <-- train data, having now 2494 rows, with 100 time steps, each row has 4 features (MANY)
print(y_train_arr.shape) #(2494, 100, 4) <-- target data, having now 2494 rows, with 100 time step, but 4 features (TO MANY)

#Split data
X_train_splitted = X_train_arr[:split] #(80%) model train input data
y_train_splitted = y_train_arr[:split] #80%) model train target data
X_test_splitted = X_train_arr[split:] #(20%) test prediction input data
y_test_splitted = y_train_arr[split:] #(20%) test prediction compare data

#Reshaping to rows/time_steps/columns
X_train_splitted = np.reshape(X_train_splitted, (X_train_splitted.shape[0], time_steps, features))
y_train_splitted = np.reshape(y_train_splitted, (y_train_splitted.shape[0], time_steps, features))
X_test_splitted = np.reshape(X_test_splitted, (X_test_splitted.shape[0], time_steps, features))
y_test_splitted = np.reshape(y_test_splitted, (y_test_splitted.shape[0], time_steps, features))

print(X_train_arr.shape) #(2494, 100, 4)
print(y_train_arr.shape) #(2494, 100, 4)
print(X_test_splitted.shape) #(450, 100, 4)
print(y_test_splitted.shape) #(450, 100, 4)

#Initialize the RNN
model = Sequential()

#Add Bidirectional LSTM, has better performance than stacked LSTM
model = Sequential()
for i in range(num_layers):
        model.add(Bidirectional(LSTM(lstm_units, activation='relu', return_sequences=True)))
        #model.add(Dropout(rate=dropout_rate))
model.add(Dense(features)) #4 outputs, gives output shape (x, 100, 4)

#Compile many-to-many
model.compile(optimizer='adam', loss = 'mae', metrics=['mae', 'acc', 'mse'])

#Fit to the training set
es = EarlyStopping(monitor='val_mae', mode='min', patience=es_patinence)
model.fit(X_train_splitted, y_train_splitted, epochs=num_epochs, batch_size=num_batch, validation_split=num_validation, verbose=num_verbose, callbacks=[es])

#Evaluating
eval = model.evaluate(X_test_splitted, y_test_splitted, batch_size=num_batch, verbose=num_verbose)
print('Evaluation: ' + str(eval))

#Predicting future
def predict_future():
    global X_test_splitted
    pred = model.predict(X_test_splitted, verbose=num_verbose) #predict from updated X_test_splitted
    last_pred_item = pred[-1]
    X_test_splitted_shape = X_test_splitted.shape
    X_test_splitted = X_test_splitted = np.append(X_test_splitted, last_pred_item)
    X_test_splitted = np.reshape(X_test_splitted, (X_test_splitted_shape[0] + 1, time_steps, features)) #reshape X_test_splitted to (x + 1, 100, 4)
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

#Combine future time_steps based prediction results with initial prediction based on test data and reshape
y_pred = np.append(y_pred, future_results) #combine initial y_pred result array with predicted y_pred_future
y_pred = np.reshape(y_pred, (y_test_splitted.shape[0] + future_steps, time_steps, features)) #reshaping to (450 + 33, 100, 4)

#Reshaping data for reverse transforming
y_pred = np.reshape(y_pred[:, -1:, :], (y_pred.shape[0], features))
y_test_splitted = np.reshape(y_test_splitted[:, -1:, :], (y_test_splitted.shape[0], features))

#Reversing transform to get proper data values
y_test_splitted = scaler.inverse_transform(y_test_splitted)
y_pred = scaler.inverse_transform(y_pred)

#Plot data
plt.figure(figsize=(14,features))
plt.plot(y_test_splitted[-time_steps:, 3], label = "Real values") #I am interested only with display of column index 3
plt.plot(y_pred[-time_steps - future_steps:, 3], label = 'Predicted values') #I am interested only with display of column index 3
plt.title('Prediction test')
plt.xlabel('Time')
plt.ylabel('Column index 3')
plt.legend()
plt.show()

#Evaluation results (loss, mae, acc, mse)