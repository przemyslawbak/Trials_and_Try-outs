import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
from matplotlib.colors import ListedColormap, BoundaryNorm

#pyplot sample https://stackoverflow.com/a/47470635
sample = [10,4,6,20, 23, 29, 20]
df = pd.DataFrame(sample)
df = df.reset_index(drop=False)

ax = df.iloc[:5,:].plot(y=0, color="crimson")
df.iloc[4:,:].plot(y=0, color="C0", ax=ax)



plt.show()