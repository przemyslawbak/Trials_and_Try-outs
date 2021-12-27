import numpy as np
import pandas as pd
import datetime
from sklearn.preprocessing import MinMaxScaler
from tensorflow.keras import Sequential
from tensorflow.keras.layers import Dense, LSTM, Dropout
import matplotlib.pyplot as plt

#path to the data file
csv_path = 'GPW_DLY WIG20, 15.csv'
data = pd.read_csv(csv_path)

df = data.drop(['time'], axis = 1)

scaler = MinMaxScaler()
scaled_data = scaler.fit_transform(df)

X = []
y = []
split = 2100
test = 60

for i in range(test, scaled_data.shape[0]):
    X.append(scaled_data [i-test:i])
    y.append(scaled_data [i, 0])

X, y = np.array(X), np.array(y)

X_train = X[:split]
y_train = y[:split]
X_test = X[split:]
y_test = y[split:]

model = Sequential()
model.add(LSTM(units= 20, activation = 'relu',\
return_sequences = True,\
input_shape = (X_train.shape[1], X_train.
shape[2])))
model.add(Dropout(0.5))
model.add(LSTM(units= 40, \
activation = 'relu', \
return_sequences = True))
model.add(Dropout(0.5))
model.add(LSTM(units= 80, \
activation = 'relu'))
model.add(Dropout(0.5))
model.add(Dense(units = 1))

model.compile(optimizer='adam', loss = 'mean_squared_error')
model.fit(X_train, y_train, epochs=5, batch_size=32)
y_pred = model.predict(X_test)

plt.plot(y_test[-test:], color = 'black')
plt.plot(y_pred[-test:], color = 'blue')
plt.show()