import pandas as pd

pd.set_option('display.max_rows', 1000)
pd.set_option('display.max_columns', 10)
pd.set_option('display.width', 1000)

filename = "test_set.txt"
df = pd.read_csv(filename, sep='|', engine='python', skiprows=[0])
df = df.reset_index()  # make sure indexes pair with number of rows

buy_signals = []
sell_signals = []
profits = []

def verifySignals(buy, sell, is_long, is_short):

    if len(buy_signals) < 3:
        return buy, sell, is_long, is_short

    buy_signal = [-1.0, -1.0, -1.0]
    sell_signal = [1.0, 1.0, 1.0]
    
    buy_sample = buy_signals[-3:]
    sell_sample = sell_signals[-3:]

    if buy_signal == buy_sample and sell_signal == sell_sample and (is_long or is_short):
        is_long = False
        is_short = False
        #todo: close position
        print('close position')

    if buy_signal == buy_sample and sell_signal != sell_sample and not is_short and not is_long:
        is_long = True
        is_short = False
        #todo: long position
        print('long position')

    if buy_signal != buy_sample and sell_signal == sell_sample and not is_long and not is_short:
        is_long = False
        is_short = True
        #todo: short position
        print('short position')
        
    if buy_signal == buy_sample and sell_signal != sell_sample and is_short:
        is_long = False
        is_short = False
        #todo: close position
        print('close position')

    if buy_signal != buy_sample and sell_signal == sell_sample and is_long:
        is_long = False
        is_short = False
        #todo: close position
        print('close position')


    if buy != 0 and sell != 0:
        #todo:
        print('transaction closed')
        buy = 0
        sell = 0

    return buy, sell, is_long, is_short

buy = 0
sell = 0
is_long = False
is_short = False

for index, row in df.iterrows():
    buy_signals.append(row['direction_pred_med'])
    sell_signals.append(row['direction_pred'])
    buy, sell, is_long, is_short = verifySignals(buy, sell, is_long, is_short)



print(df)