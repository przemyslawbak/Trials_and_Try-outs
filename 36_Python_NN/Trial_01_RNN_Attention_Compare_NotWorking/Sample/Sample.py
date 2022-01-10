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

#Import the training dataset
filename = "GPW_DLY WIG20, 15.csv"
dataset_train = pd.read_csv(filename)
training_set = dataset_train[['close', 'high', 'low', 'open']]
time_step = 60

from sklearn.preprocessing import MinMaxScaler
#Perform feature scaling to transform the data
scaler = MinMaxScaler(feature_range = (0, 1))
training_set_scaled = scaler.fit_transform(training_set)

#Variables
features = len(training_set.columns)
future_steps = 33
time_step = 60 #learning step
split_percent = 0.80 #train/test daa split percent (80%)
split = int(split_percent*len(training_set_scaled)) #split percent multiplying by data rows

#Create a data structure with n-time steps
X = []
y = []
for i in range(time_step + 1, len(training_set_scaled)):
    X.append(training_set_scaled[i-time_step-1:i-1, 0:features]) #take all columns into the set, including time_step legth
    y.append(training_set_scaled[i, 0:features]) #take all columns into the set

X_train_arr, y_train_arr = np.array(X), np.array(y)

#Split data
X_train_splitted = X_train_arr[:split] #(80%) model train input data
y_train_splitted = y_train_arr[:split] #(80%) model train target data
X_test_splitted = X_train_arr[split:] #(20%) test prediction input data
y_test_splitted = y_train_arr[split:] #(20%) test prediction compare data

#Reshaping to rows/time_step/columns
X_train_splitted = np.reshape(X_train_splitted, (X_train_splitted.shape[0], time_step, features)) #(samples, time-steps, features), by default should be already
y_train_splitted = np.reshape(y_train_splitted, (y_train_splitted.shape[0], 1, features))  #(samples, time-steps, features)
X_test_splitted = np.reshape(X_test_splitted, (X_test_splitted.shape[0], time_step, features)) #(samples, time-steps, features), by default should be already
y_test_splitted = np.reshape(y_test_splitted, (y_test_splitted.shape[0], 1, features))  #(samples, time-steps, features)

base_results = []
update_results = []

for x in range(100):
    print('Trial No: ' + str(x) + ' of 100')
    #model1 - base
    model1 = Sequential()
    model1.add(LSTM(64, activation='relu', input_shape=(60, 4)))
    model1.add(RepeatVector(4))
    model1.add(LSTM(64, activation='relu', return_sequences=True))
    model1.add(TimeDistributed(Dense(1)))
    model1.compile(optimizer='adam', loss='mse')
    model1.fit(X_train_splitted, y_train_splitted, epochs=2, validation_split=0.2, verbose=1, batch_size=64)

    #model2 - with Attention()
    model2 = Sequential()
    model2.add(LSTM(64, activation='relu', input_shape=(60, 4)))
    model2.add(attention(return_sequences=True)) # receive 3D and output 3D
    model2.add(RepeatVector(4))
    model2.add(LSTM(64, activation='relu', return_sequences=True))
    model2.add(TimeDistributed(Dense(1)))
    model2.compile(optimizer='adam', loss='mse')
    model2.fit(X_train_splitted, y_train_splitted, epochs=2, validation_split=0.2, verbose=1, batch_size=64) #ERROR

    results1 = model1.evaluate(X_test_splitted, y_test_splitted, batch_size=128)
    results2 = model2.evaluate(X_test_splitted, y_test_splitted, batch_size=128)
    print("test loss 1", results1)
    print("test loss 2", results2)
    base_results.append(results1)
    update_results.append(results2)

mean_res1 = sum(base_results)/len(base_results)
mean_res2 = sum(update_results)/len(update_results)

print('Mean of results 1: ' + str(mean_res1)) #???
print('Mean of results 2: ' + str(mean_res2)) #???

#CONCLUSION: ERROR with new approach
