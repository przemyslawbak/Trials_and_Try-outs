import numpy as np
import pandas as pd
import datetime
from tensorflow.keras import Sequential
from tensorflow.keras.layers import Dense, LSTM, Dropout
from keras.preprocessing.sequence import TimeseriesGenerator
import matplotlib.pyplot as plt

#path to the data file
csv_path = 'GPW_DLY WIG20, 15.csv'
data = pd.read_csv(csv_path)

df = data.drop(['time'], axis = 1)

#splitting df
look_back=15
split_percent = 0.80
split = int(split_percent*len(df))
data_train = df[:split]
data_test = df[split:]

#creating time series generators
train_generator = TimeseriesGenerator(close_train, close_train, length=look_back, batch_size=20)     
test_generator = TimeseriesGenerator(close_test, close_test, length=look_back, batch_size=1)

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


plt.show()