import numpy as np
import tensorflow as tf
from tensorflow.keras.preprocessing.sequence import TimeseriesGenerator

# Set data path.
data_path = '../data/chapter7/' #f*ck knows where to find it

# Load data.
inflation = pd.read_csv(data_path+'inflation.csv')

# Convert to numpy array.
inflation = np.array(inflation['Inflation'])

# Instantiate time series generator.
# oher TimeseriesGenerator example: https://stackoverflow.com/a/61642690
generator = TimeseriesGenerator(inflation, inflation,
length = 4, batch_size = 12)

#We now have a generator object that we can use to create batches of
#data. A Keras model can use the generator, rather than data, as an input.

# Define sequential model.
model = tf.keras.models.Sequential()

# Add input layer.
model.add(tf.keras.Input(shape=(4,)))

# Define dense layer.
model.add(tf.keras.layers.Dense(2, activation="relu"))

# Define output layer.
model.add(tf.keras.layers.Dense(1, activation="linear"))

# Compile the model.
model.compile(loss="mse", optimizer="adam")

# Train the model.
model.fit_generator(generator, epochs=100)

