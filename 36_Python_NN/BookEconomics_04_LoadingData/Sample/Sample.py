import numpy as np
import pandas as pd

# Import image data using numpy.
images = np.load('images.npy')
# Normalize pixel values to [0,1] interval.
images = images / 255.0
# Print the tensor shape.
print(images.shape)
(32, 64, 64, 3)

# Import data using pandas.
data = np.load('data.csv')
# Convert data to a TensorFlow constant.
data_tensorflow = tf.constant(data)
# Convert data to a numpy array.
data_numpy = np.array(data)