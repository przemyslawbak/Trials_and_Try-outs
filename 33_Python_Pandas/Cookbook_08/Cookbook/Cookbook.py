import pandas as pd
import numpy as np
college = pd.read_csv("data/college.csv")
columns = college.columns
print(columns)
#Examining the Index object
columns[5] #Select items from the index by position with a scalar, list, or slice
columns[[1, 8, 10]] #Select items from the index by position with a scalar, list, or slice
print(columns + "_A") #You can use basic arithmetic and comparison operators on Index objects
print(columns > "G") #You can use basic arithmetic and comparison operators on Index objects
#Indexes are immutable
columns[1] = "city" #TypeError: Index does not support mutable operations
#Filling values with unequal indexes
baseball_14 = pd.read_csv(
"data/baseball14.csv", index_col="playerID"
)
baseball_15 = pd.read_csv(
"data/baseball15.csv", index_col="playerID"
)
baseball_16 = pd.read_csv(
"data/baseball16.csv", index_col="playerID"
)
#Use the .difference method on the index to discover which index labels are in
#baseball_14 and not in baseball_15, and vice versa
print(baseball_14.index.difference(baseball_15.index))
print(baseball_15.index.difference(baseball_14.index))