
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
#column attributes
print( movies.director_name.index)
print( movies.director_name.dtype)
print( movies.director_name.size)
print( movies.director_name.name)
#series methods
director = movies["director_name"]
fb_likes = movies["actor_1_facebook_likes"]
print(director.dtype)
print(fb_likes.dtype)
#printing series
print(director.head())
print(director.sample(n=5, random_state=42))
print(director.value_counts())
print(fb_likes.describe())
print(director.describe())
#missing values
directors_na = director.isna()
fb_likes_filled = fb_likes.fillna(0)
fb_likes_dropped = fb_likes.dropna()
#series operations
imdb_score = movies["imdb_score"]
print(imdb_score)
print(imdb_score + 1) #also other arithmetic operators
print(imdb_score > 7) #True/False