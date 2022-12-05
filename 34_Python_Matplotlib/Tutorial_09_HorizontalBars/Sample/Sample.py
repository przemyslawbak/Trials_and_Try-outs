import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
from matplotlib.colors import ListedColormap, BoundaryNorm

#pyplot sample https://stackoverflow.com/a/47470635
sample1 = [10,4,6,20, 23, 29, 20]
sample2 = [12,14,16,25, 21, 19, 10]
df = pd.DataFrame(sample1)
df2 = pd.DataFrame(sample2)

df4 = pd.DataFrame([[0]], columns=['val'])
for i in range(df2[0].max() + 1 + 10):
    df4.loc[i, 'val'] = 0

df4.xs(0)['val'] = 1.1
df4.xs(5)['val'] = 2.1
df4.xs(10)['val'] = 3.3
df4.xs(15)['val'] = 3.8
df4.xs(20)['val'] = 1.2
df4.xs(25)['val'] = 2.8
df4.xs(30)['val'] = 1.6
df4.xs(35)['val'] = 1.2

print(df4)

arr = df.to_numpy()
df1 = pd.DataFrame(arr)
length = df1.shape[0]

#ax = df1.iloc[:length -2,:].plot(color="crimson")
#df1.iloc[length -3:,:].plot(color="C0", ax=ax)
ax = df1.iloc[:length -2,:]
future = df1.iloc[length -3:,:]

fig, ax1 = plt.subplots()

ax1.plot(ax, color = '#B0B0B0')
ax1.plot(df2, color = '#B0B0B0')
ax1.plot(future, color = 'red')
ax1.set_xlabel('first')

#https://stackoverflow.com/a/46324546
#https://stackoverflow.com/q/30228069/12603542
width = 3 # the width of the bars
ind = np.arange(len(df4['val']))  # the x locations for the groups
ax2 = ax1.twiny() # ax1 and ax2 share y-axis
ax2.barh(ind, df4['val'], width, color="blue", alpha=0.3)
ax2.set_xlabel('added')

plt.legend('',frameon=False)

plt.show()