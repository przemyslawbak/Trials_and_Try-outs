import pandas as pd

# Set number of observations and samples
S = 100
N = 10000

# Set true values of parameters.
alpha = tf.constant([1.], tf.float32)
beta = tf.constant([3.], tf.float32)

# Draw independent variable and error.
X = tf.random.normal([N, S])
epsilon = tf.random.normal([N, S], stddev=0.25)

# Compute dependent variable.
Y = alpha + beta*X + epsilon