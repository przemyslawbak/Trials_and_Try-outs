import numpy as np
import pandas as pd
from sklearn.preprocessing import MinMaxScaler
from keras.models import Sequential
from keras.layers import *
from keras.optimizers import *
import matplotlib.pyplot as plt

# Data prep
df = pd.read_csv('data.csv')
df = df.drop('USD', axis=1)
df['Date'] = pd.to_datetime(df["Date"])
indexed_df = df.set_index(["Date"], drop=True)

indexed_df = indexed_df[indexed_df.INR != 0.0]
indexed_df = indexed_df.iloc[::-1]

shifted_df= indexed_df.shift()
concat_df = [indexed_df, shifted_df]
data = pd.concat(concat_df,axis=1)
# Replace NaNs with 0
data.fillna(0, inplace=True)

data = np.array(data)
# train test split, we can take last 500 data points as test set
train , test = data[0:-500], data[-500:]

# Scale
scaler = MinMaxScaler()
train_scaled = scaler.fit_transform(train)
test_scaled = scaler.transform(test)

# train data
y_train = train_scaled[:,-1]
X_train = train_scaled[:,0:-1]
X_train = X_train.reshape(len(X_train),1,1)

#test data
y_test = test_scaled[:,-1]
X_test = test_scaled[:,0:-1]


# Model
model = Sequential()
model.add(GRU(75, input_shape=(1,1)))
model.add(Dense(1))
optimizer = Adam(lr=1e-3)
model.compile(loss='mean_squared_error', optimizer=optimizer, metrics=['accuracy'])
model.fit(X_train, y_train, epochs=100, batch_size=20, shuffle=False)