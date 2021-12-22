import numpy as np
import matplotlib.pyplot as plt
import pandas as pd
from sklearn.preprocessing import StandardScaler, MinMaxScaler
import io
from tensorflow.keras import Sequential
from tensorflow.keras.layers import Input, Dense, Dropout

data = pd.read_csv('NVDA.csv')



#Now, split the training data. Use all data that is older than 2019-01-01 using
#the Date column for your training data. Save it as data_training. Save this
#in a separate file by using the copy() method
data_training = data[data['Date']<'2019-01-01'].copy()

#Now, split the test data. Use all data that is more recent than or equal to
#2019-01-01 using the Date column. Save it as data_test.
data_test = data[data['Date']>='2019-01-01'].copy()

#Use drop() to remove your Date and Adj Close columns in your
#DataFrame. Remember that you used the Date column to split your training and
#test sets, so the date information is not needed.
training_data = data_training.drop\
(['Date', 'Adj Close'], axis = 1)

#Create a scaler from MinMaxScaler to scale training_data to numbers
#between zero and one. Use the fit_transform function to fit the model to
#the data and then transform the data according to the fitted model
scaler = MinMaxScaler()
training_data = scaler.fit_transform(training_data)

X_train = []
y_train = []

#Check the shape of training_data:
training_data.shape[0] #You can see there are 868 observations in the training set.

#Create a training dataset that has the previous 60 days' stock prices so that you
#can predict the closing stock price for day 61. Here, X_train will have two
#columns. The first column will store the values from 0 to 59, and the second will
#store values from 1 to 60. In the first column of y_train, store the 61st value at
#index 60, and in the second column, store the 62nd value at index 61. Use a for
#loop to create data in 60 time steps

for i in range(60, training_data.shape[0]):
    X_train.append(training_data[i-60:i])
    y_train.append(training_data[i, 0])

#Convert X_train and y_train into NumPy arrays
X_train, y_train = np.array(X_train), np.array(y_train)

#Transform the data into a 2D matrix with the shape of the sample (the number
#of samples and the number of features in each sample). Stack the features for
#all 60 days on top of each other to get an output size of (808, 300). Use the
#following code for this purpose
X_old_shape = X_train.shape
X_train = X_train.reshape(X_old_shape[0], \
X_old_shape[1]*X_old_shape[2])

#Initialize the neural network by calling regressor_ann = Sequential()
regressor_ann = Sequential()

#Add an input layer with shape as 300
regressor_ann.add(Input(shape = (300,)))

#Then, add the first dense layer. Set it to 512 units, which will be your
#dimensionality for the output space. Use a ReLU activation function. Finally,
#add a dropout layer that will remove 20% of the units during training to
#prevent overfitting
regressor_ann.add(Dense(units = 512, activation = 'relu'))
regressor_ann.add(Dropout(0.2))
#Add another dense layer with 128 units, ReLU as the activation function, and a
#dropout of 0.3
regressor_ann.add(Dense(units = 128, activation = 'relu'))
regressor_ann.add(Dropout(0.3))
#Add another dense layer with 64 units, ReLU as the activation function, and a
#dropout of 0.4
regressor_ann.add(Dense(units = 64, activation = 'relu'))
regressor_ann.add(Dropout(0.4))
#Again, add another dense layer with 128 units, ReLU as the activation function,
#and a dropout of 0.5
regressor_ann.add(Dense(units = 16, activation = 'relu'))
regressor_ann.add(Dropout(0.5))
#Add a final dense layer with one unit
regressor_ann.add(Dense(units = 1))

#Use the compile() method to configure your model for training. Choose Adam
#as your optimizer and mean squared error to measure your loss function
regressor_ann.compile(optimizer='adam', \
loss = 'mean_squared_error')

#Finally, fit your model and set it to run on 10 epochs. Set your batch size to 32
regressor_ann.fit(X_train, y_train, epochs=10, batch_size=32)

#Use the tail(60) method to create a past_60_days variable, which consists
#of the last 60 days of data in the training set. Add the past_60_days variable
#to the test data with the append() function. Assign True to ignore_index
past_60_days = data_training.tail(60)
df = past_60_days.append(data_test, ignore_index = True)

#Now, prepare your test data for predictions by repeating what you did for the
#training data in previous steps
df = df.drop(['Date', 'Adj Close'], axis = 1)
inputs = scaler.transform(df)
X_test = []
y_test = []
for i in range(60, inputs.shape[0]):
    X_test.append(inputs[i-60:i])
    y_test.append(inputs[i, 0])
X_test, y_test = np.array(X_test), np.array(y_test)
X_old_shape = X_test.shape
X_test = X_test.reshape(X_old_shape[0], \
X_old_shape[1] * X_old_shape[2])
X_test.shape, y_test.shape

#Test some predictions for your stock prices by calling the predict() method
#on X_test
y_pred = regressor_ann.predict(X_test)

#Before looking at the results, reverse the scaling you did earlier so that
#the number you get as output will be at the correct scale using the
#StandardScaler utility class that you imported with scaler.scale_
scaler.scale_

#Use the first value in the preceding array to set your scale in preparation for the
#multiplication of y_pred and y_test. Recall that you are converting your data
#back from your earlier scale, in which you converted all values to between zero
#and one:
scale = 1/3.70274364e-03

#Multiply y_pred and y_test by scale to convert your data back to the
#proper values
y_pred = y_pred*scale
y_test = y_test*scale

#Review the real Nvidia stock price and your predictions
plt.figure(figsize=(14,5))
plt.plot(y_test, color = 'black', label = "Real NVDA Stock Price")
plt.plot(y_pred, color = 'gray',\
label = 'Predicted NVDA Stock Price')
plt.title('NVDA Stock Price Prediction')
plt.xlabel('time')
plt.ylabel('NVDA Stock Price')
plt.legend()
plt.show()

#COPNCLUSION
#In the preceding graph, you can see that your trained model is able to capture
#some of the trends of the Nvidia stock price. Observe that the predictions are
#quite different from the real values. It is evident from this result that ANNs are
#not suited for sequential data