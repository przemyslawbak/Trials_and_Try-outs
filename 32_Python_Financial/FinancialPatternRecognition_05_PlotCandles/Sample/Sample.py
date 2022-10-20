import numpy as np
import pandas as pd
import matplotlib.pyplot as plt

# Import data
df = pd.read_csv('GPW_DLY WIG20, 15.csv', usecols=["open", 'high', 'low', 'close'])
df.reset_index(drop=True)
my_data = np.array(df)

def delete_row(data, number):
    data = data[number:, ]
    return data

def add_column(data, times):
    for i in range(1, times + 1):
        new = np.zeros((len(data), 1), dtype = float)
        data = np.append(data, new, axis = 1)
    return data

def ma(data, lookback, close, position):
    data = add_column(data, 1)
    for i in range(len(data)):
        try:
            data[i, position] = (data[i - lookback + 1:i + 1,
close].mean())
        except IndexError:
            pass
    data = delete_row(data, lookback)
    return data

def ohlc_plot_candles(data, window):
    sample = data[-window:, ]
    for i in range(len(sample)):
        plt.vlines(x = i, ymin = sample[i, 2], ymax = sample[i, 1],color = 'black', linewidth = 1)
        if sample[i, 3] > sample[i, 0]:
            plt.vlines(x = i, ymin = sample[i, 0], ymax = sample[i, 3],
color = 'green', linewidth = 3)
        if sample[i, 3] < sample[i, 0]:
            plt.vlines(x = i, ymin = sample[i, 3], ymax = sample[i, 0],
color = 'red', linewidth = 3)
        if sample[i, 3] == sample[i, 0]:
            plt.vlines(x = i, ymin = sample[i, 3], ymax = sample[i, 0] +
0.00003, color = 'black', linewidth = 1.00)
    plt.grid()

# Setting the lookback period
lookback = 30
# Setting the index of the close price column
close_column = 3
# Setting the index of the moving average column
ma_column = 4
# Calling the moving average function
my_data = ma(my_data, lookback, close_column, ma_column)
print(my_data)

ohlc_plot_candles(my_data, window = 100)