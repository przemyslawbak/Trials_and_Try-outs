import tensorflow as tf

#writing logs for TensorBoard
logdir = 'logs/'
writer = tf.summary.create_file_writer(logdir)
tf.summary.trace_on(graph=True, profiler=True)
with writer.as_default():
    tf.summary.trace_export(name="my_func_trace",\
    step=0, profiler_outdir=logdir)

#exercise

#generating random values
tf.random.set_seed(42)

#Create a file_writer object
logdir = 'logs/'
writer = tf.summary.create_file_writer(logdir)

#Create a TensorFlow function to multiply two matrices together
@tf.function
def my_matmult_func(x, y):
    result = tf.matmul(x, y)
    return result

#Create sample data in the form of two tensors with the shape 7x7
x = tf.random.uniform((7, 7))
y = tf.random.uniform((7, 7))

#Turn on graph tracing using TensorFlow's summary class
tf.summary.trace_on(graph=True, profiler=True)

z = my_matmult_func(x, y)
with writer.as_default():
    tf.summary.trace_export(name="my_func_trace",\
    step=0,\
    profiler_outdir=logdir)

#Finally, launch TensorBoard