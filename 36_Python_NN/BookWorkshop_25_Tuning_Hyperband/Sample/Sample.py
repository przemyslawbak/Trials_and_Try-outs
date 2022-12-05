import tensorflow as tf
import pandas as pd
from tensorflow.keras.layers import Dense
from sklearn.model_selection import train_test_split
import kerastuner as kt

file_url = 'connect-4.csv'
data = pd.read_csv(file_url)
target = data.pop('class')
X_train, X_test, y_train, y_test = train_test_split\
(data, target, \
test_size=0.2, \
random_state=42)

tf.random.set_seed(8)

def model_builder(hp):
    model = tf.keras.Sequential()
    hp_units = hp.Int('units', min_value=128, max_value=512, \
    step=64)
    reg_fc1 = Dense(hp_units, input_shape=(42,), \
    activation='relu', \
    kernel_regularizer=tf.keras.regularizers\
    .l2(l=0.0001))
    reg_fc2 = Dense(512, activation='relu', \
    kernel_regularizer=tf.keras.regularizers\
    .l2(l=0.0001))
    reg_fc3 = Dense(128, activation='relu', \
    kernel_regularizer=tf.keras.regularizers\
    .l2(l=0.0001))
    reg_fc4 = Dense(128, activation='relu', \
    kernel_regularizer=tf.keras.regularizers\
    .l2(l=0.0001))
    reg_fc5 = Dense(3, activation='softmax')
    model.add(reg_fc1)
    model.add(reg_fc2)
    model.add(reg_fc3)
    model.add(reg_fc4)
    model.add(reg_fc5)
    loss = tf.keras.losses.SparseCategoricalCrossentropy()
    hp_learning_rate = hp.Choice('learning_rate', \
    values = [0.01, 0.001, 0.0001])
    optimizer = tf.keras.optimizers.Adam(hp_learning_rate)
    model.compile(optimizer = optimizer, loss = loss, \
    metrics = ['accuracy'])
    return model

#Bayesian optimization is another very popular algorithm used for automatic hyperparameter tuning
tuner = kt.Hyperband(model_builder, objective='val_accuracy', \
max_epochs=5)
tuner.search(X_train, y_train, validation_data=(X_test, y_test))
best_hps = tuner.get_best_hyperparameters()[0]
best_units = best_hps.get('units')
best_lr = best_hps.get('learning_rate')
model.fit(X_train, y_train, epochs=5, \
validation_data=(X_test, y_test))
s