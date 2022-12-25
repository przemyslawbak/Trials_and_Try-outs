import pandas as pd
import signals_model

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

def getBuySignal(last_signals, buy_pattern, sell_pattern):
    if sell_pattern in last_signals:
        return False
    else:
        return True

def getSellSignal(last_signals, buy_pattern, sell_pattern):
    if buy_pattern in last_signals:
        return False
    else:
        return True

def is_ready(all_signals, case, buy_pattern):
    if 'direction_pred_med' in case:
        if len(all_signals.direction_pred_med_signals) < len(buy_pattern):
            return False

    if 'direction_pred' in case:
        if len(all_signals.direction_pred_signals) < len(buy_pattern):
            return False

    if 'combined_result' in case:
        if len(all_signals.combined_result_signals) < len(buy_pattern):
            return False

    if 'pattern_sum_result_trend' in case:
        if len(all_signals.pattern_sum_result_trend_signals) < len(buy_pattern):
            return False

    if 'sentiment_trend' in case:
        if len(all_signals.sentiment_trend_signals) < len(buy_pattern):
            return False
        
    if 'med_peak_sums_trend' in case:
        if len(all_signals.med_peak_sums_trend_signals) < len(buy_pattern):
            return False

    return True

def getLastSignals(all_signals, buy_pattern):
    last_signals = []
    if 'direction_pred_med' in case:
        last_signals.append(all_signals.direction_pred_med_signals[-len(buy_pattern):])

    if 'direction_pred' in case:
        last_signals.append(all_signals.direction_pred_signals[-len(buy_pattern):])

    if 'combined_result' in case:
        last_signals.append(all_signals.combined_result_signals[-len(buy_pattern):])

    if 'pattern_sum_result_trend' in case:
        last_signals.append(all_signals.pattern_sum_result_trend_signals[-len(buy_pattern):])

    if 'sentiment_trend' in case:
        last_signals.append(all_signals.sentiment_trend_signals[-len(buy_pattern):])
        
    if 'med_peak_sums_trend' in case:
        last_signals.append(last_signals.med_peak_sums_trend_signals[-len(buy_pattern):])

    return last_signals
        
def verifySignals(transactions, buy, sell, is_long, is_short, last_close, buy_pattern, sell_pattern, case, all_signals):
    
    if buy != 0 and sell != 0:

        profit = sell - buy
        transactions.append(profit)

        buy = 0
        sell = 0
        is_long = False
        is_short = False

    if not is_ready(all_signals, case, buy_pattern):
        return transactions, buy, sell, is_long, is_short

    last_signals = getLastSignals(all_signals, buy_pattern)
        
    buy_signal = getBuySignal(last_signals, buy_pattern, sell_pattern)
    sell_signal = getSellSignal(last_signals, buy_pattern, sell_pattern)

    if buy_signal and sell_signal and (is_long or is_short):
        if is_long:
            sell = last_close
        if is_short:
            buy = last_close

    if buy_signal and not sell_signal and not is_short and not is_long:

        buy = last_close
        is_long = True

    if not buy_signal and sell_signal and not is_long and not is_short:

        sell = last_close
        is_short = True
        
    if buy_signal and not sell_signal and is_short:

        buy = last_close

    if not buy_signal and sell_signal and is_long:

        sell = last_close

    return transactions, buy, sell, is_long, is_short

def testData(case):
    signal_qtys = [1, 2, 3]
    for qty in signal_qtys:
        buy_pattern = []
        sell_pattern = []
        for i in range(qty):
            buy_pattern.append(-1.0)
            sell_pattern.append(1.0)

        all_signals = signals_model.AllSignals()
        all_signals.direction_pred_med_signals = []
        all_signals.direction_pred_signals = []
        all_signals.combined_result_signals = []
        all_signals.pattern_sum_result_trend_signals = []
        all_signals.sentiment_trend_signals = []
        all_signals.med_peak_sums_trend_signals = []

        transactions = []
        buy = 0
        sell = 0
        is_long = False
        is_short = False

        for index, row in df.iterrows():
            all_signals.direction_pred_med_signals.append(row['direction_pred_med'])
            all_signals.direction_pred_signals.append(row['direction_pred'])
            all_signals.combined_result_signals.append(row['combined_result'])
            all_signals.pattern_sum_result_trend_signals.append(row['pattern_sum_result_trend'])
            all_signals.sentiment_trend_signals.append(row['sentiment_trend'])
            all_signals.med_peak_sums_trend_signals.append(row['med_peak_sums_trend'])

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

        if (len(transactions) > 0):
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
        else:
            print('')
            print('no transactions')
            print('')

for case in cases_list:
    testData(case)