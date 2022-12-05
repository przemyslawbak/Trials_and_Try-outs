import tensorflow as tf
import pandas as pd
from tensorflow.keras.layers import Dense
#comparing 2 models
#1 model without regularization (L2)
#2 model with regularization

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

model.add(fc1)
model.add(fc2)
model.add(fc3)
model.add(fc4)
model.add(fc5)

loss = tf.keras.losses.SparseCategoricalCrossentropy()
optimizer = tf.keras.optimizers.Adam(0.001)
model.compile(optimizer=optimizer, loss=loss, \
metrics=['accuracy'])
model.fit(data, target, epochs=5, validation_split=0.2)

#Create five fully connected layers similar to the previous model's and specify
#the L2 regularizer for the kernel_regularizer parameters. Use the
#value 0.001 for the regularizer factor.
reg_fc1 = Dense(512, input_shape=(42,), activation='relu', \
kernel_regularizer=tf.keras.regularizers\
.l2(l=0.1))
reg_fc2 = Dense(512, activation='relu', \
kernel_regularizer=tf.keras.regularizers\
.l2(l=0.1))
reg_fc3 = Dense(128, activation='relu', \
kernel_regularizer=tf.keras.regularizers\
.l2(l=0.1))
reg_fc4 = Dense(128, activation='relu', \
kernel_regularizer=tf.keras.regularizers\
.l2(l=0.1))
reg_fc5 = Dense(3, activation='softmax')

model2 = tf.keras.Sequential()
model2.add(reg_fc1)
model2.add(reg_fc2)
model2.add(reg_fc3)
model2.add(reg_fc4)
model2.add(reg_fc5)

model2.compile(optimizer=optimizer, loss=loss, \
metrics=['accuracy'])
model2.fit(data, target, epochs=5, validation_split=0.2)

#With the addition of L2 regularization, the model now has similar accuracy scores
#between the training (0.68) and test (0.58) sets. The model is not overfitting as
#much as before, but its performance is not great.