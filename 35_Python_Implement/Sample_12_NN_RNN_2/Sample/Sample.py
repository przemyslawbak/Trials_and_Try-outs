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

#future
future_dates=[df.index[-1]+ x for x in range(0,futures)]
future_df=pd.DataFrame(index=future_dates[1:],columns=df.columns)
future_df.tail()

#results
model.compile(optimizer='adam', loss = 'mean_squared_error')
resut_df=pd.concat([df,future_df])
resut_df['forecast'] = model_fit.predict(start = len(df.index), end = len(df.index) + futures, dynamic= True)
resut_df[['close', 'forecast']].plot(figsize=(12, 8))



model.fit(X_train, y_train, epochs=5, batch_size=32)#
y_pred = model.predict(X_test)#

#reverse scale resuts
res_scaler = MinMaxScaler()
res_scaler.min_, res_scaler.scale_ = scaler.min_[0], scaler.scale_[0]
y_pred = res_scaler.inverse_transform(y_pred)#
y_test = res_scaler.inverse_transform(y_test.reshape(-1,1))#

plt.plot(y_test[-test:], color = 'black')#
plt.plot(y_pred[-test:], color = 'blue')
plt.show()