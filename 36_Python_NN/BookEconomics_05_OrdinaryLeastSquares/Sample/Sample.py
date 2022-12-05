import pandas as pd

#Implementation of OLS in TensorFlow 2
# Define the data as constants.
X = tf.constant([[1, 0], [1, 2]], tf.float32)
Y = tf.constant([[2], [4]], tf.float32)

# Compute vector of parameters.
XT = tf.transpose(X)
XTX = tf.matmul(XT,X)
beta = tf.matmul(tf.matmul(tf.linalg.inv(XTX),XT),Y)