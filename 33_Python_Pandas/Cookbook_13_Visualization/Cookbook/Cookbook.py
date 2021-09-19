#Visualization
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
from matplotlib.figure import Figure
from matplotlib.backends.backend_agg import FigureCanvasAgg as FigureCanvas
import matplotlib.pyplot as plt

#Getting started with matplotlib
#making a line
x = [-3, 5, 7]
y = [10, 2, 5]
fig = plt.figure(figsize=(15,3))
plt.plot(x, y)
plt.xlim(0, 10)
plt.ylim(-3, 8)
plt.xlabel('X Axis')
plt.ylabel('Y axis')
plt.title('Line Plot')
plt.suptitle('Figure Title', size=20, y=1.03)
#optional object oriened approach
fig = Figure(figsize=(15, 3))
FigureCanvas(fig)
ax = fig.add_subplot(111)
ax.plot(x, y)
ax.set_xlim(0, 10)
ax.set_ylim(-3, 8)
ax.set_xlabel('X axis')
ax.set_ylabel('Y axis')
ax.set_title('Line Plot')
fig.suptitle('Figure Title', size=20, y=1.03)
#combination of two approaches
fig, ax = plt.subplots(figsize=(15,3))
ax.plot(x, y)
ax.set(xlim=(0, 10), ylim=(-3, 8),
xlabel='X axis', ylabel='Y axis',
title='Line Plot')
fig.suptitle('Figure Title', size=20, y=1.03)
fig, ax = plt.subplots(nrows=1, ncols=1)
#we can give each one a unique facecolor
fig.set_facecolor('.7')
ax.set_facecolor('.5')
fig
#change spine position
spines = ax.spines
spine_left = spines['left']
spine_left.set_position(('outward', -100))
spine_left.set_linewidth(5)
spine_bottom = spines['bottom']
spine_bottom.set_visible(False)
#Visualizing data with matplotlib
alta = pd.read_csv('data/alta-noaa-1980-2019.csv')
print(alta)
data = (alta
.assign(DATE=pd.to_datetime(alta.DATE))
.set_index('DATE')
.loc['2018-09':'2019-08'] #Get the data for the 2018-2019 season
.SNWD
)
blue = '#99ddee'
white = '#ffffff'
fig, ax = plt.subplots(figsize=(12,4),
linewidth=5, facecolor=blue)
ax.set_facecolor(blue)
ax.spines['top'].set_visible(False)
ax.spines['right'].set_visible(False)
ax.spines['bottom'].set_visible(False)
ax.spines['left'].set_visible(False)
ax.tick_params(axis='x', colors=white)
ax.tick_params(axis='y', colors=white)
ax.set_ylabel('Snow Depth (in)', color=white)
ax.set_title('2009-2010', color=white, fontweight='bold')
ax.fill_between(data.index, data, color=white)
#also possible to plot several charts


plt.show()