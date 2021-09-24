import matplotlib.pyplot as plt
from mplfinance.original_flavor import candlestick_ohlc #fixed
import pandas as pd
import matplotlib.dates as mpl_dates

plt.style.use('ggplot')

#https://saralgyaan.com/posts/python-candlestick-chart-matplotlib-tutorial-chapter-11/
# Load a numpy structured array from yahoo csv data with fields date, open,
# close, volume, adj_close from the mpl-data/example directory.  This array
# stores the date as an np.datetime64 with a day unit ('D') in the 'date'
# column.
#data1 = cbook.get_sample_data('goog.npz', np_load=True)['price_data']
data = pd.read_csv('../../data/wig20_d.csv')

ohlc = data.loc[:, ['Date', 'Open', 'High', 'Low', 'Close']]
ohlc['Date'] = pd.to_datetime(ohlc['Date'])
ohlc['Date'] = ohlc['Date'].apply(mpl_dates.date2num)
ohlc = ohlc.astype(float)

# Creating Subplots
fig, ax = plt.subplots()

candlestick_ohlc(ax, ohlc.values, width=0.6, colorup='green', colordown='red', alpha=0.8)

# Setting labels & titles
ax.set_xlabel('Date')
ax.set_ylabel('Price')
fig.suptitle('Daily Candlestick Chart of anything')

# Formatting Date
date_format = mpl_dates.DateFormatter('%d-%m-%Y')
ax.xaxis.set_major_formatter(date_format)
fig.autofmt_xdate()

fig.tight_layout()

plt.show()