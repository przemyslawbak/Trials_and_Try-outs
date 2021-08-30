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

#Quandl makes available a dataset with stock prices
df = pd.read_csv('wiki_stocks.csv')
# no longer needed
# df = pd.concat([df.loc[:, 'code'].str.strip(),
#                 df.loc[:, 'name'].str.split('(', expand=True)[0].str.strip().to_frame('name')], axis=1)

print(df.info(null_counts=True))
with pd.HDFStore(DATA_STORE) as store:
    store.put('quandl/wiki/prices', df)


#Set data store location
START = 2000
END = 2018
with pd.HDFStore(DATA_STORE) as store:
    prices = (store['quandl/wiki/prices']
              .loc[idx[str(START):str(END), :], 'adj_close']
              .unstack('ticker'))
    stocks = store['us_equities/stocks'].loc[:, ['marketcap', 'ipoyear', 'sector']]

prices.info()