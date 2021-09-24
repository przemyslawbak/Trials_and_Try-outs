import warnings
warnings.filterwarnings('ignore')
import pandas as pd
import glob as g
import os

START = '2020-01-01'
DATA_STORE = 'data/assets.h5'
#pd.set_option('display.max_columns', 500)
pd.set_option('display.max_rows', 1000)
#pd.set_option('display.width', 1000)

weight_dictionary = {'pko': 0.14514,
          'ale': 0.0948,
          'kgh': 0.08804,
          'pkn': 0.08767,
          'pzu': 0.08346,
          'peo': 0.0723,
          'dnp': 0.06627,
          'lpp': 0.06076,
          'cdr': 0.05405,
          'pgn': 0.03975,
          'spl': 0.03879,
          'cps': 0.03515,
          'pge': 0.02887,
          'opl': 0.02029,
          'lts': 0.02014,
          'acp': 0.01869,
          'ccc': 0.01817,
          'tpe': 0.01445,
          'jsw': 0.01082,
          'mrc': 0.00239,
          }

wig20_d = pd.read_csv('../../data/wig20_d_1.csv')
peaks = pd.DataFrame();
peaks['Data'] = wig20_d['Data']
peaks['Zamkniecie'] = wig20_d['Zamkniecie']

def verifyPeak(volumen, averange):
    if volumen > int(averange):
        return True
    return False

def verifyPeakDirection(open, close, size):
    if open > close:
        return size * -1
    else:
        return size * 1

def processWig20Csv(df, file):
    global peaks
    name = os.path.basename(file).split('.', 1)[0] .split('_', 1)[0]
    df['Waga'] = weight_dictionary[name]
    df['Sredni_wolumen_10'] = df['Wolumen'].rolling(min_periods=1, window=10).sum() / 10
    for i, row in df.iterrows():
        peak = verifyPeak(row['Wolumen'], row['Sredni_wolumen_10'])
        size_peak = (row['Wolumen'] - row['Sredni_wolumen_10']) / row['Sredni_wolumen_10']
        if peak == True and i > 10:
            df.at[i,'Peak'] = peak
            df.at[i,'Peak_rozmiar'] = size_peak * df.loc[i, 'Zamkniecie']
        else:
            df.at[i,'Peak'] = False
            df.at[i,'Peak_rozmiar'] = 0

        if i + 5 < len(df.index) and peak == True:
            df.at[i,'Peak_rozmiar'] = verifyPeakDirection(df.loc[i, 'Otwarcie'], (df.loc[i + 5, 'Zamkniecie'] + df.loc[i + 4, 'Zamkniecie'] + df.loc[i + 3, 'Zamkniecie'] + df.loc[i + 2, 'Zamkniecie'] + df.loc[i + 1, 'Zamkniecie']) / 5 , df.at[i,'Peak_rozmiar']) * df.at[i,'Waga']
            #print('obecny: ' + df.loc[i, 'Data'] + ' nastepne: ' + df.loc[i - 1, 'Data']) #remove later on

    peaks = peaks.merge(df[['Peak_rozmiar', 'Data']], on='Data', how='left')
    peaks = peaks.rename(columns = {'Peak_rozmiar' : name + '_peak'})

#load wig20 files
files = g.glob("../../data/wig20/*.csv")
for f in files:
    df = pd.read_csv(f)
    processWig20Csv(df, f)

peaks['sum'] = peaks['acp_peak'].fillna(0) + peaks['ale_peak'].fillna(0) + peaks['ccc_peak'].fillna(0) + peaks['cdr_peak'].fillna(0) + peaks['cps_peak'].fillna(0) + peaks['dnp_peak'].fillna(0) + peaks['jsw_peak'].fillna(0) + peaks['kgh_peak'].fillna(0) + peaks['lpp_peak'].fillna(0) + peaks['lts_peak'].fillna(0) + peaks['mrc_peak'].fillna(0) + peaks['opl_peak'].fillna(0) + peaks['peo_peak'].fillna(0) + peaks['pge_peak'].fillna(0) + peaks['pgn_peak'].fillna(0) + peaks['pkn_peak'].fillna(0) + peaks['pko_peak'].fillna(0) + peaks['pzu_peak'].fillna(0) + peaks['spl_peak'].fillna(0) + peaks['tpe_peak'].fillna(0)

print(peaks.tail(1000))