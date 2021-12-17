#p. 266

import tensorflow as tf
from tensorflow import keras
tf.random.set_seed(42)

model = keras.Sequential([
    keras.layers.Dense(units=20, activation='relu'),
    keras.layers.Dense(units=8, activation='relu'),
    keras.layers.Dense(units=1)
])
model.compile(loss='mean_squared_error', optimizer=tf.keras.optimizers.Adam(0.02))
model.fit(X_train, y_train, epochs=300)

predictions = model.predict(X_test)[:, 0]

print(predictions)