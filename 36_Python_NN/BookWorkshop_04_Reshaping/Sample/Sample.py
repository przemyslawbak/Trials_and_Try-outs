import tensorflow as tf

"""
For example, a (4x3) matrix can be reshaped into a (6x2) matrix since they both have a total of 12 elements. The rank, or number, of dimensions, can also be changed
in the reshaping process. For instance, a (4x3) matrix that has a rank equal to 2 can be reshaped into a (3x2x2) tensor that has a rank equal to 3. The (4x3) matrix
can also be reshaped into a (12x1) vector in which the rank has changed from 2 to 1.
"""

tensor1 = tf.Variable([1,2,3,4,5,6])
tensor_reshape = tf.reshape(tensor1, shape=[3,2])
print(tensor_reshape)

tensor2 = tf.Variable([1,2,3,4,5,6])
print(tensor2)
tensor_transpose = tf.transpose(tensor2)
print(tensor_transpose)

matrix1 = tf.Variable([[1,2,3,4], [5,6,7,8]])
print(matrix1.shape) #2,4
reshape1 = tf.reshape(matrix1, shape=[4, 2])
reshape4 = tf.reshape(matrix1, shape=[2, 2, 2])

transpose1 = tf.transpose(matrix1)
print(transpose1.shape) #4,2

#activity
varx = tf.Variable([1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24])

re1 = tf.reshape(varx, shape=[12,2])
print(re1)

re2 =  tf.reshape(varx, shape=[3,4,2])
print(tf.rank(re2))