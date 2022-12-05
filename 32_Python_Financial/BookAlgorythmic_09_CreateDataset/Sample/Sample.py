import warnings
warnings.filterwarnings('ignore')
from pathlib import Path
import requests
from io import BytesIO
from zipfile import ZipFile, BadZipFile
import numpy as np
import pandas as pd
import pandas_datareader.data as web
from sklearn.datasets import fetch_openml
pd.set_option('display.expand_frame_repr', False)

DATA_STORE = Path('data/assets.h5')

#Quandl
print('Quandl')
df = pd.read_csv('sources/wiki_prices.csv')
# no longer needed
# df = pd.concat([df.loc[:, 'code'].str.strip(),
#                 df.loc[:, 'name'].str.split('(', expand=True)[0].str.strip().to_frame('name')], axis=1)

print(df.info(null_counts=True))
with pd.HDFStore(DATA_STORE) as store:
    store.put('quandl/wiki/prices', df)

#Quandl
print('Quandl')
df = pd.read_csv('sources/wiki_stocks.csv')
# no longer needed
# df = pd.concat([df.loc[:, 'code'].str.strip(),
#                 df.loc[:, 'name'].str.split('(', expand=True)[0].str.strip().to_frame('name')], axis=1)

print(df.info(null_counts=True))
with pd.HDFStore(DATA_STORE) as store:
    store.put('quandl/wiki/stocks', df)

#FRED
print('FRED')
df = pd.read_csv('sources/fredSP500.csv')
# no longer needed
# df = pd.concat([df.loc[:, 'code'].str.strip(),
#                 df.loc[:, 'name'].str.split('(', expand=True)[0].str.strip().to_frame('name')], axis=1)

print(df.info(null_counts=True))
with pd.HDFStore(DATA_STORE) as store:
    store.put('sp500/fred', df)

#Stooq
print('Stooq')
df = pd.read_csv('sources/stooq^spx_d.csv')
# no longer needed
# df = pd.concat([df.loc[:, 'code'].str.strip(),
#                 df.loc[:, 'name'].str.split('(', expand=True)[0].str.strip().to_frame('name')], axis=1)

print(df.info(null_counts=True))
with pd.HDFStore(DATA_STORE) as store:
    store.put('sp500/stooq', df)

#Wikipedia
print('Wikipedia')
url = 'https://en.wikipedia.org/wiki/List_of_S%26P_500_companies'
df = pd.read_html(url, header=0)[0]
df.head()
df.columns = ['ticker', 'name', 'sec_filings', 'gics_sector', 'gics_sub_industry',
              'location', 'first_added', 'cik', 'founded']
df = df.drop('sec_filings', axis=1).set_index('ticker')
print(df.info())
with pd.HDFStore(DATA_STORE) as store:
    store.put('sp500/stocks', df)

#US equities
print('US equities')
df = pd.read_csv('sources/us_equities_meta_data.csv')
df.info()
with pd.HDFStore(DATA_STORE) as store:
    store.put('us_equities/stocks', df.set_index('ticker'))

###################################################################################
