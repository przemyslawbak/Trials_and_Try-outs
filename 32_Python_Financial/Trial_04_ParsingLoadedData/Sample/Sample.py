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