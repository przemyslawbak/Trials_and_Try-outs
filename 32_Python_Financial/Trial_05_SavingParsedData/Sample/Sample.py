import matplotlib.pyplot as plt
from mplfinance.original_flavor import candlestick_ohlc #fixed
import pandas as pd
import matplotlib.dates as mpl_dates
import pandas_datareader.stooq
import requests
import re
from bs4 import BeautifulSoup as bs
from io import StringIO

new_df = pd.DataFrame()
new_df_fromfile = pd.DataFrame()
df_all = pd.DataFrame()
df = pandas_datareader.stooq.StooqDailyReader('pge.pl')
df = df.read()
#print(df.head())
FILE_NAME = 'GPW_DLY CDR, 15.csv'
stooq_sample = ('https://stooq.com/q/a2/d/?s=cdr&i=15')
reqst_sample = requests.get(stooq_sample)
st = reqst_sample.content
splitted = st.decode().split("~~")[1]
df = pd.read_csv(StringIO(splitted))

new_df['time'] = (df['Date'].astype(str) + df['Time'].astype(str).str[:-2])
new_df['time'] = pd.to_datetime(new_df['time'], format='%Y%m%d%H%M').dt.tz_localize(None)
new_df = pd.merge(new_df, df['Open'], left_index=True, right_index=True)
new_df = pd.merge(new_df, df['High'], left_index=True, right_index=True)
new_df = pd.merge(new_df, df['Low'], left_index=True, right_index=True)
new_df = pd.merge(new_df, df['Close'], left_index=True, right_index=True)
new_df = pd.merge(new_df, df['Vol'], left_index=True, right_index=True)
new_df = new_df.rename(columns={'Open': 'open', 'High': 'high', 'Low': 'low', 'Close': 'close', 'Vol': 'Volume'})
file_df = pd.read_csv(FILE_NAME)
new_df_fromfile['time'] = pd.to_datetime(file_df['time']).dt.tz_localize(None)
new_df_fromfile = pd.merge(new_df_fromfile, file_df['open'], left_index=True, right_index=True)
new_df_fromfile = pd.merge(new_df_fromfile, file_df['high'], left_index=True, right_index=True)
new_df_fromfile = pd.merge(new_df_fromfile, file_df['low'], left_index=True, right_index=True)
new_df_fromfile = pd.merge(new_df_fromfile, file_df['close'], left_index=True, right_index=True)
new_df_fromfile = pd.merge(new_df_fromfile, file_df['Volume'], left_index=True, right_index=True)

df_all = pd.concat([new_df_fromfile, new_df]).fillna(0).drop_duplicates(subset=['time'], keep='first')

print(new_df)
print(new_df_fromfile)
print(df_all)

df_all.to_csv(FILE_NAME, sep=',', encoding='utf-8')