from pathlib import Path
import numpy as np
import pandas as pd
from scipy.stats import spearmanr

pd.set_option('display.expand_frame_repr', False)
np.random.seed(42)
DATA = 'data/assets.h5'
ASSETS = 'data/data.h5'
def get_backtest_data(predictions='lasso/predictions'):
    """Combine chapter 7 lr/lasso/ridge regression predictions
        with adjusted OHLCV Quandl Wiki data"""
    with pd.HDFStore(DATA) as store:
        prices = (store['quandl/wiki/prices']
                  .filter(like='adj')
                  .rename(columns=lambda x: x.replace('adj_', ''))
                  )
    with pd.HDFStore(ASSETS) as store:
        print(store.info())
        predictions = store[predictions] #exception: No object named lasso/predictions in the file

    best_alpha = predictions.groupby('alpha').apply(lambda x: spearmanr(x.actuals, x.predicted)[0]).idxmax()
    predictions = predictions[predictions.alpha == best_alpha]
    predictions.index.names = ['ticker', 'date']
    tickers = predictions.index.get_level_values('ticker').unique()
    start = predictions.index.get_level_values('date').min().strftime('%Y-%m-%d')
    stop = (predictions.index.get_level_values('date').max() + pd.DateOffset(1)).strftime('%Y-%m-%d')
    idx = pd.IndexSlice
    prices = prices.sort_index().loc[idx[tickers, start:stop], :]
    predictions = predictions.loc[predictions.alpha == best_alpha, ['predicted']]
    return predictions.join(prices, how='right')


df = get_backtest_data('lasso/predictions')
print(df.info())
df.to_hdf('backtest.h5', 'data')