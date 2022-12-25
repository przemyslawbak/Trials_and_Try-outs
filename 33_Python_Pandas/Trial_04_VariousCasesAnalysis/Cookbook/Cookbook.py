import pandas as pd

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

for case in cases_list:
    testData(case)










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

    buy_signal = [-1.0, -1.0]
    sell_signal = [1.0, 1.0]
    
    buy_sample = sell_signals[-len(buy_signal):] #sell_signals[-3:] <- best performance for now
    sell_sample = sell_signals[-len(sell_signal):]

    if buy_signal == buy_sample and sell_signal == sell_sample and (is_long or is_short):
        if is_long:
            sell = last_close
        if is_short:
            buy = last_close

        print('close position')

    if buy_signal == buy_sample and sell_signal != sell_sample and not is_short and not is_long:

        #buy = last_close
        #is_long = True

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


def testData(case):
    signal_qtys = [1, 2, 3]
    for qty in signal_qtys:
        buy_signal = []
        sell_signal = []
        for i in range(qty):
            buy_signal.append(-1.0)
            sell_signal.append(1.0)

        direction_pred_med_signals = []
        direction_pred_signals = []
        combined_result_signals = []
        pattern_sum_result_trend_signals = []
        sentiment_trend_signals = []
        med_peak_sums_trend_signals = []

        transactions = []
        buy = 0
        sell = 0
        is_long = False
        is_short = False

        for index, row in df.iterrows():

            direction_pred_med_signals.append(row['direction_pred_med'])
            direction_pred_signals.append(row['direction_pred'])
            combined_result_signals.append(row['combined_result'])
            pattern_sum_result_trend_signals.append(row['pattern_sum_result_trend'])
            sentiment_trend_signals.append(row['sentiment_trend'])
            med_peak_sums_trend_signals.append(row['med_peak_sums_trend'])

            buy, sell, is_long, is_short = verifySignals(buy, sell, is_long, is_short, row['last_close'], buy_signal, sell_signal, case)



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

#todo: save to the file
#todo: test multiple variants in separate project