#many-to-many according to this: https://stackoverflow.com/a/43047615/12603542

import pandas as pd
import matplotlib.pyplot as plt
import numpy as np
from sklearn.preprocessing import MinMaxScaler
from tensorflow.keras import Sequential
from tensorflow.keras.layers import Dense, LSTM, Dropout, TimeDistributed, Activation, RepeatVector, Bidirectional
from tensorflow.keras.callbacks import EarlyStopping
import models

#Import the training dataset
filename = "GPW_DLY WIG20, 15.csv"
dataset_train = pd.read_csv(filename, sep=';')
dataset_train = dataset_train.iloc[::-1]
dataset_train = dataset_train.drop('week', axis=1)
dataset_train = dataset_train.drop('from', axis=1)
dataset_train = dataset_train.drop('to', axis=1)
dataset_train = dataset_train.drop('delta', axis=1)

training_set = dataset_train
#training_set["minute"] = dataset_train['DateTime'].map(lambda x: x.minute)

#Perform feature scaling to transform the data
scaler = MinMaxScaler(feature_range = (0, 1))
training_set_scaled = scaler.fit_transform(training_set)

#Model values !!!!!!!!!SAMPLE SETUP!!!!!!!!!!
dropout_rate=0.2
num_layers=1
future_steps = 2
time_steps = 20
lstm_units = 500 #allow to learn very long sequences
num_batch = 16 #number of samples to work through before updating the internal model parameters
num_epochs = 1000 #number times that the learning algorithm will work through the entire training dataset (10, 100, 1000)
num_validation=0.2 #% split for validation set
num_verbose=1 #how to display model fit progress (0 = silent, 1 = progress bar, 2 = one line per epoch)
es_patinence = 50

#Variables
features = len(training_set.columns)
split_percent = 0.50 #train/test daa split percent (80%)
split = int(split_percent*len(training_set_scaled)) #split percent multiplying by data rows

def getTestSets():
    train_data = models.TrainSets()
    #Create a data structure with n-time steps
    X = []
    y = []
    for i in range(time_steps + 1, len(training_set_scaled)):
        X.append(training_set_scaled[i-time_steps-1:i-1, 0:features]) #take all columns into the set, including time_steps legth
        y.append(training_set_scaled[i-time_steps:i, 0:features]) #take all columns into the set, including time_steps legth

    X_train_arr, y_train_arr = np.array(X), np.array(y)

    #Split data
    X_train_splitted = X_train_arr[:split] #(50%) model train input data
    y_train_splitted = y_train_arr[:split] #50%) model train target data
    X_test_splitted = X_train_arr[split:] #(50%) test prediction input data
    y_test_splitted = y_train_arr[split:] #(50%) test prediction compare data

    #Reshaping to rows/time_steps/columns
    X_train_splitted = np.reshape(X_train_splitted, (X_train_splitted.shape[0], time_steps, features))
    y_train_splitted = np.reshape(y_train_splitted, (y_train_splitted.shape[0], time_steps, features))
    X_test_splitted = np.reshape(X_test_splitted, (X_test_splitted.shape[0], time_steps, features))
    y_test_splitted = np.reshape(y_test_splitted, (y_test_splitted.shape[0], time_steps, features))

    #fitting shapes to batch size
    remTestX = X_test_splitted.shape[0] % num_batch
    remTesty = y_test_splitted.shape[0] % num_batch
    remTrainX = X_train_splitted.shape[0] % num_batch
    remTrainy = y_train_splitted.shape[0] % num_batch
    
    #slicing if too long (must be dividable by batch size to use pre-trained model)
    if remTestX != 0:
        X_test_splitted = X_test_splitted[remTestX:,:,:]
    if remTesty != 0:
        y_test_splitted = y_test_splitted[remTesty:,:,:]
    if remTrainX != 0:
        X_train_splitted = X_train_splitted[remTrainX:,:,:]
    if remTrainy != 0:
        y_train_splitted = y_train_splitted[remTrainy:,:,:]

    train_data.X_train_splitted = X_train_splitted
    train_data.y_train_splitted = y_train_splitted
    train_data.X_test_splitted = X_test_splitted
    train_data.y_test_splitted = y_test_splitted

    return train_data

#Predicting future
def predict_future(train_data, model):
    remTestX = train_data.X_test_splitted.shape[0] % num_batch
    if remTestX != 0:
        train_data.X_test_splitted = train_data.X_test_splitted[remTestX:,:,:]
    pred = model.predict(train_data.X_test_splitted, verbose = num_verbose, batch_size = num_batch)
    last_pred_item = pred[-1]
    X_test_splitted_shape = train_data.X_test_splitted.shape
    train_data.X_test_splitted = train_data.X_test_splitted = np.append(train_data.X_test_splitted, last_pred_item)
    train_data.X_test_splitted = np.reshape(train_data.X_test_splitted, (X_test_splitted_shape[0] + 1, time_steps, features))
    return pred, train_data

def nextFutureStep(train_data, model):
    pred, train_data = predict_future(train_data, model)
    return pred[-1], train_data

def startPredictions(train_data, model):
    train_data.X_test_splitted[~np.isfinite(train_data.X_test_splitted)] = 0

    future_results = np.array([]) #initialize numpy array
    y_pred, train_data = predict_future(train_data, model) #create initial result array

    for next in range(future_steps):
        print('future step ' + str(next) + ' of ' + str(future_steps)) #display info
        new_item, train_data = nextFutureStep(train_data, model) #assign item
        future_results = np.append(future_results, new_item) #add last step to future_results prediction array

    #Combine future time_steps based prediction results with initial prediction based on test data and reshape
    y_pred = np.append(y_pred, future_results) #combine initial y_pred result array with predicted y_pred_future
    y_pred = np.reshape(y_pred, (train_data.y_test_splitted.shape[0] + future_steps, time_steps, features)) #reshaping

    #Reshaping data for reverse transforming
    y_pred = np.reshape(y_pred[:, -1:, :], (y_pred.shape[0], features))
    train_data.y_test_splitted = np.reshape(train_data.y_test_splitted[:, -1:, :], (train_data.y_test_splitted.shape[0], features))

    #Reversing transform to get proper data values
    train_data.y_test_splitted = scaler.inverse_transform(train_data.y_test_splitted)
    y_pred = scaler.inverse_transform(y_pred)

    return train_data.y_test_splitted, y_pred

def trainModel(train_data):
    #create model
    model = Sequential()
    for i in range(num_layers):
            model.add(Bidirectional(LSTM(lstm_units, activation='relu', return_sequences=True)))
            model.add(Dropout(rate=dropout_rate))
    model.add(Dense(features)) #4 outputs, gives output shape

    #Compile many-to-many
    #loss should decrease, acc should increase
    model.compile(optimizer='adam', loss = 'mae', metrics=['mae', 'acc', 'mse']) #MAE & MSE are popular for regression models
    
    #Fit to the training set
    es = EarlyStopping(monitor='val_mae', mode='min', patience=es_patinence, verbose = num_verbose)
    model.fit(train_data.X_train_splitted, train_data.y_train_splitted, epochs=num_epochs, batch_size=num_batch, validation_split=num_validation, verbose=num_verbose, callbacks=[es])
    
    return model

train_data = getTestSets()
model = trainModel(train_data)
y_test_splitted, y_pred = startPredictions(train_data, model)

#Plot data
plt.figure(figsize=(14,features))
plt.plot(y_test_splitted[-time_steps:, 1], label = "Real values") #I am interested only with display of column index 3
plt.plot(y_pred[-time_steps - future_steps:, 1], label = 'Predicted values') #I am interested only with display of column index 3
plt.title('Prediction test')
plt.xlabel('Time')
plt.ylabel('Column index 1')
plt.legend()
plt.show()