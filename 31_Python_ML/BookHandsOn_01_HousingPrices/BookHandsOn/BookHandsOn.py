
import os
import tarfile
import urllib
import pandas as pd
import matplotlib.pyplot as plt
import numpy as np
from zlib import crc32
from sklearn.model_selection import train_test_split
from sklearn.model_selection import StratifiedShuffleSplit
from pandas.plotting import scatter_matrix
from sklearn.impute import SimpleImputer
from sklearn.base import BaseEstimator, TransformerMixin
from sklearn.pipeline import Pipeline
from sklearn.preprocessing import StandardScaler
from sklearn.compose import ColumnTransformer
from sklearn.compose import ColumnTransformer
from sklearn.linear_model import LinearRegression
from sklearn.preprocessing import OneHotEncoder
from sklearn.metrics import mean_squared_error
from sklearn.tree import DecisionTreeRegressor

#1. RETREIVING AND SORTING

DOWNLOAD_ROOT = "/"
HOUSING_PATH = os.path.join("datasets", "housing")
HOUSING_URL = DOWNLOAD_ROOT + "datasets/housing/housing.tgz"

#retreiving file and extracting csv to the directory
def fetch_housing_data(housing_url=HOUSING_URL, housing_path=HOUSING_PATH):
    os.makedirs(housing_path, exist_ok=True)
    tgz_path = os.path.join(housing_path, "housing.tgz")
    housing_tgz = tarfile.open(tgz_path)
    housing_tgz.extractall(path=housing_path)
    housing_tgz.close()

#loading data to the variable
def load_housing_data(housing_path=HOUSING_PATH):
    csv_path = os.path.join(housing_path, "housing.csv")
    return pd.read_csv(csv_path)

fetch_housing_data()
data = load_housing_data()

#display data in console
print(data.info()) #data types / columns
print(data["ocean_proximity"].value_counts()) #how many various ocean_proximity
print(data.describe())

#display data graphs / charts
data.hist(bins=50, figsize=(20,15))

#checks for randomize identifier 
def test_set_check(identifier, test_ratio):
    """
    This ensures that the
    test set will remain consistent across multiple runs, even if you refresh the dataset
    """
    return crc32(np.int64(identifier)) & 0xffffffff < test_ratio * 2**32

#method creating train and test set
def split_train_test_by_id(data, test_ratio, id_column):
    """
    If you run the program again, it will generate a
    different test set
    """
    ids = data[id_column]
    in_test_set = ids.apply(lambda id_: test_set_check(id_, test_ratio))
    return data.loc[~in_test_set], data.loc[in_test_set]

"""
district’s latitude and longitude are guaranteed to be stable for a few
million years
"""
housing_with_id = data.reset_index()
housing_with_id["id"] = data["longitude"] * 1000 + data["latitude"]


#using custom method, display histogram
data["income_cat"] = pd.cut(data["median_income"], bins=[0., 1.5, 3.0, 4.5, 6., np.inf], labels=[1, 2, 3, 4, 5])
train_set, test_set = split_train_test_by_id(housing_with_id, 0.2, "id")
data["income_cat"].hist() #no display?
print(data["income_cat"])

#using sklearn, display histogram
data["income_cat"] = pd.cut(data["median_income"], bins=[0., 1.5, 3.0, 4.5, 6., np.inf], labels=[1, 2, 3, 4, 5])
train_set, test_set = train_test_split(data, test_size=0.2, random_state=42)
data["income_cat"].hist() #no display?
print(data["income_cat"])

#stratified sampling based on the income category
split = StratifiedShuffleSplit(n_splits=1, test_size=0.2, random_state=42)
for train_index, test_index in split.split(data, data["income_cat"]):
    strat_train_set = data.loc[train_index]
    strat_test_set = data.loc[test_index]
strat_test_set["income_cat"].value_counts() / len(strat_test_set)

#Now we should remove the income_cat attribute so the data is back to its original state
for set_ in (strat_train_set, strat_test_set):
    set_.drop("income_cat", axis=1, inplace=True)

#A better visualization that highlights high-density areas (alpha=0.1)
data.plot(kind="scatter", x="longitude", y="latitude", alpha=0.4, s=data["population"]/100, label="population", figsize=(10,7), c="median_house_value", cmap=plt.get_cmap("jet"), colorbar=True)
plt.legend()

#standard corelations
corr_matrix = data.corr()
"""
The correlation coefficient ranges from –1 to 1. When it is close to 1, it means that
there is a strong positive correlation; for example, the median house value tends to go
up when the median income goes up. When the coefficient is close to –1, it means
that there is a strong negative correlation; you can see a small negative correlation
between the latitude and the median house value (i.e., prices have a slight tendency to
go down when you go north). Finally, coefficients close to 0 mean that there is no
linear correlation. Figure 2-14 shows various plots along with the correlation coefficient
between their horizontal and vertical axes.
"""
print(corr_matrix["median_house_value"].sort_values(ascending=False))

#Another way to check for correlation between attributes
"""
This plot reveals a few things. First, the correlation is indeed very strong; you can
clearly see the upward trend, and the points are not too dispersed. Second, the price
cap that we noticed earlier is clearly visible as a horizontal line at $500,000. But this
plot reveals other less obvious straight lines: a horizontal line around $450,000,
another around $350,000, perhaps one around $280,000, and a few more below that.
"""
attributes = ["median_house_value", "median_income", "total_rooms", "housing_median_age"]
scatter_matrix(data[attributes], figsize=(12, 8))
data.plot(kind="scatter", x="median_income", y="median_house_value", alpha=0.1)

#cont. proparing data
data["rooms_per_household"] = data["total_rooms"]/data["households"]
data["bedrooms_per_room"] = data["total_bedrooms"]/data["total_rooms"]
data["population_per_household"] = data["population"]/data["households"]

#2. PREPARING DATA

#let’s revert to a clean training set
data = strat_train_set.drop("median_house_value", axis=1)
housing_labels = strat_train_set["median_house_value"].copy()

#We saw earlier that the total_bedrooms attribute has some missing values, so let’s fix this. Get rid of the whole attribute
data.drop("total_bedrooms", axis=1)

#Scikit-Learn provides a handy class to take care of missing values
imputer = SimpleImputer(strategy="median")
housing_num = data.drop("ocean_proximity", axis=1)
imputer.fit(housing_num)
X = imputer.transform(housing_num)
housing_tr = pd.DataFrame(X, columns=housing_num.columns, index=housing_num.index)

#here is a small transformer class that adds the combined attributes we discussed earlier (ALTERNATIVE)
rooms_ix, bedrooms_ix, population_ix, households_ix = 3, 4, 5, 6
class CombinedAttributesAdder(BaseEstimator, TransformerMixin):
    def __init__(self, add_bedrooms_per_room = True): # no *args or **kargs
        self.add_bedrooms_per_room = add_bedrooms_per_room
    def fit(self, X, y=None):
        return self # nothing else to do
    def transform(self, X):
        rooms_per_household = X[:, rooms_ix] / X[:, households_ix]
        population_per_household = X[:, population_ix] / X[:, households_ix]
        if self.add_bedrooms_per_room:
            bedrooms_per_room = X[:, bedrooms_ix] / X[:, rooms_ix]
            return np.c_[X, rooms_per_household, population_per_household, bedrooms_per_room]
        else:
            return np.c_[X, rooms_per_household, population_per_household]

attr_adder = CombinedAttributesAdder(add_bedrooms_per_room=False)
housing_extra_attribs = attr_adder.transform(data.values)

#Feature Scaling, Transformation Pipelines
#pipeline for the numerical attributes
num_pipeline = Pipeline([
        ('imputer', SimpleImputer(strategy="median")),
        ('attribs_adder', CombinedAttributesAdder()),
        ('std_scaler', StandardScaler()),
    ])
housing_num_tr = num_pipeline.fit_transform(housing_num)

#apply all the transformations to the housing data
num_attribs = list(housing_num)
cat_attribs = ["ocean_proximity"]
full_pipeline = ColumnTransformer([
    ("num", num_pipeline, num_attribs),
    ("cat", OneHotEncoder(), cat_attribs),
])
housing_prepared = full_pipeline.fit_transform(data)

#3. SELECT AND TRAIN MODEL

#3.1 LinearRegression example
lin_reg = LinearRegression()
lin_reg.fit(housing_prepared, housing_labels)
#test
some_data = data.iloc[:5]
some_labels = housing_labels.iloc[:5]
some_data_prepared = full_pipeline.transform(some_data)
print("Predictions:", lin_reg.predict(some_data_prepared))
print("Labels:", list(some_labels))
#mean squared error
housing_predictions = lin_reg.predict(housing_prepared)
lin_mse = mean_squared_error(housing_labels, housing_predictions)
lin_rmse = np.sqrt(lin_mse)
"""
This is better than nothing, but clearly not a great score: most districts’ median_hous
ing_values range between $120,000 and $265,000, so a typical prediction error of
$68,628 is not very satisfying.
"""
print("mean squared error:", lin_rmse)

#3.2 DecisionTreeRegressor example
tree_reg = DecisionTreeRegressor()
tree_reg.fit(housing_prepared, housing_labels)
#test
housing_predictions = tree_reg.predict(housing_prepared)
tree_mse = mean_squared_error(housing_labels, housing_predictions)
tree_rmse = np.sqrt(tree_mse)
print("mean squared error:", tree_rmse)
"""
Wait, what!? No error at all? Could this model really be absolutely perfect? Of course,
it is much more likely that the model has badly overfit the data. How can you be sure?
As we saw earlier, you don’t want to touch the test set until you are ready to launch a
model you are confident about, so you need to use part of the training set for training
and part of it for model validation.
"""





#plt.show()