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
#visualization of flights dataset
flights = pd.read_csv('data/flights.csv')
#display types
cols = ['DIVERTED', 'CANCELLED', 'DELAYED']
(flights
.assign(DELAYED=flights['ARR_DELAY'].ge(15).astype(int),
ON_TIME=lambda df_:1 - df_[cols].any(axis=1))
.select_dtypes(int)
.sum()
)
fig, ax_array = plt.subplots(2, 3, figsize=(18,8))
(ax1, ax2, ax3), (ax4, ax5, ax6) = ax_array
fig.suptitle('2015 US Flights - Univariate Summary', size=20)
ac = flights['AIRLINE'].value_counts()
ac.plot.barh(ax=ax1, title='Airline')
(flights
['ORG_AIR']
.value_counts()
.plot.bar(ax=ax2, rot=0, title='Origin City')
)
(flights
['DEST_AIR']
.value_counts()
.head(10)
.plot.bar(ax=ax3, rot=0, title='Destination City')
)
(flights
.assign(DELAYED=flights['ARR_DELAY'].ge(15).astype(int),
ON_TIME=lambda df_:1 - df_[cols].any(axis=1))
[['DIVERTED', 'CANCELLED', 'DELAYED', 'ON_TIME']]
.sum()
.plot.bar(ax=ax4, rot=0,
log=True, title='Flight Status')
)
flights['DIST'].plot.kde(ax=ax5, xlim=(0, 3000),
title='Distance KDE')
flights['ARR_DELAY'].plot.hist(ax=ax6,
title='Arrival Delay',
range=(0,200)
)










plt.show()