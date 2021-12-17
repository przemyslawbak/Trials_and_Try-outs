import tensorflow as tf

tensor1 = tf.Variable([1,2,3])
tensor2 = tf.Variable([4,5,6])

tensor_add1 = tf.add(tensor1, tensor2) #same
tensor_add2 = tensor1 + tensor2 #same

int1 = tf.Variable(4113, tf.int32)
int2 = tf.Variable(7511, tf.int32)
int3 = tf.Variable(6529, tf.int32)

int_sum = int1+int2+int3

print(int(int_sum))

vec1 = tf.Variable([4113, 3870, 5102], tf.int32)
vec2 = tf.Variable([7511, 6725, 7038], tf.int32)
vec3 = tf.Variable([6529, 6962, 6591], tf.int32)

vec_sum = vec1 + vec2 + vec3

print(vec_sum.numpy())

matrix1 = tf.Variable([[4113, 3870, 5102], \
[3611, 951, 870]], tf.int32)
matrix2 = tf.Variable([[7511, 6725, 7038], \
[5901, 1208, 645]], tf.int32)
matrix3 = tf.Variable([[6529, 6962, 6591], \
[6235, 1098, 948]], tf.int32)

matrix1.shape == matrix2.shape == matrix3.shape

inta = tf.Variable(2706, tf.int32)
intb = tf.Variable(2386, tf.int32)

int_x = inta+intb