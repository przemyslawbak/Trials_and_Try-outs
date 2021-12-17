import tensorflow as tf

#Adam optimizer with a learning rate equal to 0.001 can be initialized as follows
optimizer = tf.optimizer.adam(learning_rate=0.001)

#Sigmoid activation function can be applied to a tensor as follows
y=tf.keras.activations.sigmoid(x)