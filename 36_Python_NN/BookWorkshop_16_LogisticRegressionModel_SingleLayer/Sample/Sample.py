import tensorflow as tf
import pandas as pd

df = pd.read_csv('qsar_androgen_receptor.csv', sep=';')
df.dropna(inplace=True)

target = df['positive'].apply(lambda x: 1 if x=='positive' else 0)
features = df.drop('positive', axis=1)

model = tf.keras.Sequential()
model.add(tf.keras.layers.InputLayer(input_shape=(features.shape[1],), name='Input_layer'))

model.add(tf.keras.layers.Dense(1, name='Output_layer', activation='sigmoid'))

model.compile(tf.optimizers.RMSprop(0.0001), loss='binary_crossentropy', metrics=['accuracy'])

tensorboard_callback = tf.keras.callbacks.TensorBoard(log_dir="./logs")

model.fit(x=features.to_numpy(), y=target.to_numpy(), epochs=50, callbacks=[tensorboard_callback], validation_split=0.2)

loss, accuracy = model.evaluate(features.to_numpy(), target.to_numpy())
print(f'loss: {loss}, accuracy: {accuracy}')