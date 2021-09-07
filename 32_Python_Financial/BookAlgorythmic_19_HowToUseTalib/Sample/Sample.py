import warnings
warnings.filterwarnings('ignore')

import os
from datetime import datetime
import pandas as pd
import pandas_datareader.data as web
import matplotlib.pyplot as plt
import mplfinance as mpf
import seaborn as sns

#The download of the content of one or more html tables works as follows, for instance for the constituents of the S&P500 index from Wikipedia
sp_url = 'https://en.wikipedia.org/wiki/List_of_S%26P_500_companies'
sp500_constituents = pd.read_html(sp_url, header=0)[0]
sp500_constituents.info()

#pandas used to facilitate access to data provider APIs directly, but this functionality has moved to the pandas-datareader
#using Yahoo finance API
start = '2014'
end = datetime(2017, 5, 24)
yahoo= web.DataReader('FB', 'yahoo', start=start, end=end)
yahoo.info()

#drawing plot
#mpf.plot(yahoo.drop('Adj Close', axis=1), type='candle') #commented
#plt.tight_layout() #commented

#IEX api
#IEX_API_KEY = 'xxx'
#start = datetime(2015, 2, 9)
# end = datetime(2017, 5, 24)
#iex = web.DataReader('FB', 'iex', start, api_key=IEX_API_KEY)
#iex.info()

book = web.get_iex_book('AAPL')
list(book.keys())
orders = pd.concat([pd.DataFrame(book[side]).assign(side=side) for side in ['bids', 'asks']])
orders.head()
for key in book.keys():
    try:
        print(f'\n{key}')
        print(pd.DataFrame(book[key]))
    except:
        print(book[key])

"""
OTHER:
Quandl
FRED
Fama/French
World Bank
OECD
Tiingo
"""

#stooq
print('stooq')
index_url = 'https://stooq.com/t/'
ix = pd.read_html(index_url)
len(ix)
sp500_stooq = web.DataReader('^SPX', 'stooq')
sp500_stooq.info()
sp500_stooq.head()
sp500_stooq.Close.plot(figsize=(14,4))
sns.despine()
plt.tight_layout()