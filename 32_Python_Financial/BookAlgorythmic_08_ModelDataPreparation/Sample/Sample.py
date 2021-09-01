import warnings
warnings.filterwarnings('ignore')
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns
from scipy.stats import pearsonr, spearmanr
from talib import RSI, BBANDS, MACD, ATR

#settings
MONTH = 21
YEAR = 12 * MONTH
START = 2000
END = 2018
sns.set_style('whitegrid')
idx = pd.IndexSlice
print('fetching data...')
#Loading Quandl Wiki Stock Prices & Meta Data
ohlcv = ['adj_open', 'adj_close', 'adj_low', 'adj_high', 'adj_volume']
DATA_STORE = 'data/assets.h5'
print('next...')
df = pd.read_csv('wiki_prices.csv')
# no longer needed
# df = pd.concat([df.loc[:, 'code'].str.strip(),
#                 df.loc[:, 'name'].str.split('(', expand=True)[0].str.strip().to_frame('name')], axis=1)

print(df.info(null_counts=True))
with pd.HDFStore(DATA_STORE) as store:
    store.put('quandl/wiki/prices', df)
df = pd.read_csv('us_equities_meta_data.csv')
df.info()
with pd.HDFStore(DATA_STORE) as store:
    store.put('us_equities/stocks', df.set_index('ticker'))
with pd.HDFStore(DATA_STORE) as store:
    prices = (store['quandl/wiki/prices']
              .loc[idx[START:END], ohlcv]
              .rename(columns=lambda x: x.replace('adj_', ''))
              .assign(volume=lambda x: x.volume.div(1000))
              .sort_index())
    stocks = (store['us_equities/stocks']
              .loc[:, ['marketcap', 'ipoyear', 'sector']])

print('ok...')
print(prices)
print(stocks)

# want at least 2 years of data
min_obs = 2 * YEAR

# have this much per ticker 
nobs = prices.groupby(level='ticker').size()

# keep those that exceed the limit
keep = nobs[nobs > min_obs].index

prices = prices.loc[idx[keep, :], :]