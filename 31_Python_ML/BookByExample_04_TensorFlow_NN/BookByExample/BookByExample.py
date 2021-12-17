import tensorflow as tf
from tensorflow import keras
import numpy as np
from sklearn import preprocessing
#p. 266

data = np.array([9, 11, 16, 24, 18, 14, 17, 19, 13, 17, 22, 24, 28, 34, 42, 40, 41, 47, 32, 16, 8, 11, 13, 17, 22, 18, 16, 14, 13, 11, 10, 9, 4, 7, 9, 5, 11, 17, 19, 23, 25, 38, 45])
print(data)

n = len(data)
train_df = data[0:int(n*0.7)]
val_df = data[int(n*0.7):int(n*0.9)]
test_df = data[int(n*0.9):]

print('df len: ' + str(len(data))) #43
print('train_df len: ' + str(len(train_df))) #30
print('val_df len: ' + str(len(val_df))) #8
print('test_df len: ' + str(len(test_df))) #5

model = keras.Sequential([
    keras.layers.Dense(units=20, activation='relu'),
    keras.layers.Dense(units=8, activation='relu'),
    keras.layers.Dense(units=1)
])
model.compile(loss=tf.losses.MeanSquaredError(),
                optimizer=tf.optimizers.Adam(),
                metrics=[tf.metrics.MeanAbsoluteError()])

model.fit(train_df, epochs=300, validation_data=val_df.any())

predictions = model.predict(X_test)[:, 0]

print(predictions)