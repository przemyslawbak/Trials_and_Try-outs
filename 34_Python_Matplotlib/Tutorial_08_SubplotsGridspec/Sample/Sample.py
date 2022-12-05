import matplotlib.pyplot as plt
import numpy as np

#https://matplotlib.org/stable/gallery/subplots_axes_and_figures/subplots_demo.html

# Some example data to display
x = np.linspace(0, 2 * np.pi, 400)
y = np.sin(x ** 2)

# Sharing axes with titles
fig, axs = plt.subplots(4, gridspec_kw={'height_ratios':[2, 1, 1, 2]})
axs[0].plot(x, y)
axs[0].set_title("main")
axs[1].plot(x, y**2)
axs[1].set_title("shares x with main")
axs[2].plot(x + 1, y + 1)
axs[2].set_title("unrelated")
axs[3].plot(x + 2, y + 2)
axs[3].set_title("also unrelated")
fig.tight_layout()

print('plotted...')

plt.show()