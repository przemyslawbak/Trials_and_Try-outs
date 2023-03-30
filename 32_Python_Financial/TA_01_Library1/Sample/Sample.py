import pandas as pd
from ta.utils import dropna
from ta.volatility import BollingerBands
import matplotlib.pyplot as plt


#https://github.com/bukosabino/ta
#https://technical-analysis-library-in-python.readthedocs.io/en/latest/ta.html#momentum-indicators

pd.set_option('display.max_rows', 1000)
pd.set_option('display.max_columns', 10)
pd.set_option('display.width', 1000)

filename = "GPW_DLY WIG20, 15.csv"
base_df = pd.read_csv(filename)

##Bollinger Bands
df = base_df
indicator_bb = BollingerBands(close=df["close"], window=20, window_dev=2)

# Add Bollinger Bands features
df['bb_bbm'] = indicator_bb.bollinger_mavg()
df['bb_bbh'] = indicator_bb.bollinger_hband()
df['bb_bbl'] = indicator_bb.bollinger_lband()

# Add Bollinger Band high indicator
#df['bb_bbhi'] = indicator_bb.bollinger_hband_indicator()

# Add Bollinger Band low indicator
#df['bb_bbli'] = indicator_bb.bollinger_lband_indicator()

print(df.tail(1000))

plt_df = pd.DataFrame()
plt_df['close'] = df['close']
plt_df['bb_bbh'] = df['bb_bbh']
plt_df['bb_bbl'] = df['bb_bbl']

plt_df.plot()

plt.show()