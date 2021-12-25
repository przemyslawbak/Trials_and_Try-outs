import pandas as pd
import matplotlib.pyplot as plt
from prophet import Prophet

df = pd.read_csv('co2-ppm-daily_csv.csv')
df['date'] = pd.to_datetime(df['date'])
df.columns = ['ds', 'y']

model = Prophet()
model.fit(df)

future = model.make_future_dataframe(periods=365 * 10)
forecast = model.predict(future)
fig = model.plot(forecast, xlabel='Date', ylabel=r'CO$_2$ PPM')

plt.title('Daily Carbon Dioxide Levels Measured at Mauna Loa')
plt.show()