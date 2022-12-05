import warnings
warnings.filterwarnings('ignore')
import numpy as np
import pandas as pd
import pandas_datareader.data as web
import pymc3 as pm
import arviz
from scipy import stats
import seaborn as sns
import matplotlib.pyplot as plt
from matplotlib import gridspec

benchmark = web.DataReader('SP500', data_source='fred', start=2010)
benchmark.columns = ['benchmark']

with pd.HDFStore('../../assets.h5') as store:
    stock = store['quandl/wiki/prices'].adj_close.unstack()['AMZN'].to_frame('stock') #exception!

data = stock.join(benchmark).pct_change().dropna().loc['2010':]
data.info()