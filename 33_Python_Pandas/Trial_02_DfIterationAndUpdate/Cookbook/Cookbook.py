import pandas as pd

pd.set_option('display.max_rows', 1000)
pd.set_option('display.max_columns', 10)
pd.set_option('display.width', 1000)

filename = "test_set.txt"
df = pd.read_csv(filename, sep='|', engine='python', skiprows=[0])

print(df)