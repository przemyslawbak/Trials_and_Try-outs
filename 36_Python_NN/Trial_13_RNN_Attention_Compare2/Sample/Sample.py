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

# Add attention layer to the deep learning network
class attention(Layer):
    def __init__(self,**kwargs):
        super(attention,self).__init__(**kwargs)

    def build(self,input_shape):
        self.W=self.add_weight(name='attention_weight', shape=(input_shape[-1],1), 
                               initializer='random_normal', trainable=True)
        self.b=self.add_weight(name='attention_bias', shape=(input_shape[1],1), 
                               initializer='zeros', trainable=True)        
        super(attention, self).build(input_shape)

    def call(self,x):
        # Alignment scores. Pass them through tanh function
        e = K.tanh(K.dot(x,self.W)+self.b)
        # Remove dimension of size 1
        e = K.squeeze(e, axis=-1)   
        # Compute the weights
        alpha = K.softmax(e)
        # Reshape to tensorFlow format
        alpha = K.expand_dims(alpha, axis=-1)
        # Compute the context vector
        context = x * alpha
        context = K.sum(context, axis=1)
        return context

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
    model1.fit(X_train_arr, y_train_arr, epochs=100, validation_data=(X_test_arr, y_test_arr), verbose=0, batch_size=64)

    #model2 - with Attention()
    #RNN Network With Attention Layer
    def create_model_with_attention(hidden_units, dense_units, input_shape, activation):
        x=Input(shape=input_shape)
        LSTM_layer1 = LSTM(hidden_units, return_sequences=True, activation=activation)(x)
        attention_layer = attention()(LSTM_layer1)
        repeat=RepeatVector(3)(attention_layer)
        LSTM_layer2 = LSTM(hidden_units, return_sequences=True, activation=activation)(repeat)
        outputs=TimeDistributed(Dense(dense_units, trainable=True, activation=activation))(LSTM_layer2)
        model=Model(x,outputs)
        model.compile(loss='mse', optimizer='adam')    
        return model  

    model2 = create_model_with_attention(hidden_units=100, dense_units=1, input_shape=(3,1), activation='relu')
    model2.fit(X_train_arr, y_train_arr, epochs=100, validation_data=(X_test_arr, y_test_arr), verbose=0, batch_size=64)

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
