import tensorflow as tf

model = tf.keras.Sequential()

#Add an input layer to the model using the model's add method, and add the input_shape argument with size (8,) to represent input data with eight features
model.add(tf.keras.layers.InputLayer(input_shape=(8,), name='Input_layer'))

#Add two layers of the Dense class to the model. The first will represent your hidden layer with four units and a ReLU activation function, and the second will
#represent your output layer with one unit
model.add(tf.keras.layers.Dense(4, activation='relu', \
name='First_hidden_layer'))
model.add(tf.keras.layers.Dense(1, name='Output_layer'))

#View the weights by calling the variables attribute of the model
model.variables

#Create a tensor of size 32x8, which represents a tensor with 32 records and
#8 features

data = tf.random.normal((32,8))
prediction = model.predict(data)

print(prediction)