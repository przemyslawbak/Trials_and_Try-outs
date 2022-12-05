import tensorflow as tf
import os
import io

# Download the zip file
path_to_zip = tf.keras.utils.get_file('smsspamcollection.zip', origin='https://archive.ics.uci.edu/ml/machine-learning-databases/00228/smsspamcollection.zip', extract=True)
print('dupa')

# Let's see if we read the data correctly
lines = io.open(path_to_zip.replace("smsspamcollection.zip", "SMSSpamCollection")).read().strip().split('\n')
print(lines[0])