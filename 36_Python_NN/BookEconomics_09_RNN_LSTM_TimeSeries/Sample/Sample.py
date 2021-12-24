import numpy as np
import tensorflow as tf
from tensorflow.keras.preprocessing.sequence import TimeseriesGenerator

#The first problem with RNNs is that they suffer from the vanishing gradient
#problem when long sequences of data are used as inputs. The most
#effective solution to this problem is to make use of a gated RNN cell. There
#are two such cells that are commonly used: (1) long short-term memory
#(LSTM) and (2) gated recurrent units (GRUs). We will concentrate on the
#former in this subsection.

# Set data path.
data_path = '../data/chapter7/' #f*ck knows where to find it

# Load data.
inflation = pd.read_csv(data_path+'inflation.csv')

# Convert to numpy array.
inflation = np.array(inflation['Inflation'])

# Add dimension.
inflation = np.expand_dims(inflation, 1)

# Instantiate time series generator.
# oher TimeseriesGenerator example: https://stackoverflow.com/a/61642690
generator = TimeseriesGenerator(inflation[:211], inflation[:211],
length = 4, batch_size = 12)

# Define sequential model.
model = tf.keras.models.Sequential()

# Define recurrent layer to return hidden states.
model.add(tf.keras.layers.LSTM(3, return_sequences=True, input_shape=(4, 1)))

# Define second recurrent layer.
model.add(tf.keras.layers.LSTM(2))

# Define output layer.
model.add(tf.keras.layers.Dense(1, activation="linear"))