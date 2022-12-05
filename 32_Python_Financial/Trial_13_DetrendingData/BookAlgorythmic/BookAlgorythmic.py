#https://scipy-lectures.org/intro/scipy/auto_examples/plot_detrend.html
import numpy as np
t = np.linspace(0, 5, 100)
x = t + np.random.normal(size=100)

from scipy import signal
x_detrended = signal.detrend(x)

from matplotlib import pyplot as plt
plt.figure(figsize=(5, 4))
plt.plot(t, x, label="x")
plt.plot(t, x_detrended, label="x_detrended")
plt.legend(loc='best')
#plt.show()

#trend
import numpy as np
from matplotlib import pyplot as plt
from scipy import signal

n = 150
t = np.linspace(0, 10, n)
x_raw = .4 * t + np.random.normal(size=n)

x = signal.detrend(x_raw)
d = x_raw - x

is_positive_trend = d[-1] > d[0]
m = "+" if is_positive_trend else "-"

plt.figure(figsize=(5, 4))
plt.plot(t, x_raw, label=f"Raw data ({m})")
plt.plot(t, d)
plt.legend(loc='best')
plt.show()