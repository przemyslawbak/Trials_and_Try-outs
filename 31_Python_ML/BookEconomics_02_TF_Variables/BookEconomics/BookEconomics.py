import tensorflow as tf

# Define the data as constants.
X = tf.constant([[1, 0], [1, 2]], tf.float32)
Y = tf.constant([[2], [4]], tf.float32)
# Initialize beta.
beta = tf.Variable([[0.01],[0.01]], tf.float32)
# Compute the residual.
residuals = Y - tf.matmul(X, beta)

print(X)
print(Y)
print(beta)