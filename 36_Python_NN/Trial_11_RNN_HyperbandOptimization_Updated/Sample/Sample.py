import tensorflow as tf
from tensorflow import keras
import keras_tuner as kt
import numpy as np
import pandas as pd
import os
from sklearn.preprocessing import MinMaxScaler
from tensorflow.keras.layers import Dense, LSTM, Dropout, TimeDistributed, Activation, RepeatVector, Bidirectional
#Import the training dataset
filename = "GPW_DLY WIG20, 15_OLD.csv"
dataset_train = pd.read_csv(filename)
training_set = dataset_train[['close', 'high', 'low', 'open']]

#Perform feature scaling to transform the data
scaler = MinMaxScaler(feature_range = (0, 1))
training_set_scaled = scaler.fit_transform(training_set)

#Model values
time_steps = 100 #learning step
num_epochs = 50
num_verbose=2
es_patinence = 5

#Variables
features = len(training_set.columns)
split_percent = 0.80 #train/test daa split percent (80%)
split = int(split_percent*len(training_set_scaled)) #split percent multiplying by data rows

#Create a data structure with n-time steps
X = []
y = []
for i in range(time_steps + 1, len(training_set_scaled)):
    X.append(training_set_scaled[i-time_steps-1:i-1, 0:features]) #take all columns into the set, including time_steps legth
    y.append(training_set_scaled[i-time_steps:i, 0:features]) #take all columns into the set, including time_steps legth

X_train_arr, y_train_arr = np.array(X), np.array(y)

print(X_train_arr.shape) #(2494, 100, 4) <-- train data, having now 2494 rows, with 100 time steps, each row has 4 features (MANY)
print(y_train_arr.shape) #(2494, 100, 4) <-- target data, having now 2494 rows, with 100 time step, but 4 features (TO MANY)

#Split data
X_train_splitted = X_train_arr[:split] #(80%) model train input data
y_train_splitted = y_train_arr[:split] #80%) model train target data
X_test_splitted = X_train_arr[split:] #(20%) test prediction input data
y_test_splitted = y_train_arr[split:] #(20%) test prediction compare data

#Reshaping to rows/time_steps/columns
X_train_splitted = np.reshape(X_train_splitted, (X_train_splitted.shape[0], time_steps, features))
y_train_splitted = np.reshape(y_train_splitted, (y_train_splitted.shape[0], time_steps, features))
X_test_splitted = np.reshape(X_test_splitted, (X_test_splitted.shape[0], time_steps, features))
y_test_splitted = np.reshape(y_test_splitted, (y_test_splitted.shape[0], time_steps, features))

def model_builder(hp):
    hp_layers = hp.Int(
        'num_layers',
        min_value=1,
        max_value=5,
        step=1,
        default=1
    )
    hp_bi_units1 = hp.Int(
        'units',
        min_value=32,
        max_value=512,
        step=32,
        default=128
    )
    hp_bi_units2 = hp.Int(
        'units',
        min_value=32,
        max_value=512,
        step=32,
        default=128
    )
    hp_activation_bilstm = hp.Choice(
        'activation',
        values=['relu', 'tanh', 'sigmoid', 'elu'],
        default='relu'
    )
    hp_activation_dense = hp.Choice(
        'activation',
        values=['relu', 'tanh', 'sigmoid', 'elu'],
        default='relu'
    )
    hp_dropout = hp.Float(
                'dropout_rate',
                min_value=0.0,
                max_value=0.5,
                default=0.1,
                step=0.1,
            )
    hp_adam = hp.Float(
                    'adam_learning_rate',
                    min_value=1e-4,
                    max_value=1e-2,
                    sampling='LOG',
                    default=1e-3
                )
    model = tf.keras.Sequential()
    for i in range(hp_layers):
        model.add(Bidirectional(LSTM(units=hp_bi_units1, activation=hp_activation_bilstm, return_sequences=True)))
        model.add(Dropout(rate=hp_dropout))
    model.add(Dense(4, activation=hp_activation_dense))
    model.compile(loss='mae', metrics=['mae', 'acc', 'mse'], optimizer=keras.optimizers.Adam(learning_rate=hp_adam))
    return model

#TUNE - Hyperband
tuner = kt.Hyperband(model_builder,
                     objective='acc',
                     max_epochs=10,
                     factor=3,
                     directory=os.path.normpath('C:/keras_tuning'),
                     project_name='intro_to_kt',
                     overwrite=True)
print(tuner.search_space_summary()
)
stop_early = tf.keras.callbacks.EarlyStopping(monitor='val_acc', patience=es_patinence)

tuner.search(X_train_splitted, y_train_splitted, epochs=num_epochs, validation_data=(X_test_splitted, y_test_splitted), callbacks=[stop_early], verbose=num_verbose)

# Get the optimal hyperparameters
best_hps=tuner.get_best_hyperparameters(num_trials=1)[0]

print(f"""
The hyperparameter search is complete. The optimal number of units in the first densely-connected
layer is {best_hps.get('units')} and the optimal learning rate for the optimizer
is {best_hps.get('learning_rate')}.
""")
print(best_hps)

# Build the model with the optimal hyperparameters and train it on the data for 50 epochs
model = tuner.hypermodel.build(best_hps)
history = model.fit(X_train_splitted, y_train_splitted, epochs=50, validation_data=(X_test_splitted, y_test_splitted), verbose=num_verbose)

val_acc_per_epoch = history.history['val_accuracy']
best_epoch = val_acc_per_epoch.index(max(val_acc_per_epoch)) + 1
print('Best epoch: %d' % (best_epoch,))

hypermodel = tuner.hypermodel.build(best_hps)

# Retrain the model
hypermodel.fit(X_train_splitted, y_train_splitted, epochs=best_epoch, validation_data=(X_test_splitted, y_test_splitted), verbose=num_verbose, callbacks=[stop_early])

eval_result = hypermodel.evaluate(X_test_splitted, y_test_splitted)
print("[evaluation]: ", eval_result)

#CONCLUSION: