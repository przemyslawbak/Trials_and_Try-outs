import warnings
warnings.filterwarnings('ignore')
from datetime import datetime
import pandas as pd
import pandas_datareader.data as web
# replaces pyfinance.ols.PandasRollingOLS (no longer maintained)
from statsmodels.regression.rolling import RollingOLS
import statsmodels.api as sm
import matplotlib.pyplot as plt
import seaborn as sns
from pathlib import Path
import requests
from io import BytesIO
from zipfile import ZipFile, BadZipFile
import numpy as np
from sklearn.datasets import fetch_openml

#settings
sns.set_style('whitegrid')
idx = pd.IndexSlice
pd.set_option('display.expand_frame_repr', False)

DATA_STORE = 'data/assets.h5'

if not Path('data', 'assets.h5').exists():
    print('fetching data...')
    #Quandl makes available a dataset with stock prices
    df = pd.read_csv('wiki_prices.csv')
    # no longer needed
    # df = pd.concat([df.loc[:, 'code'].str.strip(),
    #                 df.loc[:, 'name'].str.split('(', expand=True)[0].str.strip().to_frame('name')], axis=1)
    print(df.info(null_counts=True))
    with pd.HDFStore(DATA_STORE) as store:
        store.put('quandl/wiki/prices', df)
else:
    print('data exists...')

#Set data store location
START = 2000
END = 2018
with pd.HDFStore(DATA_STORE) as store:
    prices = (store['quandl/wiki/prices']
              .loc[idx[str(START):str(END), :], 'adj_close']
              .unstack('ticker'))
    stocks = store['us_equities/stocks'].loc[:, ['marketcap', 'ipoyear', 'sector']]

prices.info()

#Remove `stocks` duplicates and align index names for later joining
stocks = stocks[~stocks.index.duplicated()]
stocks.index.name = 'ticker'

#Get tickers with both price information and metdata
shared = prices.columns.intersection(stocks.index)
stocks = stocks.loc[shared, :]
stocks.info()
prices = prices.loc[:, shared]
prices.info()
assert prices.shape[1] == stocks.shape[0]

#Create monthly return series
monthly_prices = prices.resample('M').last()
monthly_prices.info()
outlier_cutoff = 0.01
data = pd.DataFrame()
lags = [1, 2, 3, 6, 9, 12]
for lag in lags:
    data[f'return_{lag}m'] = (monthly_prices
                           .pct_change(lag)
                           .stack()
                           .pipe(lambda x: x.clip(lower=x.quantile(outlier_cutoff),
                                                  upper=x.quantile(1-outlier_cutoff)))
                           .add(1)
                           .pow(1/lag)
                           .sub(1)
                           )
data = data.swaplevel().dropna()
data.info()

#Drop stocks with less than 10 yrs of returns
min_obs = 120
nobs = data.groupby(level='ticker').size()
keep = nobs[nobs>min_obs].index
data = data.loc[idx[keep,:], :]
data.info()
data.describe()

# cmap = sns.diverging_palette(10, 220, as_cmap=True)
sns.clustermap(data.corr('spearman'), annot=True, center=0, cmap='Blues');
