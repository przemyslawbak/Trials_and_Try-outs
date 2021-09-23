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

def verifyPeakDirection(otwarcie, zamkniecie, rozmiar):
    if otwarcie > zamkniecie:
        return rozmiar * -1
    else:
        return rozmiar * 1

acp_d = pd.read_csv('../../data/wig20/acp_d.csv')
acp_d['Waga'] = 0.0187
acp_d['Sredni_wolumen_10'] = acp_d['Wolumen'].rolling(min_periods=1, window=10).sum() / 10
for i, row in acp_d.iterrows():
    peak = verifyPeak(row['Wolumen'], row['Sredni_wolumen_10'])
    wielkosc_peak = (row['Wolumen'] - row['Sredni_wolumen_10']) / row['Sredni_wolumen_10']
    if peak == True and i > 10:
        acp_d.at[i,'Peak'] = peak
        acp_d.at[i,'Peak_rozmiar'] = wielkosc_peak
    else:
        acp_d.at[i,'Peak'] = False
        acp_d.at[i,'Peak_rozmiar'] = 0

    if i - 5 >= 0 and peak == True:
        acp_d.at[i,'Peak_rozmiar'] = verifyPeakDirection(acp_d.loc[i, 'Otwarcie'], acp_d.loc[i - 5, 'Zamkniecie'], acp_d.at[i,'Peak_rozmiar']) * acp_d.at[i,'Waga']

print(acp_d)

wig20_d = pd.read_csv('../../data/wig20/wig20_d.csv')
peaks = pd.DataFrame();
peaks['Data'] = wig20_d['Data']

newa = peaks.merge(acp_d[['Peak_rozmiar', 'Data']], on='Data', how='left')

print(newa)