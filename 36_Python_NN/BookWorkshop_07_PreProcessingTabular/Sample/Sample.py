import tensorflow as tf
import pandas as pd
import matplotlib.pyplot as plt

#reading CSV dataset:
df = pd.read_csv('Bias_correction_ucl.csv')

#In order to normalize the data, you can use a scaler that is available in scikit-learn
#scaler = StandardScaler()
#transformed_df = scaler.fit_transform(df)

#exercise
df.drop('Date', inplace=True, axis=1)
ax = df['Present_Tmax'].hist(color='gray')
ax.set_xlabel("Temperature")
ax.set_ylabel("Frequency")

#converting non-numerical columns into numerical dummies:
dummies = pd.get_dummies(df['feature1'], prefix='feature1')

df2 = pd.read_csv('Bias_correction_ucl.csv')
df2['Date'] = pd.to_datetime(df2['Date'])
year_dummies = pd.get_dummies(df2['Date'].dt.year, \
prefix='year')

#Concatenate the original DataFrame and the dummy DataFrames you created
df = pd.concat([df2, year_dummies], \
axis=1)

plt.show()