import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
from matplotlib.colors import ListedColormap, BoundaryNorm

#pyplot sample https://stackoverflow.com/a/47470635
sample1 = [10,4,6,20, 23, 29, 20]
sample2 = [12,14,16,25, 21, 19, 10]
df = pd.DataFrame(sample1)
df2 = pd.DataFrame(sample2)
arr = df.to_numpy()
df1 = pd.DataFrame(arr)
print(df1)
length = df1.shape[0]

ax = df1.iloc[:length -2,:].plot(color="crimson")
df1.iloc[length -3:,:].plot(color="C0", ax=ax)

plt.plot(sample2, label = "Real values")

plt.title("sample")
plt.xlabel('Timeaxis')
plt.ylabel('Index value')
plt.legend('',frameon=False)

plt.show()