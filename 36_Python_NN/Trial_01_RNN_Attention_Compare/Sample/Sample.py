from keras.preprocessing.text import one_hot
from keras.preprocessing.sequence import pad_sequences
from keras.models import Sequential
from keras.layers.core import Activation, Dropout, Dense
from keras.layers import Flatten, LSTM, Attention, MultiHeadAttention, Layer
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
import keras
import matplotlib.pyplot as plt
from keras.layers import RepeatVector
from keras.layers import TimeDistributed
from keras import backend as K
from keras_self_attention import SeqSelfAttention, SeqWeightedAttention

class attention(Layer):
    
    def __init__(self, return_sequences=True):
        self.return_sequences = return_sequences
        super(attention,self).__init__()
        
    def build(self, input_shape):
        
        self.W=self.add_weight(name="att_weight", shape=(input_shape[-1],1),
                               initializer="normal")
        self.b=self.add_weight(name="att_bias", shape=(input_shape[1],1),
                               initializer="zeros")
        
        super(attention,self).build(input_shape)
        
    def call(self, x):
        
        e = K.tanh(K.dot(x,self.W)+self.b)
        a = K.softmax(e, axis=1)
        output = x*a
        
        if self.return_sequences:
            return output
        
        return K.sum(output, axis=1)

#Creating the Dataset
X = list()
Y = list()
Xt = list()
Yt = list()
X = [x for x in range(5, 901, 5)]
Y = [y for y in range(20, 916, 5)]
Xt = [x for x in range(10, 910, 5)]
Yt = [y for y in range(25, 921, 5)]

X_train_arr = np.array(X).reshape(60, 3, 1) #(samples, time-steps, features)
y_train_arr = np.array(Y).reshape(60, 3, 1) #(samples, time-steps, features)
X_test_arr = np.array(Xt).reshape(60, 3, 1) #(samples, time-steps, features)
y_test_arr = np.array(Yt).reshape(60, 3, 1) #(samples, time-steps, features)

base_results = []
update_results = []

for x in range(100):
    print('Trial No: ' + str(x) + ' of 100')
    #model1 - base
    model1 = Sequential()
    model1.add(LSTM(60, activation='relu', input_shape=(3, 1)))
    model1.add(RepeatVector(3))
    model1.add(LSTM(60, activation='relu', return_sequences=True))
    model1.add(TimeDistributed(Dense(1)))
    model1.compile(optimizer='adam', loss='mse')
    model1.fit(X_train_arr, y_train_arr, epochs=100, validation_data=(X_test_arr, y_test_arr), verbose=0, batch_size=64)

    #model2 - with Attention()
    model2 = Sequential()
    model2.add(LSTM(60, activation='relu', input_shape=(3, 1)))
    model2.add(attention(return_sequences=True)) # receive 3D and output 3D
    model2.add(RepeatVector(3))
    model2.add(LSTM(60, activation='relu', return_sequences=True))
    model2.add(TimeDistributed(Dense(1)))
    model2.compile(optimizer='adam', loss='mse')
    model2.fit(X_train_arr, y_train_arr, epochs=100, validation_data=(X_test_arr, y_test_arr), verbose=0, batch_size=64)

    results1 = model1.evaluate(X_test_arr, y_test_arr, batch_size=128)
    results2 = model2.evaluate(X_test_arr, y_test_arr, batch_size=128)
    print("test loss 1", results1)
    print("test loss 2", results2)
    base_results.append(results1)
    update_results.append(results2)

mean_res1 = sum(base_results)/len(base_results)
mean_res2 = sum(update_results)/len(update_results)

print('Mean of results 1: ' + str(mean_res1)) #3.05502985984087
print('Mean of results 2: ' + str(mean_res2)) #4.541839057207108

#CONCLUSION: Attention do not improve performance
