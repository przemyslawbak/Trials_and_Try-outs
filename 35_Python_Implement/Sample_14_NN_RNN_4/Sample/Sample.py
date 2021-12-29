import tensorflow as tf
import pandas as pd
import matplotlib.pyplot as plt
import numpy as np
from sklearn.preprocessing import MinMaxScaler
from tensorflow.keras.models import Sequential
from tensorflow.keras.layers import Dense
from tensorflow.keras.layers import LSTM

#variables
LABEL_WIDTH = 100
NUM_BATCH = 32
MAX_EPOCHS = 20 #more ok

#downloading data
filename = "GPW_DLY WIG20, 15.csv"
df = pd.read_csv(filename)
df = df.drop(['time'], axis = 1)
df.fillna(0)

#splitting dataset into train and test split
n = len(df)
train_df = df[0:int(n*0.7)]
target_df = train_df['close'].shift(-1).fillna(0)
val_df = df[int(n*0.7):int(n*0.9)]
test_df = df[int(n*0.9):]

#creating data set
def make_dataset(data):
  data = np.array(data, dtype=np.float32)
  ds = tf.keras.preprocessing.timeseries_dataset_from_array(
      data=data,
      targets=None,
      sequence_stride=1,
      sequence_length=LABEL_WIDTH,
      shuffle=False,
      batch_size=NUM_BATCH)

  return ds

train_data = make_dataset(train_df)
target_data = make_dataset(target_df)
val_data = make_dataset(val_df)
test_data = make_dataset(test_df)

# Create the Stacked LSTM model
model=Sequential()
model.add(LSTM(50,return_sequences=True))
model.add(LSTM(50,return_sequences=True))
model.add(LSTM(50))
model.add(Dense(1))
model.compile(loss='mean_squared_error',optimizer='adam')

#compile and fit model
def compile_and_fit(model, data, targets, validation, patience=2):
  early_stopping = tf.keras.callbacks.EarlyStopping(monitor='val_loss',
                                                    patience=patience,
                                                    mode='min')
  model.compile(loss=tf.losses.MeanSquaredError(), optimizer=tf.optimizers.Adam(), metrics=[tf.metrics.MeanAbsoluteError()])
  history = model.fit(data, targets, epochs=MAX_EPOCHS, validation_data=validation, callbacks=[early_stopping])
  return history

history_lstm = compile_and_fit(model, train_data, target_data, val_data, 2)

# Lets Do the prediction and check performance metrics
train_predict=model.predict(X_train)
test_predict=model.predict(X_test)

train_predict=scaler.inverse_transform(train_predict)
test_predict=scaler.inverse_transform(test_predict)

# demonstrate prediction for next 100 steps
x_input=test_data[341:].reshape(1,-1)
x_input.shape
temp_input=list(x_input)
temp_input=temp_input[0].tolist()

lst_output=[]
n_steps=100
i=0
while(i<30):
    if(len(temp_input)>n_steps):
        x_input=np.array(temp_input[1:])
        x_input=x_input.reshape(1,-1)
        x_input = x_input.reshape((1, n_steps, 1))
        yhat = model.predict(x_input, verbose=0)
        temp_input.extend(yhat[0].tolist())
        temp_input=temp_input[1:]
        lst_output.extend(yhat.tolist())
        i=i+1
    else:
        x_input = x_input.reshape((1, n_steps,1))
        yhat = model.predict(x_input, verbose=0)
        temp_input.extend(yhat[0].tolist())
        lst_output.extend(yhat.tolist())
        i=i+1

#plot future prediction
day_new=np.arange(1,101)
day_pred=np.arange(101,131)
plt.plot(day_new,scaler.inverse_transform(df1[1158:]))
plt.plot(day_pred,scaler.inverse_transform(lst_output))
plt.show()