import numpy as np
import pandas as pd

futures = 40

#https://www.analyticsvidhya.com/blog/2020/10/how-to-create-an-arima-model-for-time-series-forecasting-in-python/

# Import data
df = pd.read_csv('GPW_DLY WIG20, 15.csv', usecols=["close", 'open', 'high', 'low'])
df.reset_index(drop=True)
my_data = np.array(df)

def delete_row(data, number):
    data = data[number:, ]
    return data

def add_row(data, times):
    for i in range(1, times + 1):
        columns = np.shape(data)[1]
        new = np.zeros((1, columns), dtype = float)
        data = np.append(data, new, axis = 0)
    return data

def add_column(data, times):
    for i in range(1, times + 1):
        new = np.zeros((len(data), 1), dtype = float)
        data = np.append(data, new, axis = 1)
    return data

def delete_column(data, index, times):
    for i in range(1, times + 1):
        data = np.delete(data, index, axis = 1)
    return data

def rounding(data, how_far):
    data = data.round(decimals = how_far)
    return data

my_data = add_column(my_data, 2)
my_data = delete_column(my_data, 0, 2)

# An array has the following structure: array[row,
#column]
# Referring to the whole my_data array
#my_data
# Referring to the first 100 rows inside the array
#my_data[:100, ]
# Referring to the first 100 rows of the first two
#columns
#my_data[:100, 0:2]
# Referring to all the rows of the seventh column
#my_data[:, 6]
# Referring to the last 500 rows of the array
#my_data[-500:, ]
# Referring to the first row of the first column
#my_data[0, 0]
# Referring to the last row of the last column
#my_data[-1, -1]

#Add two columns to the array:
my_data = add_column(my_data, 2)
#Delete three columns starting from the second column:
my_data = delete_column(my_data, 1, 3)
#Add 11 rows to the end of the array:
my_data = add_row(my_data, 11)
#Delete the first 4 rows in the array:
my_data = delete_row(my_data, 4)
#Round all the values in the array to four decimals:
my_data = rounding(my_data, 4)


print(my_data)