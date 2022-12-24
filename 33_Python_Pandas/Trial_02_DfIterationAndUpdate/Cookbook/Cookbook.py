import pandas as pd

pd.set_option('display.max_rows', 1000)
pd.set_option('display.max_columns', 10)
pd.set_option('display.width', 1000)

filename = "test_set.txt"
df = pd.read_csv(filename, sep='|', engine='python', skiprows=[0])
df = df.reset_index()  # make sure indexes pair with number of rows

buy_signals = []
sell_signals = []
transactions = []

def verifySignals(buy, sell, is_long, is_short, last_close):


    if buy != 0 and sell != 0:

        profit = sell - buy
        transactions.append(profit)

        buy = 0
        sell = 0
        is_long = False
        is_short = False
        
        print('transaction closed')

    if len(buy_signals) < 3:
        return buy, sell, is_long, is_short

    buy_signal = [-1.0, -1.0, -1.0]
    sell_signal = [1.0, 1.0, 1.0]
    
    buy_sample = buy_signals[-3:] #sell_signals[-3:] <- best performance for now
    sell_sample = sell_signals[-3:]

    if buy_signal == buy_sample and sell_signal == sell_sample and (is_long or is_short):
        if is_long:
            sell = last_close
        if is_short:
            buy = last_close

        print('close position')

    if buy_signal == buy_sample and sell_signal != sell_sample and not is_short and not is_long:

        buy = last_close
        is_long = True

        print('open long position')

    if buy_signal != buy_sample and sell_signal == sell_sample and not is_long and not is_short:

        sell = last_close
        is_short = True

        print('open short position')
        
    if buy_signal == buy_sample and sell_signal != sell_sample and is_short:

        buy = last_close

        print('close position with buy')

    if buy_signal != buy_sample and sell_signal == sell_sample and is_long:

        sell = last_close

        print('close position with sell')

    return buy, sell, is_long, is_short

buy = 0
sell = 0
is_long = False
is_short = False

for index, row in df.iterrows():
    buy_signals.append(row['direction_pred_med'])
    sell_signals.append(row['direction_pred'])
    buy, sell, is_long, is_short = verifySignals(buy, sell, is_long, is_short, row['last_close'])

print('')
print('SUMMARY:')
print('transactions no.: ' + str(len(transactions)))
print('profits total: ' + str(sum(transactions)))
print('max income: ' + str(max(transactions)))
print('max dropdown: ' + str(min(transactions)))

#todo: save to the file
#todo: test multiple variants in separate project