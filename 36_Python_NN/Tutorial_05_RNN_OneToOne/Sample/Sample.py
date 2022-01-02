#https://stackabuse.com/solving-sequence-problems-with-lstm-in-keras/

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
X = [x+1 for x in range(20)]
Y = [y * 15 for y in X]

print(X) #[1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20] 
print(Y) #[15, 30, 45, 60, 75, 90, 105, 120, 135, 150, 165, 180, 195, 210, 225, 240, 255, 270, 285, 300]

X_train_arr, y_train_arr = np.array(X), np.array(Y)

#Reshaping for LSTM 
X_train_arr = X_train_arr.reshape(20, 1, 1) #(samples, time-steps, features)

#Solution via Simple LSTM
model = Sequential()
model.add(LSTM(50, activation='relu', input_shape=(1, 1)))
model.add(Dense(1))
model.compile(optimizer='adam', loss='mse')
print(model.summary())

#Training model
model.fit(X_train_arr, y_train_arr, epochs=1000, batch_size=5)

#Predicting
test_input = np.array([30])
test_input = test_input.reshape((1, 1, 1))
test_output = model.predict(test_input, verbose=0)
print(test_output) #439