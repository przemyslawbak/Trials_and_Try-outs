
import pandas as pd
import numpy as np

pd.set_option('max_columns', 4, 'max_rows', 10) #col display options for Pandas

movies = pd.read_csv('data/movie.csv')
print(movies.head())