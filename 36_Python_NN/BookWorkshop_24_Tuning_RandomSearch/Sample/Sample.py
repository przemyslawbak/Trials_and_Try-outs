import tensorflow as tf
import pandas as pd
from tensorflow.keras.layers import Dense
from sklearn.model_selection import train_test_split
import kerastuner as kt

file_url = 'connect-4.csv'
data = pd.read_csv(file_url)
target = data.pop('class')

#Split the data into training and test sets using train_test_split(), with
#20% of the data for testing and 42 for random_state
X_train, X_test, y_train, y_test = train_test_split\
(data, target, \
test_size=0.2, \
random_state=42)

tf.random.set_seed(8)

#Define a function called model_builder that will create a sequential model
def model_builder(hp):
    model = tf.keras.Sequential()
    p_l2 = hp.Choice('l2', values = [0.1, 0.01, 0.001, 0.0001])
    reg_fc1 = Dense(512, input_shape=(42,), activation='relu', \
    kernel_regularizer=tf.keras.regularizers\
    .l2(l=hp_l2))
    reg_fc2 = Dense(512, activation='relu', \
    kernel_regularizer=tf.keras.regularizers\
    .l2(l=hp_l2))
    reg_fc3 = Dense(128, activation='relu', \
    kernel_regularizer=tf.keras.regularizers\
    .l2(l=hp_l2))
    reg_fc4 = Dense(128, activation='relu', \
    kernel_regularizer=tf.keras.regularizers\
    .l2(l=hp_l2))
    reg_fc5 = Dense(3, activation='softmax')
    model.add(reg_fc1)
    model.add(reg_fc2)
    model.add(reg_fc3)
    model.add(reg_fc4)
    model.add(reg_fc5)
    loss = tf.keras.losses.SparseCategoricalCrossentropy()
    optimizer = tf.keras.optimizers.Adam(0.001)
    model.compile(optimizer = optimizer, loss = loss, \
    metrics = ['accuracy'])
    return model

#Instantiate a RandomSearch tuner and assign val_accuracy to objective
#and 10 to max_trials
#Hyperband is another tuner available in the Keras Tuner package. Like random
#search, it randomly picks candidates from the search space, but more efficiently.
tuner = kt.RandomSearch(model_builder, objective='val_accuracy', \
max_trials=10)

tuner.search(X_train, y_train, validation_data=(X_test, y_test))

best_hps = tuner.get_best_hyperparameters()[0]
best_l2 = best_hps.get('l2') #The best value for the l2 hyperparameter found by random search is 0.0001

model = tuner.hypermodel.build(best_hps)
model.fit(X_train, y_train, epochs=5, \
validation_data=(X_test, y_test))