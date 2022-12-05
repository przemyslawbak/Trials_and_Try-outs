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
from keras.layers import RepeatVector
from keras.layers import TimeDistributed

#Creating the Dataset
X = list()
Y = list()
X = [x for x in range(5, 301, 5)]
Y = [y for y in range(20, 316, 5)]

X_train_arr = np.array(X).reshape(20, 3, 1) #(samples, time-steps, features)
y_train_arr = np.array(Y).reshape(20, 3, 1) #(samples, time-steps, features)

#Creating model
model = Sequential()

# encoder layer
model.add(LSTM(100, activation='relu', input_shape=(3, 1)))

# repeat vector
model.add(RepeatVector(3))

# decoder layer
model.add(LSTM(100, activation='relu', return_sequences=True))

model.add(TimeDistributed(Dense(1)))
model.compile(optimizer='adam', loss='mse')

print(model.summary())

#Training model
history = model.fit(X_train_arr, y_train_arr, epochs=1000, validation_split=0.2, verbose=1, batch_size=3)

test_input = np.array([300, 305, 310])
test_input = test_input.reshape((1, 3, 1))
test_output = model.predict(test_input, verbose=0)
print(test_output)
#[[[308.9502 ]
  #[321.2577 ]
  #[336.23688]]]