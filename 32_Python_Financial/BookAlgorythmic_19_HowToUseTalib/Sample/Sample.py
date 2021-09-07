import warnings
warnings.filterwarnings('ignore')
import matplotlib.pyplot as plt
import seaborn as sns
import pandas as pd
from talib import RSI, BBANDS, MACD

sns.set_style('whitegrid')
idx = pd.IndexSlice
DATA_STORE = 'data/assets.h5'
START = 2007
END = 2010

df = pd.read_csv('wiki_prices.csv')
with pd.HDFStore(DATA_STORE) as store:
    store.put('quandl/wiki/prices', df)

ohlcv = ['adj_open', 'adj_close', 'adj_low', 'adj_high', 'adj_volume', 'ticker']
with pd.HDFStore(DATA_STORE) as store:
    data = (store['quandl/wiki/prices']
            .loc[idx[START:END], ohlcv]
            .unstack('ticker') #exception...
            .swaplevel(axis=1)
            .loc[:, 'AAPL']
            .rename(columns=lambda x: x.replace('adj_', '')))

data.info()