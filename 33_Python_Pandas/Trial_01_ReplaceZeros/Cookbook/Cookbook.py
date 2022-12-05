#Visualization with Pandas
import pandas as pd

array = [12.1,42.3,4.9,95.6,0.0,0.0,22.2,33.9,44.2,55.8,0.0,0.0,0.0,77.2,22.1,33.9,44.4,11.5]

df = pd.DataFrame(array, columns = ['some_column'])

df['some_column'] = df['some_column'].replace(to_replace=0, method='ffill')

print(df)