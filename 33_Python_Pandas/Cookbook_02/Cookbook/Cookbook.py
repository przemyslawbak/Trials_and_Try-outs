
import pandas as pd
import numpy as np

movies = pd.read_csv("data/movie.csv")
movie_actor_director = movies[
    [
    "actor_1_name",
    "actor_2_name",
    "actor_3_name",
    "director_name",
    ]
]

print(movie_actor_director.head())
#Selecting columns with methods
def shorten(col):
    return (
    str(col)
    .replace("facebook_likes", "fb")
    .replace("_for_reviews", "")
)
movies = movies.rename(columns=shorten) #using custom method
movies.select_dtypes(include="int").head() #select only the integer columns
movies.select_dtypes(include=["int", "object"]).head() #If we wanted integer and string columns
movies.select_dtypes(exclude="float").head() #exclufing floats
movies.filter(like="fb").head() #alternative way