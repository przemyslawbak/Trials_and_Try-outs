import tensorflow as tf
import pandas as pd
from sklearn.preprocessing import StandardScaler

df = pd.read_csv('superconductivity.csv')
df.dropna(inplace=True)

target = df['critical_temp'].apply(lambda x: 1 if x>77.36 else 0)
features = df.drop('critical_temp', axis=1)

scaler = StandardScaler()
feature_array = scaler.fit_transform(features)
features = pd.DataFrame(feature_array, columns=features.columns)

model = tf.keras.Sequential()
model.add(tf.keras.layers.InputLayer\
(input_shape=features.shape[1], \
name='Input_layer'))
model.add(tf.keras.layers.Dense(32, name='Hidden_layer_1'))
model.add(tf.keras.layers.Dense(16, name='Hidden_layer_2'))
model.add(tf.keras.layers.Dense(8, name='Hidden_layer_3'))
#applying the sigmoid function on the output of a linear regression model turns
#it into logistic regression
model.add(tf.keras.layers.Dense(1, name='Output_layer', \
activation='sigmoid'))

model.compile(tf.optimizers.RMSprop(0.0001), \
loss= 'binary_crossentropy', metrics=['accuracy'])

tensorboard_callback = tf.keras.callbacks.TensorBoard\
(log_dir="./logs")

model.fit(x=features.to_numpy(), y=target.to_numpy(),\
epochs=50, callbacks=[tensorboard_callback],\
validation_split=0.2)

loss, accuracy = model.evaluate(features.to_numpy(), \
target.to_numpy())
print(f'loss: {loss}, accuracy: {accuracy}')

