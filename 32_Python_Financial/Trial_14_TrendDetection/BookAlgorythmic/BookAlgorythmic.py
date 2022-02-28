#https://scipy-lectures.org/intro/scipy/auto_examples/plot_detrend.html

#trend
import numpy as np
from matplotlib import pyplot as plt
from scipy import signal
import pandas as pd

wig20_d = pd.read_csv('../../data/wig20_d_1.csv')
x_raw = wig20_d['Zamkniecie'].to_numpy()
period = 100

#n = 150
#t = np.linspace(0, 10, n)
#x_raw = .4 * t + np.random.normal(size=n)

x_raw = x_raw[-period:]

x = signal.detrend(x_raw)
d = x_raw - x

is_positive_trend = d[-1] > d[0]
m = "+" if is_positive_trend else "-"

plt.figure(figsize=(5, 4))
plt.plot(x_raw, label=f"Raw data ({m})")
plt.legend(loc='best')
plt.show()