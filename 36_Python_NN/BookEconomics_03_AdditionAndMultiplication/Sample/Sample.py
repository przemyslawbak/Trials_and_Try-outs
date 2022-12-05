import tensorflow as tf

# Define two scalars as constants.
s1 = tf.constant(5, tf.float32)
s2 = tf.constant(15, tf.float32)
# Add and multiply using tf.add() and tf.multiply().
s1s2_sum = tf.add(s1, s2)
s1s2_product = tf.multiply(s1, s2)
# Add and multiply using operator overloading.
s1s2_sum = s1+s2
s1s2_product = s1*s2
# Print sum.
print(s1s2_sum)
tf.Tensor(20.0, shape=(), dtype=float32)
# Print product.
print(s1s2_product)
tf.Tensor(75.0, shape=(), dtype=float32)

# Print the shapes of the two tensors.
print(images.shape)
(32, 64, 64, 3)
print(transform.shape)
(32, 64, 64, 3)
# Convert numpy arrays into tensorflow constants.
images = tf.constant(images, tf.float32)
transform = tf.constant(transform, tf.float32)
# Perform tensor addition with tf.add().
images = tf.add(images, transform)
# Perform tensor addition with operator overloading.
images = images+transform

# Generate 6-tensors from normal distribution draws.
A = tf.random.normal([5, 10, 7, 3, 2, 15])
B = tf.random.normal([5, 10, 7, 3, 2, 15])
# Perform elementwise multiplication.
C = tf.multiply(A, B)
C = A*B

# Set random seed to generate reproducible results.
tf.random.set_seed(1)

# Use normal distribution draws to generate tensors.
A = tf.random.normal([200])
B = tf.random.normal([200])
# Perform dot product.
c = tf.tensordot(A, B, axes = 1)
# Print numpy argument of c.
print(c.numpy()) #-15.284362

# Use normal distribution draws to generate tensors.
A = tf.random.normal([200, 50])
B = tf.random.normal([50, 10])
# Perform matrix multiplication.
C = tf.matmul(A, B)
# Print shape of C.
print(C.shape)
(200, 10)