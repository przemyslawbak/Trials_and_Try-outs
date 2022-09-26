import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
from matplotlib.colors import ListedColormap, BoundaryNorm

#pyplot sample https://stackoverflow.com/a/47470635
sample = [10,4,6,20, 23, 29, 20]
df = pd.DataFrame(sample)
arr = df.to_numpy()
df1 = pd.DataFrame(arr)
print(df1)
length = df1.shape[0]

ax = df1.iloc[:length -2,:].plot(y=0, color="crimson")
df1.iloc[length -3:,:].plot(y=0, color="C0", ax=ax)



plt.show()