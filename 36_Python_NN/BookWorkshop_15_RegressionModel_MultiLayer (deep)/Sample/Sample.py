import tensorflow as tf
import pandas as pd

df = pd.read_csv('Bias_correction_ucl.csv')

df.drop('Date', inplace=True, axis=1)
df.dropna(inplace=True)

target = df[['Next_Tmax', 'Next_Tmin']]
features = df.drop(['Next_Tmax', 'Next_Tmin'], axis=1)

from sklearn.preprocessing import MinMaxScaler
scaler = MinMaxScaler()
feature_array = scaler.fit_transform(features)
features = pd.DataFrame(feature_array, columns=features.columns)

model = tf.keras.Sequential()

#single input layer
model.add(tf.keras.layers.InputLayer\
(input_shape=(features.shape[1],), \
name='Input_layer'))

#several hidden layers
model.add(tf.keras.layers.Dense(16, name='Dense_layer_1'))
model.add(tf.keras.layers.Dense(8, name='Dense_layer_2'))
model.add(tf.keras.layers.Dense(4, name='Dense_layer_3'))
model.add(tf.keras.layers.Dense(2, name='Output_layer'))

#For linear regression, the most frequently used loss functions are mean squared
#error and mean absolute error
model.compile(tf.optimizers.RMSprop(0.001), loss='mse')

tensorboard_callback = tf.keras.callbacks.TensorBoard(log_dir="./logs")

model.fit(x=features.to_numpy(), y=target.to_numpy(),\
epochs=50, callbacks=[tensorboard_callback] , \
validation_split=0.2)

loss = model.evaluate(features.to_numpy(), target.to_numpy())
print('loss:', loss)

