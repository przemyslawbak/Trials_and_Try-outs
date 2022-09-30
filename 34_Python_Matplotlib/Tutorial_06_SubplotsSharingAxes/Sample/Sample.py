import matplotlib.pyplot as plt
import numpy as np

#https://matplotlib.org/stable/gallery/subplots_axes_and_figures/subplots_demo.html

# Some example data to display
x = np.linspace(0, 2 * np.pi, 400)
y = np.sin(x ** 2)

# Sharing axes
fig, axs = plt.subplots(3, sharex=True, sharey=True)
fig.suptitle('Sharing both axes')
axs[0].plot(x, y ** 2)
axs[1].plot(x, 0.3 * y, 'o')
axs[2].plot(x, y, '+')

print('plotted...')

plt.show()