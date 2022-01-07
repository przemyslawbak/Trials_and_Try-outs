#https://datascience.stackexchange.com/a/73755/130601

from tensorflow import keras
from kerastuner.tuners import BayesianOptimization
import numpy as np
import tensorflow as tf
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

def build_model(hp):
    model = tf.keras.Sequential()
    model.add(LSTM(units=hp.Int('units',min_value=32,
                                    max_value=512,
                                    step=32), 
               activation='relu', input_shape=(3, 1)))
    model.add(Dense(units=hp.Int('units',min_value=32,
                                    max_value=512,
                                    step=32), activation='relu'))
    model.add(Dense(1))
    model.compile(loss='mse', metrics=['mse'], optimizer=keras.optimizers.Adam(
        hp.Choice('learning_rate',
                  values=[1e-2, 1e-3, 1e-4])))
    return model

bayesian_opt_tuner = BayesianOptimization(
    build_model,
    objective='mse',
    max_trials=3,
    executions_per_trial=1,
    directory=os.path.normpath('C:/keras_tuning'),
    project_name='intro_to_kt',
    overwrite=True)

bayesian_opt_tuner.search(X_train_arr, y_train_arr,epochs=5,
     validation_data=(X_test_arr, y_test_arr),verbose=1)


bayes_opt_model_best_model = bayesian_opt_tuner.get_best_models(num_models=1)
model = bayes_opt_model_best_model[0]
print('model:')
print(model.summary())