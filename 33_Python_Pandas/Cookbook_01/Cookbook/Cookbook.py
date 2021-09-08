
import pandas as pd
import numpy as np

pd.set_option('max_columns', 4, 'max_rows', 10) #col display options for Pandas
#load data from csv
movies = pd.read_csv('data/movie.csv')
#display
print(movies.head())
#attributes
print(movies.columns)
print(movies.index)
print(movies.to_numpy())
#data types
print(movies.dtypes)
print(movies.dtypes.value_counts())
print(movies.info())
#selecting columns
print(movies["director_name"]) # = movies.director_name
#.loc / .iloc - to pull out a Series
print(movies.loc[:, "director_name"]) # = movies.iloc[:, 1]
