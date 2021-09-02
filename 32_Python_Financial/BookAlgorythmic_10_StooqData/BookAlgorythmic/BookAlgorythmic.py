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

DATA_STORE = Path('data', 'assets.h5')
STOOQ_URL = 'https://static.stooq.com/db/h/'

stooq_path = Path('stooq') 
if not stooq_path.exists():
    stooq_path.mkdir()

def download_price_data(market='us'):
    data_url = f'd_{market}_txt.zip'
    response = requests.get(STOOQ_URL + data_url).content
    with ZipFile(BytesIO(response)) as zip_file:
        for i, file in enumerate(zip_file.namelist()):
            if not file.endswith('.txt'):
                continue
            local_file = stooq_path / file
            local_file.parent.mkdir(parents=True, exist_ok=True)
            try:
                with local_file.tryopen('wb') as output:
                    for line in zip_file.open(file).readlines():
                        output.write(line)
            except:
                print("File not found...") 

for market in ['us', 'jp']:
    download_price_data(market=market)

#Add symbols
df = pd.read_csv('https://stooq.com/db/l/?g=69', sep='        ').apply(lambda x: x.str.strip())
df.columns = ['ticker', 'name']
df.drop_duplicates('ticker').to_csv('stooq/data/tickers/us/nasdaq etfs.csv', index=False)

metadata_dict = {
    ('jp', 'tse etfs'): 34,
    ('jp', 'tse stocks'): 32,
    ('us', 'nasdaq etfs'): 69,
    ('us', 'nasdaq stocks'): 27,
    ('us', 'nyse etfs'): 70,
    ('us', 'nyse stocks'): 28,
    ('us', 'nysemkt stocks'): 26
}

for (market, asset_class), code in metadata_dict.items():
    df = pd.read_csv(f'https://stooq.com/db/l/?g={code}', sep='        ').apply(lambda x: x.str.strip())
    df.columns = ['ticker', 'name']
    df = df.drop_duplicates('ticker').dropna()
    print(market, asset_class, f'# tickers: {df.shape[0]:,.0f}')
    path = stooq_path / 'tickers' / market
    if not path.exists():
        path.mkdir(parents=True)
    df.to_csv(path / f'{asset_class}.csv', index=False)  

def get_stooq_prices_and_tickers(frequency='daily',
                                 market='us',
                                 asset_class='nasdaq etfs'):
    prices = []
    
    tickers = (pd.read_csv(stooq_path / 'tickers' / market / f'{asset_class}.csv'))

    if frequency in ['5 min', 'hourly']:
        parse_dates = [['date', 'time']]
        date_label = 'date_time'
    else:
        parse_dates = ['date']
        date_label = 'date'
    names = ['ticker', 'freq', 'date', 'time', 
             'open', 'high', 'low', 'close','volume', 'openint']
    
    usecols = ['ticker', 'open', 'high', 'low', 'close', 'volume'] + parse_dates
    path = stooq_path / 'data' / frequency / market / asset_class
    print(path.as_posix())
    files = path.glob('**/*.txt')
    for i, file in enumerate(files, 1):
        if i % 500 == 0:
            print(i)
        if file.stem not in set(tickers.ticker.str.lower()):
            print(file.stem, 'not available')
            file.unlink()
        else:
            try:
                df = (pd.read_csv(
                    file,
                    names=names,
                    usecols=usecols,
                    header=0,
                    parse_dates=parse_dates))
                prices.append(df)
            except pd.errors.EmptyDataError:
                print('\tdata missing', file.stem)
                file.unlink()

    prices = (pd.concat(prices, ignore_index=True) #BUG
              .rename(columns=str.lower)
              .set_index(['ticker', date_label])
              .apply(lambda x: pd.to_numeric(x, errors='coerce')))
    return prices, tickers

# load some Japanese and all US assets for 2000-2019
markets = {'jp': ['tse stocks'],
           'us': ['nasdaq etfs', 'nasdaq stocks', 'nyse etfs', 'nyse stocks', 'nysemkt stocks']
          }
frequency = 'daily'

idx = pd.IndexSlice
for market, asset_classes in markets.items():
    for asset_class in asset_classes:
        print(f'\n{asset_class}')
        prices, tickers = get_stooq_prices_and_tickers(frequency=frequency, 
                                                       market=market, 
                                                       asset_class=asset_class)
        
        prices = prices.sort_index().loc[idx[:, '2000': '2019'], :]
        names = prices.index.names
        prices = (prices
                  .reset_index()
                  .drop_duplicates()
                  .set_index(names)
                  .sort_index())
        
        print('\nNo. of observations per asset')
        print(prices.groupby('ticker').size().describe())
        key = f'stooq/{market}/{asset_class.replace(" ", "/")}/'
        
        print(prices.info(null_counts=True))
        
        prices.to_hdf(DATA_STORE, key + 'prices', format='t')
        
        print(tickers.info())
        tickers.to_hdf(DATA_STORE, key + 'tickers', format='t')