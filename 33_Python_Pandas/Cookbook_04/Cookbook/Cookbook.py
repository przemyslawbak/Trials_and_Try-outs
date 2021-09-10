import pandas as pd
import numpy as np
import datetime
import pandas_datareader.data as web
import requests_cache

college = pd.read_csv("data/college.csv")

print(college.sample(random_state=42))
#Selecting the smallest of the largest
movie = pd.read_csv("data/movie.csv")
movie2 = movie[["movie_title", "imdb_score", "budget"]]
movie2.head()
print(movie2.nlargest(100, "imdb_score").head())
#Selecting the largest of each group by sorting
movie = pd.read_csv("data/movie.csv")
movie[
["movie_title", "title_year", "imdb_score"]
].sort_values("title_year", ascending=True)

#calculating trailing stop for stocks
session = requests_cache.CachedSession(
cache_name="cache",
backend="sqlite",
expire_after=datetime.timedelta(days=90),
)
tsla = web.DataReader(
"tsla",
data_source="yahoo",
start="2017-1-1",
session=session, #exception
)
print(tsla.head(8))