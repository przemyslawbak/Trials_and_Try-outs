import pandas as pd
import signals

pd.set_option('display.max_rows', 1000)
pd.set_option('display.max_columns', 10)
pd.set_option('display.width', 1000)

filename = "test_set.csv"
cases_file = "cases_base.csv"
df = pd.read_csv(filename, sep='|', engine='python', skiprows=[0])
df = df.reset_index()  # make sure indexes pair with number of rows
cases_list = []

with open(cases_file) as topo_file:
    for line in topo_file:
        cols = line.split()
        cases_list.append(cols)

def getBuySignal(all_signals, buy_pattern, sell_pattern, case):
    print('getBuySignal')

def getSellSignal(all_signals, buy_pattern, sell_pattern, case):
    print('getSellSignal')
        
def verifySignals(transactions, buy, sell, is_long, is_short, last_close, buy_pattern, sell_pattern, case, all_signals):
    
    if buy != 0 and sell != 0:

        profit = sell - buy
        transactions.append(profit)

        buy = 0
        sell = 0
        is_long = False
        is_short = False
        
        print('transaction closed')
        
    if 'direction_pred_med' in case:
        print('direction_pred_med')
        if len(all_signals.direction_pred_med_signals) < len(buy_pattern):
            return transactions, buy, sell, is_long, is_short

    if 'direction_pred' in case:
        print('direction_pred')
        if len(all_signals.direction_pred_signals) < len(buy_pattern):
            return transactions, buy, sell, is_long, is_short

    if 'combined_result' in case:
        print('combined_result')
        if len(all_signals.combined_result_signals) < len(buy_pattern):
            return transactions, buy, sell, is_long, is_short

    if 'pattern_sum_result_trend' in case:
        print('pattern_sum_result_trend')
        if len(all_signals.pattern_sum_result_trend_signals) < len(buy_pattern):
            return transactions, buy, sell, is_long, is_short

    if 'sentiment_trend' in case:
        print('sentiment_trend')
        if len(all_signals.sentiment_trend_signals) < len(buy_pattern):
            return transactions, buy, sell, is_long, is_short
        
    if 'med_peak_sums_trend' in case:
        print('med_peak_sums_trend')
        if len(all_signals.med_peak_sums_trend_signals) < len(buy_pattern):
            return transactions, buy, sell, is_long, is_short
        
    buy_signal = getBuySignal(all_signals, buy_pattern, sell_pattern, case)
    sell_signal = getSellSignal(all_signals, buy_pattern, sell_pattern, case)

    if buy_signal and sell_signal and (is_long or is_short):
        if is_long:
            sell = last_close
        if is_short:
            buy = last_close

        print('close position')

    if buy_signal and not sell_signal and not is_short and not is_long:

        #buy = last_close
        #is_long = True

        print('open long position')

    if not buy_signal and sell_signal and not is_long and not is_short:

        sell = last_close
        is_short = True

        print('open short position')
        
    if buy_signal and not sell_signal and is_short:

        buy = last_close

        print('close position with buy')

    if not buy_signal and sell_signal and is_long:

        sell = last_close

        print('close position with sell')

    return transactions, buy, sell, is_long, is_short

def testData(case):
    signal_qtys = [1, 2, 3]
    for qty in signal_qtys:
        buy_pattern = []
        sell_pattern = []
        for i in range(qty):
            buy_pattern.append(-1.0)
            sell_pattern.append(1.0)

        all_signals = signals.AllSignals()
        signals.direction_pred_med_signals = []
        signals.combined_result_signals = []
        signals.pattern_sum_result_trend_signals = []
        signals.sentiment_trend_signals = []
        signals.med_peak_sums_trend_signals = []

        transactions = []
        buy = 0
        sell = 0
        is_long = False
        is_short = False

        for index, row in df.iterrows():
            signals.direction_pred_med_signals.append(row['direction_pred_med'])
            signals.direction_pred_signals.append(row['direction_pred'])
            signals.combined_result_signals.append(row['combined_result'])
            signals.pattern_sum_result_trend_signals.append(row['pattern_sum_result_trend'])
            signals.sentiment_trend_signals.append(row['sentiment_trend'])
            signals.med_peak_sums_trend_signals.append(row['med_peak_sums_trend'])

            transactions, buy, sell, is_long, is_short = verifySignals(transactions, buy, sell, is_long, is_short, row['last_close'], buy_pattern, sell_pattern, case, all_signals)
            
        total_negative = 0
        total_positive = 0
        positives_sum = 0
        negatives_sum = 0
        for value in transactions:
            if value < 0:
                total_negative += 1
                negatives_sum += value
            else:
                total_positive += 1
                positives_sum += value
                
        print('')
        print('SUMMARY:')
        print('transactions no.: ' + str(len(transactions)))
        print('profits total: ' + str(sum(transactions)))
        print('max income: ' + str(max(transactions)))
        print('max dropdown: ' + str(min(transactions)))
        print('success total: ' + str(round(total_positive / len(transactions) * 100)) + ' %')
        print('failed total: ' + str(round(total_negative / len(transactions) * 100)) + ' %')
        print('success med: ' + str(round(positives_sum / total_positive)))
        print('failed med: ' + str(round(negatives_sum / total_negative)))
        print('')

for case in cases_list:
    testData(case)