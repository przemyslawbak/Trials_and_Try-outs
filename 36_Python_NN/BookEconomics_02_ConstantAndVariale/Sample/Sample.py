import tensorflow as tf

#Define the data as constants.
#That is, a constant is fixed, whereas a variable may
#change over time.
X = tf.constant([[1, 0], [1, 2]], tf.float32)
Y = tf.constant([[2], [4]], tf.float32)

# Initialize beta. The parameter vector,
#beta, is defined as a variable using tf.Variable(), since it will be varied
#by the optimization algorithm to try to minimize a transformation of the
#residuals
beta = tf.Variable([[0.01],[0.01]], tf.float32)

# Compute the residual.
residuals = Y - tf.matmul(X, beta)