#Exploratory Data Analysis
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import seaborn as sns
#Summary statistics (mean, quartiles, and standard deviation)
fueleco = pd.read_csv("data/vehicles.csv.zip")
print(fueleco.mean())
print(fueleco.std())
print(fueleco.describe()) #all
#Column types
print(fueleco.dtypes)
print(fueleco.dtypes.value_counts())
"""
When pandas converts columns to floats or integers, it uses the 64-bit versions of those
types. If you know that your integers fail into a certain range (or you are willing to sacrifice
some precision on floats), you can save some memory by converting these columns to
columns that use less memory.
"""
(
fueleco[["city08", "comb08"]]
.assign(
city08=fueleco.city08.astype(np.int16),
comb08=fueleco.comb08.astype(np.int16),
)
.info(memory_usage="deep")
)
#Categorical data
print(fueleco.select_dtypes(object).columns)
print(fueleco.drive.nunique())
#display top 6
top_n = fueleco.make.value_counts().index[:6]
(
fueleco.assign(
make=fueleco.make.where(
fueleco.make.isin(top_n), "Other"
)
).make.value_counts()
)
#visualization
fig, ax = plt.subplots(figsize=(10, 8))
top_n = fueleco.make.value_counts().index[:6]
(fueleco     # doctest: +SKIP
   .assign(make=fueleco.make.where(
              fueleco.make.isin(top_n),
              'Other'))
   .make
   .value_counts()
   .plot.bar(ax=ax)
)
#plt.show() #window
#fig.savefig("data/c5-catpan.png", dpi=300) #file
#Visualize the correlations in a heatmap
fig, ax = plt.subplots(figsize=(8, 8))
corr = fueleco[
["city08", "highway08", "cylinders"]
].corr()
mask = np.zeros_like(corr, dtype=np.bool)
mask[np.triu_indices_from(mask)] = True
sns.heatmap(
corr,
mask=mask,
fmt=".2f",
annot=True,
ax=ax,
cmap="RdBu",
vmin=-1,
vmax=1,
square=True,
)
plt.show()