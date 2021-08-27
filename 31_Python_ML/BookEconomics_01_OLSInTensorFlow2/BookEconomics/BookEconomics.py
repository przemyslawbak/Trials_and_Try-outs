
import tensorflow as tf
print(tf.__version__)
# Define the data as constants.
X = tf.constant([[1, 0], [1, 2]], tf.float32)
Y = tf.constant([[2], [4]], tf.float32)
# Matrix multiply X by X's transpose and invert.
beta_0 = tf.linalg.inv(tf.matmul(tf.transpose(X), X))
# Matrix multiply beta_0 by X's transpose.
beta_1 = tf.matmul(beta_0, tf.transpose(X))
# Matrix multiply beta_1 by Y.
beta = tf.matmul(beta_1, Y)
# Print coefficient vector.
print(beta.numpy())