import numpy as np
import pandas as pd
import datetime
from sklearn.preprocessing import MinMaxScaler
from tensorflow.keras import Sequential
from tensorflow.keras.layers import Dense, LSTM, Dropout

data = pd.read_csv("household_power_consumption.csv")

#Create a new column, Datetime, by combining Date and Time columns using
#the following code
data['Date'] = pd.to_datetime(data['Date'], format="%d/%m/%Y")
data['Datetime'] = data['Date'].dt.strftime('%Y-%m-%d') + ' ' \
+ data['Time']
data['Datetime'] = pd.to_datetime(data['Datetime'])

#Sort the DataFrame in ascending order using the Datetime column
data = data.sort_values(['Datetime'])

#Create a list called num_cols containing the columns that have numeric
#values – Global_active_power, Global_reactive_power, Voltage,
#Global_intensity, Sub_metering_1, Sub_metering_2, and
#Sub_metering_3
num_cols = ['Global_active_power', 'Global_reactive_power', \
'Voltage', 'Global_intensity', 'Sub_metering_1', \
'Sub_metering_2', 'Sub_metering_3']

#Convert all columns listed in num_cols to a numeric datatype
for col in num_cols:
    data[col] = pd.to_numeric(data[col], errors='coerce')

#Iterate through columns in num_cols and fill in missing values with the average
#using the following code
for col in num_cols:
    data[col].fillna(data[col].mean(), inplace=True)

#Use drop() to remove Date, Time, Global_reactive_power, and
#Datetime columns from your DataFrame and save the results in a variable
#called df
df = data.drop(['Date', 'Time', 'Global_reactive_power', 'Datetime'], axis = 1)

#Create a scaler from MinMaxScaler to your DataFrame to numbers between
#zero and one. Use fit_transform to fit the model to the data and then
#transform the data according to the fitted model
scaler = MinMaxScaler()
scaled_data = scaler.fit_transform(df)

#Create two empty lists called X and y that will be used to store features and
#target variables
X = []
y = []

#Create a training dataset that has the previous 60 minutes' power consumption
#so that you can predict the value for the next minute. Use a for loop to create
#data in 60 time steps
for i in range(60, scaled_data.shape[0]):
    X.append(scaled_data [i-60:i])
    y.append(scaled_data [i, 0])

#Convert X and y into NumPy arrays in preparation for training your model
X, y = np.array(X), np.array(y)

#Split the dataset into training and testing sets with data before and after the
#index 217440, respectively
X_train = X[:217440]
y_train = y[:217440]
X_test = X[217440:]
y_test = y[217440:]

#Initialize your neural network. Add LSTM layers with 20, 40, and 80 units.
#Use a ReLU activation function and set return_sequences to True. The
#input_shape should be the dimensions of your training set (the number of
#features and days). Finally, add your dropout layer
regressor = Sequential()
regressor.add(LSTM(units= 20, activation = 'relu',\
return_sequences = True,\
input_shape = (X_train.shape[1], X_train.
shape[2])))
regressor.add(Dropout(0.5))
regressor.add(LSTM(units= 40, \
activation = 'relu', \
return_sequences = True))
regressor.add(Dropout(0.5))
regressor.add(LSTM(units= 80, \
activation = 'relu'))
regressor.add(Dropout(0.5))
regressor.add(Dense(units = 1))

#Use the compile() method to configure your model for training. Select Adam
#as your optimizer and mean squared error to measure your loss function
regressor.compile(optimizer='adam', loss = 'mean_squared_error')

#Fit your model and set it to run on two epochs. Set your batch size to 32
regressor.fit(X_train, y_train, epochs=2, batch_size=32)

#Save the predictions
y_pred = regressor.predict(X_test)

#Take a look at the real household power consumption and your predictions
plt.figure(figsize=(14,5))
plt.plot(y_test[-60:], color = 'black', \
label = "Real Power Consumption")
plt.plot(y_pred[-60:], color = 'gray', \
label = 'Predicted Power Consumption')
plt.title('Power Consumption Prediction')
plt.xlabel('time')
plt.ylabel('Power Consumption')
plt.legend()
plt.show()