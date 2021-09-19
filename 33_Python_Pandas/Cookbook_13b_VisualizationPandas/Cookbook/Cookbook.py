#Visualization with Pandas
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt

#basics
df = pd.DataFrame(index=['Atiya', 'Abbas', 'Cornelia',
'Stephanie', 'Monte'],
data={'Apples':[20, 10, 40, 20, 50],
'Oranges':[35, 40, 25, 19, 33]})
print(df)
color = ['.2', '.7']
ax = df.plot.bar(color=color, figsize=(16,4)) #Bar plots use the index as the labels for the x-axis and the column values as the bar highlights
ax = df.plot.kde(color=color, figsize=(16,4)) #A KDE plot ignores the index and uses the column names along the x-axis and uses the column values to calculate a probability density along the y values
#multiple variables
fig, (ax1, ax2, ax3) = plt.subplots(1, 3, figsize=(16,4))
fig.suptitle('One Variable Plots', size=20, y=1.02)
df.plot.kde(color=color, ax=ax1, title='KDE plot')
df.plot.box(ax=ax2, title='Boxplot')
df.plot.hist(color=color, ax=ax3, title='Histogram')
plt.show()
