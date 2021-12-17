import tensorflow as tf

#4 different TF variables: scalar, vector, matrix, tensor

int_variable = tf.Variable(4113, tf.int16)

vector_variable = tf.Variable([0.23, 0.42, 0.35], \
tf.float32)

matrix_variable = tf.Variable([[4113, 7511, 6259], \
[3870, 6725, 6962]], \
tf.int32)

tensor_variable = tf.Variable([[[4113, 7511, 6259], \
[3870, 6725, 6962]], \
[[5102, 7038, 6591], \
[3661, 5901, 6235]], \
[[951, 1208, 1098], \
[870, 645, 948]]])