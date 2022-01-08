import tensorflow as tf
from tensorflow import keras
import keras_tuner as kt
import numpy as np
from tensorflow.keras.layers import Dense, LSTM, Dropout, TimeDistributed, Activation, RepeatVector, Bidirectional
import os


#Creating the Dataset
X = list()
Y = list()
Xt = list()
Yt = list()
X = np.arange(0.005, 0.301, 0.005)
Y = np.arange(0.020, 0.316, 0.005)
Xt = np.arange(0.010, 0.310, 0.005)
Yt = np.arange(0.025, 0.321, 0.005)

X_train_arr = np.array(X).reshape(20, 3, 1) #(samples, time-steps, features)
y_train_arr = np.array(Y).reshape(20, 3, 1) #(samples, time-steps, features)
X_test_arr = np.array(Xt).reshape(20, 3, 1) #(samples, time-steps, features)
y_test_arr = np.array(Yt).reshape(20, 3, 1) #(samples, time-steps, features)

def model_builder(hp):
    hp_units = hp.Int(
        'units',
        min_value=32,
        max_value=512,
        step=32,
        default=128
    )
    hp_activation = hp.Choice(
        'activation',
        values=['relu', 'tanh', 'sigmoid'],
        default='relu'
    )
    hp_dropout = hp.Float(
                'dropout_rate',
                min_value=0.0,
                max_value=0.5,
                default=0.1,
                step=0.05,
            )
    hp_dense = hp.Int(
                    'dense_units',
                    min_value=32,
                    max_value=512,
                    step=32,
                    default=128
            )
    hp_adam = hp.Float(
                    'adam_learning_rate',
                    min_value=1e-4,
                    max_value=1e-2,
                    sampling='LOG',
                    default=1e-3
                )
    model = tf.keras.Sequential()
    model.add(LSTM(units=hp_units, activation=hp_activation, input_shape=(3, 1)))
    model.add(Dropout(rate=hp_dropout))
    model.add(RepeatVector(3))
    model.add(LSTM(units=hp_units, activation=hp_activation, return_sequences=True))
    model.add(Dropout(rate=hp_dropout))
    model.add(TimeDistributed(Dense(units=hp_dense)))
    model.compile(loss='mse', metrics=['mse'], optimizer=keras.optimizers.Adam(hp_adam))
    return model

#TUNE - Hyperband
tuner = kt.Hyperband(model_builder,
                     objective='mse',
                     max_epochs=10,
                     factor=3,
                     directory=os.path.normpath('C:/keras_tuning'),
                     project_name='intro_to_kt',
                     overwrite=True)

stop_early = tf.keras.callbacks.EarlyStopping(monitor='val_loss', patience=5)

tuner.search(X_train_arr, y_train_arr, epochs=50, validation_data=(X_test_arr, y_test_arr), callbacks=[stop_early], verbose=2)

# Get the optimal hyperparameters
best_hps=tuner.get_best_hyperparameters(num_trials=1)[0]

print(f"""
The hyperparameter search is complete. The optimal number of units in the first densely-connected
layer is {best_hps.get('units')} and the optimal learning rate for the optimizer
is {best_hps.get('learning_rate')}.
""")

# Build the model with the optimal hyperparameters and train it on the data for 50 epochs
model = tuner.hypermodel.build(best_hps)
history = model.fit(X_train_arr, y_train_arr, epochs=50, validation_data=(X_test_arr, y_test_arr), verbose=2)

val_acc_per_epoch = history.history['val_accuracy']
best_epoch = val_acc_per_epoch.index(max(val_acc_per_epoch)) + 1
print('Best epoch: %d' % (best_epoch,))

hypermodel = tuner.hypermodel.build(best_hps)

# Retrain the model
hypermodel.fit(X_train_arr, y_train_arr, epochs=best_epoch, validation_data=(X_test_arr, y_test_arr), verbose=2)

eval_result = hypermodel.evaluate(X_test_arr, y_test_arr)
print("[test loss, test accuracy]:", eval_result)

#CONCLUSION: