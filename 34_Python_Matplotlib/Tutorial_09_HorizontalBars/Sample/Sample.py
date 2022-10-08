import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
from matplotlib.colors import ListedColormap, BoundaryNorm

#pyplot sample https://stackoverflow.com/a/47470635
sample1 = [10,4,6,20, 23, 29, 20]
sample2 = [12,14,16,25, 21, 19, 10]
df = pd.DataFrame(sample1)
df2 = pd.DataFrame(sample2)

df3 = pd.DataFrame([[0]], columns=['base'])
df3[0] = 11
df3[5] = 21
df3[10] = 33
df3[15] = 38
df3[20] = 12
df3[25] = 28
df3[30] = 16
df3 = df3.drop('base', axis=1) #todo: make index of column names, bars

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
ax2 = ax1.twiny() # ax1 and ax2 share y-axis
ax2.plot(df3, color = 'pink')
ax2.set_xlabel('added')

plt.legend('',frameon=False)

plt.show()