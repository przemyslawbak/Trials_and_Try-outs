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
df["Bar_Range"] = df["BarHigh"] - df["BarLow"]

df["Group_High"] = highest(df['high'], period)
df["Group_Low"] = lowest(df['low'], period)
df["Group_Open"] = df['open'][period - 1]
df["Group_Range"] = df["Group_High"] - df["Group_Low"]

pd.set_option('display.max_rows', None)
pd.set_option('display.max_columns', 500)
pd.set_option('display.width', 1000)
print(df)

if Bar_Range == 0.0:
    Bar_Range = 1.0
    
if Group_Range == 0.0:
    Group_Range = 1.0

BarBull = (((BarClose - BarLow) + (BarHigh - BarOpen))/2)
BarBear = (((BarHigh - BarClose) + (BarOpen - BarLow))/2) 

GroupBull = (((BarClose - Group_Low) + (Group_High - Group_Open)) / 2)
GroupBear = (((Group_High - BarClose) + (Group_Open - Group_Low)) / 2)

calcBull = (BarBull + GroupBull) / 2
calcBear = (BarBear + GroupBear) / 2

Bull = sma(calcBull, period)
Bear = sma(calcBear, period)
diff = Bull - Bear

color = ''
if (Bull >= Bull[1] and Bull > Bear):
    color = 'teal'
if (Bull <  Bull[1] and Bull > Bear):
    color = 'lime'
if (Bear >= Bear[1] and Bear > Bull):
    color = 'maroon'
if (Bear <  Bear[1] and Bear > Bull):
    color = 'red'

#todo
plot(diff,"Sentiment",_color,2,plot.style_columns)