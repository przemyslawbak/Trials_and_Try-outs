import numpy as np
import tensorflow as tf
from tensorflow.keras.preprocessing.sequence import TimeseriesGenerator

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

# Define recurrent layer.
model.add(tf.keras.layers.SimpleRNN(2, input_shape=(4, 1)))

# Define output layer.
model.add(tf.keras.layers.Dense(1, activation="linear"))

# Compile the model.
model.compile(loss="mse", optimizer="adam")

# Fit model to data using generator.
model.fit_generator(train_generator, epochs=100)