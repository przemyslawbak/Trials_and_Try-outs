import matplotlib.pyplot as plt
import numpy as np

#https://matplotlib.org/stable/gallery/subplots_axes_and_figures/subplots_demo.html

# Some example data to display
x = np.linspace(0, 2 * np.pi, 400)
y = np.sin(x ** 2)

# Subplots
fig, axs = plt.subplots(2)
fig.suptitle('Vertically stacked subplots')
axs[0].plot(x, y)
axs[1].plot(x, -y)

print('plotted...')

plt.show()