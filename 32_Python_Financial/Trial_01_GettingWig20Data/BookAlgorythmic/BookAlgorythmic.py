import warnings
warnings.filterwarnings('ignore')
import pandas as pd

START = '2020-01-01'
DATA_STORE = 'data/assets.h5'

acp_d = pd.read_csv('../../data/wig20/acp_d.csv')
acp_d['Waga'] = 0.0187
acp_d['Sredni_ostatnie'] = acp_d.shift(1)['Wolumen']
wig20_d = pd.read_csv('../../data/wig20/wig20_d.csv')
print(acp_d)