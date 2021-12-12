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
period  = 14
mode    = "Fast"
ma_type = "WMA"

def ma(typ, src, len):
    result = 0
    #more to be chosen in the link
    if typ=="WMA":
        result = wma(src, len)
    return result

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

length = math.ceil(period/4)
