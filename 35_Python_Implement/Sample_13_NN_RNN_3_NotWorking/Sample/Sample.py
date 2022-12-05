import numpy as np
import pandas as pd
import datetime
from sklearn.preprocessing import MinMaxScaler
from tensorflow.keras import Sequential
from tensorflow.keras.layers import Dense, LSTM, Dropout
import matplotlib.pyplot as plt
from keras.preprocessing.sequence import TimeseriesGenerator

#path to the data file
csv_path = 'GPW_DLY WIG20, 15.csv'
df = pd.read_csv(csv_path)

df = df.drop(['time'], axis = 1)
df.fillna(0)
df['forecast'] = 0
split_percent = 0.80
split = int(split_percent*len(df))
num_steps = 50
num_batch = 32
num_epochs = 5
num_verbose = 1
future_prediction = 30
look_back = 15

#splitting data
X_train = df[:split] #close_train
y_train = df[:split] #date_train
X_test = df[split:] #close_test
y_test = df[split:] #date_test

#generating time series
train_generator = TimeseriesGenerator(X_train, X_train, length=look_back, batch_size=num_batch) #targets missing
test_generator = TimeseriesGenerator(X_test, X_test, length=look_back, batch_size=1) #targets missing

model = Sequential()
model.add(LSTM(units= 20, activation = 'relu',\
return_sequences = True, input_shape=(look_back,1)))
model.add(Dropout(0.5))
model.add(LSTM(units= 40, \
activation = 'relu', \
return_sequences = True, input_shape=(look_back,1)))
model.add(Dropout(0.5))
model.add(LSTM(units= 80, \
activation = 'relu', input_shape=(look_back,1)))
model.add(Dropout(0.5))
model.add(Dense(units = 1))

model.compile(optimizer='adam', loss = 'mean_squared_error')
model.fit(train_generator,
                    steps_per_epoch=num_steps,
                    epochs=num_epochs,
                    verbose=num_verbose)

#future
future_data=[X_test.index[-1]+ x for x in range(0,future_prediction)]
future_df=pd.DataFrame(index=future_data[1:],columns=X_test.columns)
future_df.tail()
X_test=pd.concat([X_test,future_df])

y_pred = model.predict(X_test)

#results
resut_df['forecast'] = model.predict(start = len(df.index), end = len(df.index) + future_prediction, dynamic= True)
resut_df[['close', 'forecast']].plot(figsize=(12, 8))
plt.show()

