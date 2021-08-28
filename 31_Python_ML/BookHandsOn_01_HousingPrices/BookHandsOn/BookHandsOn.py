
import os
import tarfile
import urllib
import pandas as pd
import matplotlib.pyplot as plt

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
plt.show()

