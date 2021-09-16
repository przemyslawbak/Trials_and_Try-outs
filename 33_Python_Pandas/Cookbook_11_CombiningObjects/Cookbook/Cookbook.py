#Combining Pandas Objects
import pandas as pd
import numpy as np
#Read in the names dataset, and output it
names = pd.read_csv('data/names.csv')
print(names)
#Let's create a list that contains some new data and use the .loc attribute to set rows
new_data_list = [['Aria', 1], ['Pszemek', 18]]
print(new_data_list)
names.loc[4] = new_data_list[0]
names.loc[5] = new_data_list[1]
print(names)
#non-integer label
names.loc['six'] = ['Zach', 3]
print(names)
#Concatenating multiple DataFrames
stocks_2016 = pd.read_csv('data/stocks_2016.csv', index_col='Symbol')
stocks_2017 = pd.read_csv('data/stocks_2017.csv', index_col='Symbol')
#single list
s_list = [stocks_2016, stocks_2017]
pd.concat(s_list) #concat
#add
n_list = stocks_2016 + stocks_2017
print(n_list) #problem with nan values
#also .concat, .join, .merge