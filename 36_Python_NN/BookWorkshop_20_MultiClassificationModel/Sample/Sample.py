import tensorflow as tf
import pandas as pd
from tensorflow.keras.layers import Dense

train_url = 'shuttle.trn'
test_url = 'shuttle.tst'

X_train = pd.read_table(train_url, header=None, sep=' ')
X_train.head()
#Extract the target variable (column 9) using the pop() method and save it
y_train = X_train.pop(9)
X_test = pd.read_table(test_url, header=None, sep=' ')
X_test.head()
#Extract the target variable (column 9) using the pop() method and save it
y_test = X_test.pop(9)

#Set the seed for TensorFlow as 8 using tf.random.set_seed()
tf.random.set_seed(8)

model = tf.keras.Sequential()
fc1 = Dense(512, input_shape=(9,), activation='relu')
fc2 = Dense(512, activation='relu')
fc3 = Dense(128, activation='relu')
fc4 = Dense(128, activation='relu')
#softmax activator for multi classification model
fc5 = Dense(8, activation='softmax')

model.add(fc1)
model.add(fc2)
model.add(fc3)
model.add(fc4)
model.add(fc5)

#loss for multi classification model
loss = tf.keras.losses.SparseCategoricalCrossentropy()

optimizer = tf.keras.optimizers.Adam(0.001)

model.compile(optimizer=optimizer, loss=loss, \
metrics=['accuracy'])
model.fit(X_train, y_train, epochs=5)
model.evaluate(X_test, y_test)
