import tensorflow as tf

#check TF version
print(tf.__version__)

tensor1 = tf.Variable([1,2,3], dtype=tf.int32, \
name='my_tensor', trainable=True)

print(tensor1)
print(tensor1.shape)
print(tf.rank(tensor1))