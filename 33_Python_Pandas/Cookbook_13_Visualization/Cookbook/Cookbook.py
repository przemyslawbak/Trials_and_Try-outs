#Visualization
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
from matplotlib.figure import Figure
from matplotlib.backends.backend_agg import FigureCanvasAgg as FigureCanvas
import matplotlib.pyplot as plt
from IPython.core.display import display #???

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
display(fig)
#combination of two approaches
fig, ax = plt.subplots(figsize=(15,3))
ax.plot(x, y)
ax.set(xlim=(0, 10), ylim=(-3, 8),
xlabel='X axis', ylabel='Y axis',
title='Line Plot')
fig.suptitle('Figure Title', size=20, y=1.03)
fig, ax = plt.subplots(nrows=1, ncols=1)






plt.show()