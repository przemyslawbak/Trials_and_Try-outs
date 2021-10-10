import warnings
warnings.filterwarnings('ignore')
import pandas as pd
import pandas_datareader.data as web
import numpy as np

import matplotlib.pyplot as plt
import matplotlib.transforms as mtransforms
import seaborn as sns

from statsmodels.tsa.api import AR
from statsmodels.tsa.stattools import acf, q_stat, adfuller
from statsmodels.graphics.tsaplots import plot_acf, plot_pacf
from sklearn.preprocessing import minmax_scale
from scipy.stats import probplot, moment
from sklearn.metrics import mean_absolute_error

file = 'GPW_DLY CDR, 15.csv'

sent = 'UMCSENT'
df = web.DataReader(['UMCSENT', 'IPGMFN'], 'fred', '1970', '2019-12').dropna()
print(df)

df.columns = ['sentiment', 'ip']

cdr = pd.read_csv(file, index_col=["time"], usecols=["time", "close"])
df_transformed = pd.DataFrame({'close': np.log(cdr.close).diff(12)}).dropna()
df_transformed = df_transformed.apply(minmax_scale)
model = AR(df_transformed)
res = model.fit()
print(res.summary())
n =len(df_transformed)
start = n-24
preds = res.predict(start=start+1, end=n)
preds.index = df_transformed.index[start:]
fig, axes = plt.subplots(nrows=2, figsize=(14, 8), sharex=True)
df_transformed.close.loc[:].plot(ax=axes[0], label='actual', title='CDR')
preds.close.plot(label='predicted', ax=axes[0])
trans = mtransforms.blended_transform_factory(axes[0].transData, axes[0].transAxes)
axes[0].legend()
axes[0].fill_between(x=df_transformed.index[start+1:], y1=0, y2=1, transform=trans, color='grey', alpha=.5)
axes[1].set_xlabel('')
sns.despine()
fig.tight_layout();






plt.show()