import tensorflow as tf
import pandas as pd
from tensorflow.keras.layers import Dense
from tensorflow.keras.callbacks import EarlyStopping

usecols = ['AAGE','ADTIND','ADTOCC','SEOTR','WKSWORK', 'PTOTVAL']
train_url = 'census-income-train.csv'
train_data = pd.read_csv(train_url, usecols=usecols)
train_target = train_data.pop('PTOTVAL')
test_url = 'census-income-test.csv?raw=true'
test_data = pd.read_csv(test_url, usecols=usecols)
test_target = test_data.pop('PTOTVAL')
tf.random.set_seed(8)
model = tf.keras.Sequential()
fc1 = Dense(1048, input_shape=(5,), activation='relu')
fc2 = Dense(512, activation='relu')
fc3 = Dense(128, activation='relu')
fc4 = Dense(64, activation='relu')
fc5 = Dense(3, activation='softmax')
fc5 = Dense(1)

model.add(fc1)
model.add(fc2)
model.add(fc3)
model.add(fc4)
model.add(fc5)

optimizer = tf.keras.optimizers.Adam(0.05)
model.compile(optimizer=optimizer, loss='mse', metrics=['mse'])
model.fit(train_data, train_target, epochs=5, \
validation_split=0.2)

reg_fc1 = Dense(1048, input_shape=(5,), activation='relu', \
kernel_regularizer=tf.keras.regularizers\
.l1_l2(l1=0.001, l2=0.001))
reg_fc2 = Dense(512, activation='relu', \
kernel_regularizer=tf.keras.regularizers\
.l1_l2(l1=0.001, l2=0.001))
reg_fc3 = Dense(128, activation='relu', \
kernel_regularizer=tf.keras.regularizers\
.l1_l2(l1=0.001, l2=0.001))
reg_fc4 = Dense(64, activation='relu', \
kernel_regularizer=tf.keras.regularizers\
.l1_l2(l1=0.001, l2=0.001))
reg_fc5 = Dense(1, activation='relu')

model2 = tf.keras.Sequential()
model2.add(reg_fc1)
model2.add(reg_fc2)
model2.add(reg_fc3)
model2.add(reg_fc4)
model2.add(reg_fc5)

optimizer = tf.keras.optimizers.Adam(0.1)
model2.compile(optimizer=optimizer, loss='mse', metrics=['mse'])
model2.fit(train_data, train_target, epochs=5, \
validation_split=0.2)

