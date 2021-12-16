import os
import datetime
import IPython
import IPython.display
import matplotlib as mpl
import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
import seaborn as sns
import tensorflow as tf
import math
import mplfinance as mpf
from mplfinance.original_flavor import candlestick_ohlc #fixed

print('loading data...')
mpl.rcParams['figure.figsize'] = (8, 6)
mpl.rcParams['axes.grid'] = False

#path to the data file
csv_path = 'GPW_DLY WIG20, 15.csv'
df = pd.read_csv(csv_path)

def ma(typ, src, len): #todo: test other types
    result = 0
    #more to be chosen in the link
    if typ=="SMA":
        result = sma(src, len)
    return result

def sma(Data, period):
    return Data.rolling(period).mean()

def highest(Data, period):
    list = Data.rolling(period)
    res = list.max()
    return res

def lowest(Data, period):
    list = Data.rolling(period)
    res = list.min()
    return res

#https://www.tradingview.com/script/RlRxtEuQ-Sentiment-Histogram/
print('reading inputs...')
period = 14
mode = "Fast"
ma_type = "SMA"

length = math.ceil(period/4)

df["BarHigh"] = ma(ma_type, df['high'], length)
df["BarLow"] = ma(ma_type, df['low'], length)
df["BarClose"] = ma(ma_type, df['close'], length)
df["BarOpen"] = ma(ma_type, df['open'], length)
df["Bar_Range"] = df["BarHigh"] - df["BarLow"]

df["Group_High"] = highest(df['high'], period)
df["Group_Low"] = lowest(df['low'], period)
df["Group_Open"] = df['open'][period - 1]
df["Group_Range"] = df["Group_High"] - df["Group_Low"]

df.loc[df['Bar_Range'] == 0, 'Bar_Range'] = 1.0
df.loc[df['Group_Range'] == 0, 'Group_Range'] = 1.0

df["BarBull"] = (((df["BarClose"] - df["BarLow"]) + (df["BarHigh"] - df["BarOpen"]))/2)
df["BarBear"]= (((df["BarHigh"] - df["BarClose"]) + (df["BarOpen"] - df["BarLow"]))/2)

calcBull = df["BarBull"]
calcBear = df["BarBear"]

df["Bull"] = sma(calcBull, period)
df["Bear"] = sma(calcBear, period)
df["diff"] = df["Bull"] - df["Bear"]
df['time'] = pd.to_datetime(df['time']).dt.tz_localize(None)
df.index = pd.DatetimeIndex(df['time'])

pd.set_option('display.max_rows', None)
print(df)

#todo: data display

apdict = mpf.make_addplot(df['diff'], panel=1, color='blue')
mpf.plot(df, type='candle', addplot=apdict, main_panel=0, title="WIG20 15M")