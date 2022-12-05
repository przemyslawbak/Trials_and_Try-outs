#https://stackabuse.com/solving-sequence-problems-with-lstm-in-keras-part-2/

from keras.preprocessing.text import one_hot
from keras.preprocessing.sequence import pad_sequences
from keras.models import Sequential
from keras.layers.core import Activation, Dropout, Dense
from keras.layers import Flatten, LSTM
from keras.layers import GlobalMaxPooling1D
from keras.models import Model
from keras.layers.embeddings import Embedding
from sklearn.model_selection import train_test_split
from keras.preprocessing.text import Tokenizer
from keras.layers import Input
from keras.layers.merge import Concatenate
from keras.layers import Bidirectional
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt

#Creating the Dataset
X = list()
Y = list()
X = [x+3 for x in range(-2, 43, 3)]

X = np.array(X).reshape(15, 1, 1) #(samples, time-steps, features)

for i in X:
    output_vector = list()
    output_vector.append(i+1)
    output_vector.append(i+2)
    Y.append(output_vector)

print(X) #[1, 4, 7, 10, 13, 16, 19, 22, 25, 28, 31, 34, 37, 40, 43]
print(Y) #[[2, 3], [5, 6], [8, 9], [11, 12], [14, 15], [17, 18], [20, 21], [23, 24], [26, 27], [29, 30], [32, 33], [35, 36], [38, 39], [41, 42], [44, 45]]

X_train_arr, y_train_arr = np.array(X), np.array(Y)

#Creating and compiling model
model = Sequential()
model.add(LSTM(50, activation='relu', input_shape=(1, 1)))
model.add(Dense(2))
model.compile(optimizer='adam', loss='mse')

#Training model
model.fit(X_train_arr, y_train_arr, epochs=1000, validation_split=0.2, batch_size=3)

#Prediction
test_input = np.array([50])
test_input = test_input.reshape((1, 1, 1))
test_output = model.predict(test_input, verbose=0)
print(test_output) #[[32.37116  32.807873]]