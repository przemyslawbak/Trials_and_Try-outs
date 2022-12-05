#https://www.tensorflow.org/tutorials/keras/save_and_load#hdf5_format
import os
import tensorflow as tf
from tensorflow import keras
print(tf.version.VERSION)

#Get an example dataset
(train_images, train_labels), (test_images, test_labels) = tf.keras.datasets.mnist.load_data()

train_labels = train_labels[:1000]
test_labels = test_labels[:1000]

train_images = train_images[:1000].reshape(-1, 28 * 28) / 255.0
test_images = test_images[:1000].reshape(-1, 28 * 28) / 255.0

#Define a model
def create_model():
  model = tf.keras.Sequential([
    keras.layers.Dense(512, activation='relu', input_shape=(784,)),
    keras.layers.Dropout(0.2),
    keras.layers.Dense(10)
  ])

  model.compile(optimizer='adam',
                loss=tf.losses.SparseCategoricalCrossentropy(from_logits=True),
                metrics=[tf.metrics.SparseCategoricalAccuracy()])

  return model

model = create_model()

#Save model
model.save('my_model.h5')

#Load model
new_model = tf.keras.models.load_model('my_model.h5')

new_model.summary()