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
mpf.plot(yahoo.drop('Adj Close', axis=1), type='candle')
plt.tight_layout()

