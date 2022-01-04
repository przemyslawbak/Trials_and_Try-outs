from keras.preprocessing.text import one_hot
from keras.preprocessing.sequence import pad_sequences
from keras.models import Sequential
from keras.layers.core import Activation, Dropout, Dense
from keras.layers import Flatten, LSTM, Attention
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
import tensorflow as tf
import matplotlib.pyplot as plt
from keras.layers import RepeatVector
from keras.layers import TimeDistributed

#Creating the Dataset
X = list()
Y = list()
Xt = list()
Yt = list()
X = [x for x in range(5, 301, 5)]
Y = [y for y in range(20, 316, 5)]
Xt = [x for x in range(10, 310, 5)]
Yt = [y for y in range(25, 321, 5)]

X_train_arr = np.array(X).reshape(20, 3, 1) #(samples, time-steps, features)
y_train_arr = np.array(Y).reshape(20, 3, 1) #(samples, time-steps, features)
X_test_arr = np.array(Xt).reshape(20, 3, 1) #(samples, time-steps, features)
y_test_arr = np.array(Yt).reshape(20, 3, 1) #(samples, time-steps, features)

base_results = []
update_results = []

for x in range(100):
    print('Trial No: ' + str(x) + ' of 100')
    #model1 - base
    model1 = Sequential()
    model1.add(LSTM(100, activation='relu', input_shape=(3, 1)))
    model1.add(RepeatVector(3))
    model1.add(LSTM(100, activation='relu', return_sequences=True))
    model1.add(TimeDistributed(Dense(1)))
    model1.compile(optimizer='adam', loss='mse')
    model1.fit(X_train_arr, y_train_arr, epochs=100, validation_split=0.2, verbose=0, batch_size=64)

    #model2 - with bidirectional
    model2 = Sequential()
    model2.add(Bidirectional(LSTM(100, activation='relu', input_shape = (time_step, features))))
    model2.add(RepeatVector(features))
    model2.add(Bidirectional(LSTM(100, activation='relu', return_sequences=True)))
    model2.compile(optimizer='adam', loss='mse')
    model2.fit(X_train_arr, y_train_arr, epochs=100, validation_split=0.2, verbose=0, batch_size=64)

    results1 = model1.evaluate(X_test_arr, y_test_arr, batch_size=128)
    results2 = model2.evaluate(X_test_arr, y_test_arr, batch_size=128)
    print("test loss 1", results1)
    print("test loss 2", results2)
    base_results.append(results1)
    update_results.append(results2)

mean_res1 = sum(base_results)/len(base_results)
mean_res2 = sum(update_results)/len(update_results)

print('Mean of results 1: ' + str(mean_res1)) #
print('Mean of results 2: ' + str(mean_res2)) #

#CONCLUSION:
