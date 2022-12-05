import tensorflow as tf

tensor1 = tf.Variable([[1,2,3]])
tensor2 = tf.Variable([[1],[2],[3]])
tensor_mult = tf.matmul(tensor1, tensor2) #same
tensor_mult2 = tensor1 @ tensor2 #same
scalar_mult = 5 * tensor1 #scalar

matrix1 = tf.Variable([[1.0,0.0,3.0,1.0,2.0], \
[0.0,1.0,1.0,1.0,1.0], \
[2.0,1.0,0.0,2.0,0.0]], \
tf.float32)

matrix2 = tf.Variable([[0.49, 103], \
[0.18, 38], \
[0.24, 69], \
[1.02, 75], \
[0.68, 78]])

matmul1 = tf.matmul(matrix1, matrix2)

matrix3 = tf.Variable([[120.0, 100.0, 90.0], \
[30.0, 15.0, 20.0], \
[220.0, 240.0, 185.0], \
[145.0, 160.0, 155.0], \
[330.0, 295.0, 290.0]])

matmul3 = matrix3 @ matmul1