
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
movies.filter(like="fb").head() #alternative way, allows regex
#Summarizing a DataFrame
print(movies.describe().T)
#Chaining DataFrame methods
print(movies.isnull().sum().sum())
print(movies.isnull().any().any())
print(movies.isnull().dtypes.value_counts())
movies.select_dtypes(["object"]).fillna("").max() #selects obj types and fills empty
#DataFrame operations
colleges = pd.read_csv("data/college.csv", index_col="INSTNM")
college_ugds = colleges.filter(like="UGDS_")
print(college_ugds.head())
college_ugds_op_round = (
(college_ugds + 0.00501) // 0.01 / 100 #rounding
)
print(college_ugds_op_round.head())