import matplotlib.pyplot as plt
from mplfinance.original_flavor import candlestick_ohlc #fixed
import pandas as pd
import matplotlib.dates as mpl_dates
import pandas_datareader.stooq
import requests
import re
from bs4 import BeautifulSoup as bs
from io import StringIO

df = pandas_datareader.stooq.StooqDailyReader('pge.pl')
df = df.read()
#print(df.head())

stooq_sample = ('https://stooq.com/q/a2/d/?s=cdr&i=15')
reqst_sample = requests.get(stooq_sample)
st = reqst_sample.content
splitted = st.decode().split("~~")[1]
df = pd.read_csv(StringIO(splitted))

new_df = pd.DataFrame()
new_df['time'] = (df['Date'].astype(str) + df['Time'].astype(str).str[:-2])
new_df['time'] = pd.to_datetime(new_df['time'], format='%Y%m%d%H%M')
new_df = pd.merge(new_df, df['Open'], left_index=True, right_index=True)
new_df = pd.merge(new_df, df['High'], left_index=True, right_index=True)
new_df = pd.merge(new_df, df['Low'], left_index=True, right_index=True)
new_df = pd.merge(new_df, df['Close'], left_index=True, right_index=True)
new_df = pd.merge(new_df, df['Vol'], left_index=True, right_index=True)
print(new_df)
file_df = pd.read_csv('GPW_DLY CDR, 15.csv')
print(df.dtypes)
print(file_df.dtypes)


plt.style.use('ggplot')
START = 2019
END = 2021
idx = pd.IndexSlice

#https://saralgyaan.com/posts/python-candlestick-chart-matplotlib-tutorial-chapter-11/
# Load a numpy structured array from yahoo csv data with fields date, open,
# close, volume, adj_close from the mpl-data/example directory.  This array
# stores the date as an np.datetime64 with a day unit ('D') in the 'date'
# column.
#data1 = cbook.get_sample_data('goog.npz', np_load=True)['price_data']
data = pd.read_csv('../../data/wig20_d.csv')
data.set_index('Date', inplace=True, drop=False)

#check for new data, load new data here if any and save it to the file

ohlc = data.loc['2017-09-06':, ['Date', 'Open', 'High', 'Low', 'Close']]
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