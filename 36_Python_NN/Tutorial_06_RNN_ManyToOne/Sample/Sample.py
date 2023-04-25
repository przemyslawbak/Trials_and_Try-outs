#https://stackabuse.com/solving-sequence-problems-with-lstm-in-keras/

from keras.models import Sequential
from keras.layers.core import Dense
from keras.layers import LSTM
import numpy as np

#Creating the Dataset
X = np.array([x+1 for x in range(45)]) #input
print(X)

#Reshaping: Since we want 15 samples in our dataset, we will reshape the list of integers containing the first 45 integers
X = X.reshape(15,3,1) #(samples, time-steps, features)
print(X)

Y = list() #output
for x in X:
    Y.append(x.sum())

Y = np.array(Y)
print(Y)

X_train_arr, y_train_arr = np.array(X), np.array(Y)

#Creating and compiling model
model = Sequential()
model.add(LSTM(50, activation='relu', input_shape=(3, 1)))
model.add(Dense(1))
model.compile(optimizer='adam', loss='mse')

#Training model
history = model.fit(X_train_arr, y_train_arr, epochs=200, validation_split=0.2, verbose=1)

#Prediction
test_input = np.array([50,51,52])
test_input = test_input.reshape((1, 3, 1))
test_output = model.predict(test_input, verbose=0)
print(test_output) #153