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

DATA_STORE = Path('assets.h5')

#Quandl
df = pd.read_csv('sources/wiki_stocks.csv')
# no longer needed
# df = pd.concat([df.loc[:, 'code'].str.strip(),
#                 df.loc[:, 'name'].str.split('(', expand=True)[0].str.strip().to_frame('name')], axis=1)

print(df.info(null_counts=True))
with pd.HDFStore(DATA_STORE) as store:
    store.put('quandl/', df)

#FRED
df = pd.read_csv('sources/fredSP500.csv')
# no longer needed
# df = pd.concat([df.loc[:, 'code'].str.strip(),
#                 df.loc[:, 'name'].str.split('(', expand=True)[0].str.strip().to_frame('name')], axis=1)

print(df.info(null_counts=True))
with pd.HDFStore(DATA_STORE) as store:
    store.put('fred/', df)

#Stooq
df = pd.read_csv('sources/stooq^spx_d.csv')
# no longer needed
# df = pd.concat([df.loc[:, 'code'].str.strip(),
#                 df.loc[:, 'name'].str.split('(', expand=True)[0].str.strip().to_frame('name')], axis=1)

print(df.info(null_counts=True))
with pd.HDFStore(DATA_STORE) as store:
    store.put('stooq/', df)