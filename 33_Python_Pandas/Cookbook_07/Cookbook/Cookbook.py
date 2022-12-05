#Filtering Rows
import pandas as pd
import numpy as np
movie = pd.read_csv("data/movie.csv", index_col="movie_title")
print(movie[["duration"]].head())
#show longer than 2h
movie_2_hours = movie["duration"] > 120
print(movie_2_hours.head(10))
print(movie_2_hours.describe())
#For instance, we could determine the percentage of movies that have actor 1 with more Facebook likes than actor 2.
actors = movie[
["actor_1_facebook_likes", "actor_2_facebook_likes"]
].dropna()
(
actors["actor_1_facebook_likes"]
> actors["actor_2_facebook_likes"]
).mean()
#Constructing multiple Boolean conditions
criteria1 = movie.imdb_score > 8
criteria2 = movie.content_rating == "PG-13"
criteria3 = (movie.title_year < 2000) | (
movie.title_year > 2009
)
criteria_final = criteria1 & criteria2 & criteria3
print(criteria_final.head())
#Filtering with Boolean arrays
crit_a1 = movie.imdb_score > 8
crit_a2 = movie.content_rating == "PG-13"
crit_a3 = (movie.title_year < 2000) | (
movie.title_year > 2009)
final_crit_a = crit_a1 & crit_a2 & crit_a3
print(final_crit_a)
final_crit_all = final_crit_a | final_crit_b
movie[final_crit_all].head() #Once you have your Boolean array, you pass it to the index operator to filter the data
movie.loc[final_crit_all].head() #We can also filter off of the .loc attribute
#Comparing row filtering and index filtering
college = pd.read_csv("data/college.csv")
college[college["STABBR"] == "TX"].head() #meh
#Selecting with unique and sorted indexes
college2 = college.set_index("STABBR")
college2.index.is_monotonic #False
college3 = college2.sort_index()
college3.index.is_monotonic #True
college_unique.index.is_unique #True
#additional: .mask and .where