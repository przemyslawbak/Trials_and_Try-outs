#https://stackoverflow.com/a/61128030/12603542

#trend
from scipy import signal
import pandas as pd

pd.set_option('display.max_rows', 10000)
pd.set_option('display.max_columns', 10)
pd.set_option('display.width', 1000)

period = 250
wig20_d = pd.read_csv('../../data/wig20_d_1.csv')

def getTrendStartRow(rowNo):
    if rowNo > period:
        return rowNo - period
    return 0

def getTrend(row):
    i = row.name + 1
    start = getTrendStartRow(i)
    stop = i
    x_range = wig20_d.iloc[start:stop]
    x_raw = x_range['Zamkniecie'].to_numpy()
    x = signal.detrend(x_raw)
    d = x_raw - x
    is_positive_trend = d[-1] > d[0]
    m = 1 if is_positive_trend else 0
    return m
    
wig20_d['trend'] = wig20_d.apply(getTrend, axis=1)

print(wig20_d.tail(10000))