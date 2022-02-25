import warnings
warnings.filterwarnings('ignore')
import pandas as pd
import trendet

START = '2020-01-01'
DATA_STORE = 'data/assets.h5'
pd.set_option('display.max_rows', 1000)

wig20_d = pd.read_csv('../../data/wig20_d_1.csv')
wig20_d.rename(columns={'Data': 'Date'}, inplace=True) #todo: to datetime first
df = trendet.identify_df_trends(df=wig20_d, column='Zamkniecie')

#print(wig20_d.tail(1000))
print(df.tail(1000))