import tensorflow as tf
import pandas as pd
from sklearn.preprocessing import StandardScaler
from tensorflow.keras.layers import Dense

train_url = 'dota2Train.csv'
test_url = 'dota2Test.csv'
X_train = pd.read_csv(train_url, header=None)

#Extract the target variable (column 0) using the pop() method and save it in a
#variable called y_train
y_train = X_train.pop(0)
y_train = y_train.replace(-1,0)
y_train.head()

X_test = pd.read_csv(test_url, header=None)
y_test = X_test.pop(0)
y_test = y_test.replace(-1,0)
y_test.head()

tf.random.set_seed(8)

model = tf.keras.Sequential()

#Create a fully connected layer of 512 units with Dense() and specify ReLu as
#the activation function and the input shape as (116,), which corresponds to
#the number of features from the dataset. Save it in a variable called fc1
fc1 = Dense(512, input_shape=(116,), activation='relu')
#Create a fully connected layer of 512 units with Dense() and specify ReLu as
#the activation function. Save it in a variable called fc2
fc2 = Dense(512, activation='relu')
#Create a fully connected layer of 128 units with Dense() and specify ReLu as
#the activation function. Save it in a variable called fc3
fc3 = Dense(128, activation='relu')
#Create a fully connected layer of 128 units with Dense() and specify ReLu as
#the activation function. Save it in a variable called fc4
fc4 = Dense(128, activation='relu')
#Create a fully connected layer of 128 units with Dense() and specify sigmoid as
#the activation function. Save it in a variable called fc5
fc5 = Dense(1, activation='sigmoid')
#Sequentially add all five fully connected layers to the model using
#add() method
model.add(fc1)
model.add(fc2)
model.add(fc3)
model.add(fc4)
model.add(fc5)
print(model.summary())

loss = tf.keras.losses.BinaryCrossentropy()

optimizer = tf.keras.optimizers.Adam(0.001)

model.compile(optimizer=optimizer, loss=loss)

model.fit(X_train, y_train, epochs=5)

preds = model.predict(X_test)
print(preds[:5])