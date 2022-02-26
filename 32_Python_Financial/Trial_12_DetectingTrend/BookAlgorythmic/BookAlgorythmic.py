#https://trendet.readthedocs.io/

import warnings
warnings.filterwarnings('ignore')
import pandas as pd
import trendet
import matplotlib.pyplot as plt
import seaborn as sns

START = '2020-01-01'
DATA_STORE = 'data/assets.h5'
pd.set_option('display.max_rows', 1000)

wig20_d = pd.read_csv('../../data/wig20_d_1.csv')
wig20_d.rename(columns={'Data': 'Date'}, inplace=True)
wig20_d['Date'] = pd.to_datetime(wig20_d['Date'], format='%Y-%m-%d')
wig20_d.set_index('Date', inplace=True)
res = trendet.identify_df_trends(df=wig20_d, column='Zamkniecie')
res.reset_index(inplace=True)

plt.figure(figsize=(20, 10))

ax = sns.lineplot(x=res['Date'], y=res['Zamkniecie'])

labels = res['Up Trend'].dropna().unique().tolist()

for label in labels:
        sns.lineplot(x=res[res['Up Trend'] == label]['Date'],
                     y=res[res['Up Trend'] == label]['Zamkniecie'],
                     color='green')

        ax.axvspan(res[res['Up Trend'] == label]['Date'].iloc[0],
                   res[res['Up Trend'] == label]['Date'].iloc[-1],
                   alpha=0.2,
                   color='green')

labels = res['Down Trend'].dropna().unique().tolist()

for label in labels:
        sns.lineplot(x=res[res['Down Trend'] == label]['Date'],
                     y=res[res['Down Trend'] == label]['Zamkniecie'],
                     color='red')

        ax.axvspan(res[res['Down Trend'] == label]['Date'].iloc[0],
                   res[res['Down Trend'] == label]['Date'].iloc[-1],
                   alpha=0.2,
                   color='red')

print(res.tail(1000))
plt.show()
    
