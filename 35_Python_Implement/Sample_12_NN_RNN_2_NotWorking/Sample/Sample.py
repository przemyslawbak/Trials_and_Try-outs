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
df.fillna(0)
df['forecast'] = 0
split_percent = 0.80
split = int(split_percent*len(df))
test = 60
num_batch = 32
num_epochs = 5
future_prediction = 30

#scaling
scaler = MinMaxScaler()
scaled_data = scaler.fit_transform(df)
X = []
y = []
for i in range(test, scaled_data.shape[0]):
    X.append(scaled_data [i-test:i])
    y.append(scaled_data [i, 0])

#splitting data
X, y = np.array(X), np.array(y)
X_train = X[:split] #close_train
y_train = y[:split] #date_train
X_test = X[split:] #close_test
y_test = y[split:] #date_test

model = Sequential()
model.add(LSTM(units= 20, activation = 'relu',\
return_sequences = True))
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
model.fit(X_train, y_train, epochs=num_epochs, batch_size=num_batch)

#future
future_data=[X_test.index[-1]+ x for x in range(0,future_prediction)]
future_df=pd.DataFrame(index=future_data[1:],columns=X_test.columns)
future_df.tail()
X_test=pd.concat([X_test,future_df])

y_pred = model.predict(X_test)#

#reverse scale resuts
res_scaler = MinMaxScaler()
res_scaler.min_, res_scaler.scale_ = scaler.min_[0], scaler.scale_[0]

y_pred = res_scaler.inverse_transform(y_pred)
y_test = res_scaler.inverse_transform(y_test.reshape(-1,1))

#results
resut_df['forecast'] = model.predict(start = len(df.index), end = len(df.index) + future_prediction, dynamic= True)
resut_df[['close', 'forecast']].plot(figsize=(12, 8))
plt.show()

