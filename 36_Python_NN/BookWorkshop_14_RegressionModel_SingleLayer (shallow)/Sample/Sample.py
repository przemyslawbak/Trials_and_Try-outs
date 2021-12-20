import tensorflow as tf
import pandas as pd
from sklearn.preprocessing import MinMaxScaler

#Load in the dataset using the pandas read_csv function
df = pd.read_csv('Bias_correction_ucl.csv')

#Drop the date column and drop any rows that have null values
df.drop('Date', inplace=True, axis=1)
df.dropna(inplace=True)

#Create target and feature datasets
target = df[['Next_Tmax', 'Next_Tmin']]
features = df.drop(['Next_Tmax', 'Next_Tmin'], axis=1)

#Rescale the feature dataset
scaler = MinMaxScaler()
feature_array = scaler.fit_transform(features)
features = pd.DataFrame(feature_array, columns=features.columns)

#Initialize a Keras model of the Sequential class
model = tf.keras.Sequential()

#Add layers
model.add(tf.keras.layers.InputLayer(input_shape=(features.shape[1],), name='Input_layer'))
model.add(tf.keras.layers.Dense(2, name='Output_layer'))

#Compile the model with an RMSprop optimizer and a mean squared error loss
model.compile(tf.optimizers.RMSprop(0.001), loss='mse')

#Add a callback for TensorBoard
tensorboard_callback = tf.keras.callbacks.TensorBoard(log_dir="./logs")

#Fit the model to the training data
model.fit(x=features.to_numpy(), y=target.to_numpy(), epochs=50, callbacks=[tensorboard_callback])

#Evaluate the model on the training data
loss = model.evaluate(features.to_numpy(), target.to_numpy())
print('loss:', loss)

