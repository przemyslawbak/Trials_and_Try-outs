import warnings
warnings.filterwarnings('ignore')
import pandas as pd
import glob as g
import os

START = '2020-01-01'
DATA_STORE = 'data/assets.h5'
pd.set_option('display.max_rows', 1000)

wig20_d = pd.read_csv('../../data/wig20_d_1.csv')

print(wig20_d.tail(1000))