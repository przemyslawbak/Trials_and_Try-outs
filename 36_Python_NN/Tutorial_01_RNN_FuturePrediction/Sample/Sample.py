#https://towardsdatascience.com/time-series-forecasting-with-recurrent-neural-networks-74674e289816

import pandas as pd
import numpy as np
import keras
import tensorflow as tf
from keras.preprocessing.sequence import TimeseriesGenerator
from keras.models import Sequential
from keras.layers import LSTM, Dense, Dropout
import plotly.graph_objects as go

filename = "GPW_DLY WIG20, 15.csv"
df = pd.read_csv(filename)
print(df.info())

df['time'] = pd.to_datetime(df['time'])
df.set_axis(df['time'], inplace=True)
df.drop(columns=['open', 'high', 'low', 'Volume'], inplace=True)
close_data = df['close'].values
close_data = close_data.reshape((-1,1))

split_percent = 0.80
split = int(split_percent*len(close_data))
close_train = close_data[:split]
close_test = close_data[split:]
date_train = df['time'][:split]
date_test = df['time'][split:]

look_back = 15

train_generator = TimeseriesGenerator(close_train, close_train, length=look_back, batch_size=32) #targets missing 
test_generator = TimeseriesGenerator(close_test, close_test, length=look_back, batch_size=1) #targets missing

model = Sequential()
model.add(LSTM(units= 80, activation = 'relu', input_shape=(look_back,1)))
model.add(Dense(units = 1))
model.compile(optimizer='adam', loss='mse')

num_epochs = 25
model.fit(train_generator, epochs=num_epochs, verbose=1)

prediction = model.predict(test_generator)

close_train = close_train.reshape((-1))
close_test = close_test.reshape((-1))
prediction = prediction.reshape((-1))

#future forecating
close_data = close_data.reshape((-1))

def predict(num_prediction, model):
    prediction_list = close_data[-look_back:]
    
    for _ in range(num_prediction):
        x = prediction_list[-look_back:]
        x = x.reshape((1, look_back, 1))
        out = model.predict(x)[0][0]
        prediction_list = np.append(prediction_list, out)
    prediction_list = prediction_list[look_back-1:]
        
    return prediction_list
    
def predict_dates(num_prediction):
    last_date = df['time'].values[-1]
    prediction_dates = pd.date_range(last_date, periods=num_prediction+1).tolist()
    return prediction_dates

num_prediction = 30
forecast = predict(num_prediction, model)
forecast_dates = predict_dates(num_prediction)

#plotting
trace1 = go.Scatter(
    x = date_train,
    y = close_train,
    mode = 'lines',
    name = 'Data'
)
trace2 = go.Scatter(
    x = date_test,
    y = prediction,
    mode = 'lines',
    name = 'Prediction'
)
trace3 = go.Scatter(
    x = date_test,
    y = close_test,
    mode='lines',
    name = 'Ground Truth'
)
trace4 = go.Scatter(
    x = forecast_dates,
    y = forecast,
    mode='lines',
    name = 'Ground Truth'
)
layout = go.Layout(
    title = "WIG 20",
    xaxis = {'title' : "time"},
    yaxis = {'title' : "close"}
)

fig = go.Figure(data=[trace1, trace2, trace3, trace4], layout=layout)
fig.show()