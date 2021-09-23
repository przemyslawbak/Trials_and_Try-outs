import warnings
warnings.filterwarnings('ignore')
import pandas as pd
import glob as g
import os

START = '2020-01-01'
DATA_STORE = 'data/assets.h5'
pd.set_option('display.max_columns', 500)
pd.set_option('display.width', 1000)

udzial = {'pko': 0.14514,
          'ale': 0.0948,
          'kgh': 0.08804,
          'pkn': 0.08767,
          'pzu': 0.08346,
          'peo': 0.0723,
          'dnp': 0.06627,
          'lpp': 0.06076,
          'cdr': 0.05405,
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

wig20_d = pd.read_csv('../../data/wig20_d.csv')
peaks = pd.DataFrame();
peaks['Data'] = wig20_d['Data']

def verifyPeak(wolumen, sredni):
    if wolumen > int(sredni):
        return True
    return False

def verifyPeakDirection(otwarcie, zamkniecie, rozmiar):
    if otwarcie > zamkniecie:
        return rozmiar * -1
    else:
        return rozmiar * 1

def processWig20Csv(df, file):
    global peaks
    name = os.path.basename(file).split('.', 1)[0] .split('_', 1)[0]
    df['Waga'] = 0.0187
    df['Sredni_wolumen_10'] = df['Wolumen'].rolling(min_periods=1, window=10).sum() / 10
    for i, row in df.iterrows():
        peak = verifyPeak(row['Wolumen'], row['Sredni_wolumen_10'])
        wielkosc_peak = (row['Wolumen'] - row['Sredni_wolumen_10']) / row['Sredni_wolumen_10']
        if peak == True and i > 10:
            df.at[i,'Peak'] = peak
            df.at[i,'Peak_rozmiar'] = wielkosc_peak
        else:
            df.at[i,'Peak'] = False
            df.at[i,'Peak_rozmiar'] = 0

        if i - 5 >= 0 and peak == True:
            df.at[i,'Peak_rozmiar'] = verifyPeakDirection(df.loc[i, 'Otwarcie'], df.loc[i - 5, 'Zamkniecie'], df.at[i,'Peak_rozmiar']) * df.at[i,'Waga']

    peaks = peaks.merge(df[['Peak_rozmiar', 'Data']], on='Data', how='left')
    peaks = peaks.rename(columns = {'Peak_rozmiar' : name + '_peak'})

#load wig20 files
files = g.glob("../../data/wig20/*.csv")
for f in files:
    df = pd.read_csv(f)
    processWig20Csv(df, f)

print(peaks)