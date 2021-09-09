import pandas as pd
import numpy as np
from io import StringIO
import zipfile

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

#loading from file
diamonds = pd.read_csv("data/diamonds.csv", nrows=1000)
print(diamonds.info())#78.2+ KB
#changed data type precission
diamonds2 = pd.read_csv(
"data/diamonds.csv",
nrows=1000,
dtype={
"carat": np.float32,
"depth": np.float32,
"table": np.float32,
"x": np.float32,
"y": np.float32,
"z": np.float32,
"price": np.int16,
},
)
diamonds2.info() #49.0+ KB
#save to inary format
diamonds2.to_feather("data/d.arr") #optional: diamonds2.to_parquet("data/d.pqt")
#save to excel
beatles.to_excel("data/beat.xlsx")
#reading excel
beat2 = pd.read_excel("data/beat.xls") #optional select sheet name
#reading zipped single file
autos = pd.read_csv("data/vehicles.csv.zip")
#reading zipped multiple files
with zipfile.ZipFile(
"data/kaggle-survey-2018.zip"
) as z:
    print("\n".join(z.namelist()))
    kag = pd.read_csv(z.open("multipleChoiceResponses.csv"))
    kag_questions = kag.iloc[0]
    survey = kag.iloc[1:]
print(survey.head(2).T)
#also reading:
#sql: pd.read_sql
#json: pd.read_json
#html table