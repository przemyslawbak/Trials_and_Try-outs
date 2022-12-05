import pandas as pd
from statsmodels.graphics.tsaplots import plot_acf, plot_pacf
import matplotlib.pyplot as plt
from statsmodels.tsa.arima_model import ARIMA
import numpy as np

#https://www.machinelearningplus.com/time-series/arima-model-time-series-forecasting-python/
# Import data
df = pd.read_csv('GPW_DLY WIG20, 15.csv', usecols=["close"])
n_periods = 2000

# Create Training and Test
train = df.close[:]

# Build Model
model = ARIMA(train, order=(3, 2, 1))  
fitted = model.fit(disp=-1)  
print(fitted.summary())

# Forecast
fc = fitted.predict(n_periods)
index_of_fc = np.arange(len(df.close), len(df.close)+n_periods)

# make series for plotting purpose
fc_series = pd.Series(fc, index=index_of_fc)

# Plot
plt.plot(fc_series, color='darkgreen')

plt.title("Final Forecast of WWW Usage")
plt.show()