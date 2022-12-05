import tensorflow as tf
import tensorflow_hub as hub
from tensorflow.python.client import session
from tensorflow.python.summary import summary
from tensorflow.python.framework import ops

logdir = 'logs/'

module = hub.load('https://tfhub.dev/google/imagenet/inception_v3/classification/5')

model = module.signatures['default']

with session.Session(graph=ops.Graph()) as sess:
    file_writer = summary.FileWriter(logdir)
    file_writer.add_graph(model.graph)

