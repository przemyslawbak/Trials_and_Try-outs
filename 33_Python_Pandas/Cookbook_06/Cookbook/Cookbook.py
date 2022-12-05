#Selecting Subsets of Data
import pandas as pd
import numpy as np

college = pd.read_csv("data/college.csv", index_col="INSTNM")
city = college["CITY"]
print(city)
#3 ways of puling out scalar value
city["Alabama A & M University"]
city.loc["Alabama A & M University"]
city.iloc[0]
#pulling out several vaues
city[
[
"Alabama A & M University",
"Alabama State University",
]
]
city.iloc[[0, 4]]
city[
"Alabama A & M University":"Alabama State University" #slice
]
city[0:5] #slice by position
#same with loc & iloc, as above
#We can pass in a tuple (without parentheses) of row and column labels or positions, respectively
college.iloc[[0, 4], 0]
#Selecting DataFrame rows
#To select an entire row at that position, pass an integer to .iloc
college.iloc[60]
college.loc["University of Alaska Anchorage"] #alternative
college.iloc[[60, 99, 3]] #3 rows
labels = [
"University of Alaska Anchorage",
"International Academy of Hair Design",
"University of Alabama in Huntsville",
]
college.loc[labels] #same as above
#formula for selecting row and column:
df.iloc[row_idxs, column_idxs]
df.loc[row_names, column_names]
#first 3 rows and first 4 columns:
college.iloc[:3, :4]
college.loc[:"Amridge University", :"MENONLY"] #alternative
#Select disjointed rows and columns:
college.iloc[[100, 200], [7, 15]]
#Single scalar value
college.iloc[5, -4]
#Slicing lexicographically (need to sort index first)
college = college.sort_index()
college.loc["Sp":"Su"]