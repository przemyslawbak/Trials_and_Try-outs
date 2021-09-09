import pandas as pd
import numpy as np
from io import StringIO

#Creating DataFrames from scratch
fname = ["Paul", "John", "Richard", "George"]
lname = ["McCartney", "Lennon", "Starkey", "Harrison"]
birth = [1942, 1940, 1940, 1943]
people = {"first": fname, "last": lname, "birth": birth} #Create a dictionary from the lists, mapping the column name to the list
beatles = pd.DataFrame(people) #Create a DataFrame from the dictionary
print(beatles)
print(beatles.index)

#create a DataFrame from a list of dictionaries
pd.DataFrame(
[
{
"first": "Paul",
"last": "McCartney",
"birth": 1942,
},
{
"first": "John",
"last": "Lennon",
"birth": 1940,
},
{
"first": "Richard",
"last": "Starkey",
"birth": 1940,
},
{
"first": "George",
"last": "Harrison",
"birth": 1943,
},
]
)

#createing CSV file
fout = StringIO()
beatles.to_csv(fout) # use a filename instead of fout to save file
print(fout.getvalue())