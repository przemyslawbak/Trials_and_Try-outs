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

#https://www.tradingview.com/script/RlRxtEuQ-Sentiment-Histogram/
print('reading inputs...')
period = 14
mode = "Fast"
ma_type = "WMA"

length = math.ceil(period/4)

BarHigh = ma(ma_type,high, length)
BarLow = ma(ma_type,low,  length)
BarOpen = ma(ma_type,open, length)
BarClose = ma(ma_type,close,length)
Bar_Range = BarHigh - BarLow

#https://kodify.net/tradingview/bar-data/highest-high-lowest-low/
Group_High = highest(high, period)
Group_Low = lowest(low, period)
Group_Open = open[period - 1]
Group_Range = Group_High - Group_Low

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

def ma(typ, src, len):
    result = 0
    #more to be chosen in the link
    if typ=="WMA":
        result = wma(src, len)
    return result

#https://www.askpython.com/python/weighted-moving-average
def wma(Data, period):
    weighted = []
    for i in range(len(Data)):
            try:
                total = numpy.arange(1, period + 1, 1)
                matrix = Data[i - period + 1: i + 1, 3:4]
                matrix = numpy.ndarray.flatten(matrix)
                matrix = total * matrix
                wma = (matrix.sum()) / (total.sum()) # WMA
                weighted = numpy.append(weighted, wma) 
            except ValueError:
                pass
    return weighted

#https://www.alpharithms.com/calculating-moving-averages-in-python-585117/
def sma(Data, period):
    return Data.rolling(period).mean()

def highest(Data, period):
    list = Data.rolling(period).sort()
    return list1[-1]

def lowest(Data, period):
    list = Data.rolling(period).sort()
    return list1[0]