
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
#Chaining Series methods examples
director.value_counts().head(3)
fb_likes.isna().sum()
fb_likes.fillna(0).astype(int).head()
#Renaming column names
col_map = {
"director_name": "director",
"num_critic_for_reviews": "critic_reviews",
}
print(movies.rename(columns=col_map).head())

idx_map = {
"Avatar": "Ratava",
"Spectre": "Ertceps",
"Pirates of the Caribbean: At World's End": "POC",
}
col_map = {
"aspect_ratio": "aspect",
"movie_facebook_likes": "fblikes",
}
print(movies.set_index("movie_title").rename(index=idx_map, columns=col_map).head(3))
#create column
movies["has_seen"] = 0 #mutates existing df
idx_map = {
"Avatar": "Ratava",
"Spectre": "Ertceps",
"Pirates of the Caribbean: At World's End": "POC",
}
col_map = {
"aspect_ratio": "aspect",
"movie_facebook_likes": "fblikes",
}
movies.rename(
index=idx_map, columns=col_map
).assign(has_seen=0) #creates new df
movies.assign(total_likes=sum_col).head(5)
movies.assign(total_likes=total
              .fillna(0))[ #fills missing values
"total_likes"
]
#inserting column
profit_index = movies.columns.get_loc("gross") + 1 #9
movies.insert( #mutates existing df
loc=profit_index,
column="profit",
value=movies["gross"] - movies["budget"],
)
#deleting column
df2 = df2.drop(columns="total_likes")
del movies["director_name"]