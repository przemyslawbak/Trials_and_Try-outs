import warnings
warnings.filterwarnings('ignore')
import pandas as pd

START = '2020-01-01'
DATA_STORE = 'data/assets.h5'
pd.set_option('display.max_columns', 500)
pd.set_option('display.width', 1000)

def verifyPeak(wolumen, sredni):
    if wolumen > int(sredni):
        return True
    return False

acp_d = pd.read_csv('../../data/wig20/acp_d.csv')
acp_d['Data'] = pd.to_datetime(acp_d['Data'].astype(str))
acp_d['Waga'] = 0.0187
acp_d['Sredni_wolumen_10'] = acp_d['Wolumen'].rolling(min_periods=1, window=10).sum() / 10
for i, row in acp_d.iterrows():
    acp_d.at[i,'Peak'] = verifyPeak(row['Wolumen'], row['Sredni_wolumen_10'])
    if row['Peak'] == True:
        acp_d.at[i,'Reference'] = row['Zamkniecie']



print(acp_d)


wig20_d = pd.read_csv('../../data/wig20/wig20_d.csv')