import tensorflow as tf
import pandas as pd
from tensorflow.keras.layers import Dense

file_url = 'connect-4.csv'
data = pd.read_csv(file_url)
target = data.pop('class')
tf.random.set_seed(8)

model = tf.keras.Sequential()
fc1 = Dense(512, input_shape=(42,), activation='relu')
fc2 = Dense(512, activation='relu')
fc3 = Dense(128, activation='relu')
fc4 = Dense(128, activation='relu')
fc5 = Dense(3, activation='softmax')

#Sequentially add all five fully connected layers to the model with a dropout layer
#of 0.75 in between each of them using the add() method
model.add(fc1)
model.add(Dropout(0.75))
model.add(fc2)
model.add(Dropout(0.75))
model.add(fc3)
model.add(Dropout(0.75))
model.add(fc4)
model.add(Dropout(0.75))
model.add(fc5)

loss = tf.keras.losses.SparseCategoricalCrossentropy()
optimizer = tf.keras.optimizers.Adam(0.001)
model.compile(optimizer=optimizer, loss=loss, \
metrics=['accuracy'])
model.fit(data, target, epochs=5, validation_split=0.2)

#With the addition of dropout, the model now has similar accuracy scores
#between the training (0.69) and test (0.59) sets