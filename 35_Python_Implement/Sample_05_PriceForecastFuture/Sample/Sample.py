import pandas as pd
from statsmodels.graphics.tsaplots import plot_acf, plot_pacf
import matplotlib.pyplot as plt
from statsmodels.tsa.arima_model import ARIMA

#https://www.machinelearningplus.com/time-series/arima-model-time-series-forecasting-python/
plt.rcParams.update({'figure.figsize':(9,3), 'figure.dpi':120})
# Import data
df = pd.read_csv('GPW_DLY WIG20, 15.csv', usecols=["close"])

fig, axes = plt.subplots(1, 2, sharex=True)
axes[0].plot(df.close.diff()); axes[0].set_title('1st Differencing')
axes[1].set(ylim=(0,1.2))
plot_acf(df.close.diff().dropna(), ax=axes[1])

# Create Training and Test
train = df.close[:20000]
test = df.close[20000:]

# Build Model
model = ARIMA(train, order=(3, 2, 1))  
fitted = model.fit(disp=-1)  
print(fitted.summary())

# Forecast
fc, se, conf = fitted.forecast(194, alpha=0.05)  # 95% conf

# Make as pandas series
fc_series = pd.Series(fc, index=test.index)
lower_series = pd.Series(conf[:, 0], index=test.index)
upper_series = pd.Series(conf[:, 1], index=test.index)

# Plot
plt.figure(figsize=(12,5), dpi=100)
plt.plot(train, label='training')
plt.plot(test, label='actual')
plt.plot(fc_series, label='forecast')
plt.fill_between(lower_series.index, lower_series, upper_series, 
                 color='k', alpha=.15)
plt.title('Forecast vs Actuals')
plt.legend(loc='upper left', fontsize=8)

plt.show()