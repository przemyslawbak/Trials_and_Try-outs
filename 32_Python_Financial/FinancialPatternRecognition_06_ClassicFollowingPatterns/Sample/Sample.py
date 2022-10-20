import numpy as np
import pandas as pd
import matplotlib.pyplot as plt

# Import data
df = pd.read_csv('GPW_DLY WIG20, 15.csv', usecols=["close", 'open', 'high', 'low'])
df.reset_index(drop=True)
my_data = np.array(df)

# Creating the signal function
def signal_MARUBOZU(data, open_column, high_column, low_column, close_column, buy_column, sell_column):

    data = add_column(data, 5)    
    
    for i in range(len(data)):  
        
       try:
           
           # Bullish pattern
           if data[i, close_column] > data[i, open_column] and data[i, high_column] == data[i, close_column] and \
              data[i, low_column] == data[i, open_column] and data[i, buy_column] == 0:
                  
                    data[i + 1, buy_column] = 1 
                    
           # Bearish pattern
           elif data[i, close_column] < data[i, open_column] and data[i, high_column] == data[i, open_column] and \
                data[i, low_column] == data[i, close_column] and data[i, sell_column] == 0:
                  
                    data[i + 1, sell_column] = -1 
                    
       except IndexError:
            
            pass
        
    return data

# Creating the signal function
def signal_THREE_CANDLES(data, open_column, close_column, buy_column, sell_column):
    body = 0.0005#You can also adjust the variable body to volatility
    data = add_column(data, 5)
    
    for i in range(len(data)):
        
       try:
           
           # Bullish pattern
           if data[i, close_column] - data[i, open_column] > body and \
              data[i - 1, close_column] - data[i - 1, open_column] > body and \
              data[i - 2, close_column] - data[i - 2, open_column] > body and \
              data[i, close_column] > data[i - 1, close_column] and \
              data[i - 1, close_column] > data[i - 2, close_column] and \
              data[i - 2, close_column] > data[i - 3, close_column] and \
              data[i, buy_column] == 0:
                  
                    data[i + 1, buy_column] = 1
                    
           # Bearish pattern
           elif data[i, open_column] - data[i, close_column] > body and \
                data[i - 1, open_column] - data[i - 1, close_column] > body and \
                data[i - 2, open_column] - data[i - 2, close_column] > body and \
                data[i, close_column] < data[i - 1, close_column] and \
                data[i - 1, close_column] < data[i - 2, close_column] and \
                data[i - 2, close_column] < data[i - 3, close_column] and \
                data[i, sell_column] == 0:
                  
                    data[i + 1, sell_column] = -1
                    
       except IndexError:
            
            pass
        
    return data

def signal_TASUKI(data, open_column, close_column, buy_column, sell_column):

    data = add_column(data, 5)
    
    for i in range(len(data)):
        
       try:
           
           # Bullish pattern
           if data[i, close_column] < data[i, open_column] and \
              data[i, close_column] < data[i - 1, open_column] and \
              data[i, close_column] > data[i - 2, close_column] and \
              data[i - 1, close_column] > data[i - 1, open_column] and \
              data[i - 1, open_column] > data[i - 2, close_column] and \
              data[i - 2, close_column] > data[i - 2, open_column]:
                  
                    data[i + 1, buy_column] = 1
                    
           # Bearish pattern
           elif data[i, close_column] > data[i, open_column] and \
                data[i, close_column] > data[i - 1, open_column] and \
                data[i, close_column] < data[i - 2, close_column] and \
                data[i - 1, close_column] < data[i - 1, open_column] and \
                data[i - 1, open_column] < data[i - 2, close_column] and \
                data[i - 2, close_column] < data[i - 2, open_column]:
                  
                    data[i + 1, sell_column] = -1
                    
       except IndexError:
            
            pass
        
    return data

def signal_THREE_METHODS(data, open_column, high_column, low_column, close_column, buy_column, sell_column):

    data = add_column(data, 5)
    
    for i in range(len(data)):
        
       try:
           
           # Bullish pattern
           if data[i, close_column] > data[i, open_column] and \
              data[i, close_column] > data[i - 4, high_column] and \
              data[i, low_column] < data[i - 1, low_column] and \
              data[i - 1, close_column] < data[i - 4, close_column] and \
              data[i - 1, low_column] > data[i - 4, low_column] and \
              data[i - 2, close_column] < data[i - 4, close_column] and \
              data[i - 2, low_column] > data[i - 4, low_column] and \
              data[i - 3, close_column] < data[i - 4, close_column] and \
              data[i - 3, low_column] > data[i - 4, low_column] and \
              data[i - 4, close_column] > data[i - 4, open_column]:
                  
                    data[i + 1, buy_column] = 1
                    
           # Bearish pattern
           elif data[i, close_column] < data[i, open_column] and \
                data[i, close_column] < data[i - 4, low_column] and \
                data[i, high_column] > data[i - 1, high_column] and \
                data[i - 1, close_column] > data[i - 4, close_column] and \
                data[i - 1, high_column] < data[i - 4, high_column] and \
                data[i - 2, close_column] > data[i - 4, close_column] and \
                data[i - 2, high_column] < data[i - 4, high_column] and \
                data[i - 3, close_column] > data[i - 4, close_column] and \
                data[i - 3, high_column] < data[i - 4, high_column] and \
                data[i - 4, close_column] < data[i - 4, open_column]:
                  
                    data[i + 1, sell_column] = -1
                    
       except IndexError:
            
            pass
        
    return data

def signal_HIKKAKE(data, open_column, high_column, low_column, close_column, buy_signal, sell_signal):

    data = add_column(data, 5)    
    
    for i in range(len(data)):    

       try:
        
            # Bullish pattern
            if data[i, close_column] > data[i - 3, high_column] and \
               data[i, close_column] > data[i - 4, close_column] and \
               data[i - 1, low_column] < data[i, open_column] and \
               data[i - 1, close_column] < data[i, close_column] and \
               data[i - 1, high_column] <= data[i - 3, high_column] and \
               data[i - 2, low_column] < data[i, open_column] and \
               data[i - 2, close_column] < data[i, close_column] and \
               data[i - 2, high_column] <= data[i - 3, high_column] and \
               data[i - 3, high_column] < data[i - 4, high_column] and \
               data[i - 3, low_column] > data[i - 4, low_column] and \
               data[i - 4, close_column] > data[i - 4, open_column]: 
                
                   data[i + 1, buy_signal] = 1 
                 
            # Bearish pattern
            elif data[i, close_column] < data[i - 3, low_column] and \
                 data[i, close_column] < data[i - 4, close_column] and \
                 data[i - 1, high_column] > data[i, open_column] and \
                 data[i - 1, close_column] > data[i, close_column] and \
                 data[i - 1, low_column] >= data[i - 3, low_column] and \
                 data[i - 2, high_column] > data[i, open_column] and \
                 data[i - 2, close_column] > data[i, close_column] and \
                 data[i - 2, low_column] >= data[i - 3, low_column] and \
                 data[i - 3, low_column] > data[i - 4, low_column] and \
                 data[i - 3, high_column] < data[i - 4, high_column] and \
                 data[i - 4, close_column] < data[i - 4, open_column]: 
        
                     data[i + 1, sell_signal] = -1
                   
       except IndexError:
            
            pass
        
    return data


def delete_row(data, number):
    data = data[number:, ]
    return data

def add_row(data, times):
    for i in range(1, times + 1):
        columns = np.shape(data)[1]
        new = np.zeros((1, columns), dtype = float)
        data = np.append(data, new, axis = 0)
    return data

def add_column(data, times):
    for i in range(1, times + 1):
        new = np.zeros((len(data), 1), dtype = float)
        data = np.append(data, new, axis = 1)
    return data

def delete_column(data, index, times):
    for i in range(1, times + 1):
        data = np.delete(data, index, axis = 1)
    return data

def rounding(data, how_far):
    data = data.round(decimals = how_far)
    return data

def signal_chart(data, position, buy_column, sell_column, window = 500):
    sample = data[-window:, ]
    fig, ax = plt.subplots(figsize = (10, 5))
    ohlc_plot_bars(data, window)
    for i in range(len(sample)):
        if sample[i, buy_column] == 1:
            x = i
            y = sample[i, position]
            ax.annotate(' ', xy = (x, y),arrowprops = dict(width = 9, headlength = 11,headwidth = 11, facecolor = 'green', color ='green'))
        elif sample[i, sell_column] == -1:
            x = i
            y = sample[i, position]
            ax.annotate(' ', xy = (x, y),arrowprops = dict(width = 9, headlength = -11,headwidth = -11, facecolor = 'red', color ='red'))

def ohlc_plot_bars(data, window):
    sample = data[-window:, ]
    for i in range(len(sample)):
        plt.vlines(x = i, ymin = sample[i, 2], ymax = sample[i, 1], color = 'black', linewidth = 1)
        if sample[i, 3] > sample[i, 0]:
            plt.vlines(x = i, ymin = sample[i, 0], ymax = sample[i, 3], color = 'black', linewidth = 1)
        if sample[i, 3] < sample[i, 0]:
            plt.vlines(x = i, ymin = sample[i, 3], ymax = sample[i, 0], color = 'black', linewidth = 1)
        if sample[i, 3] == sample[i, 0]:
            plt.vlines(x = i, ymin = sample[i, 3], ymax = sample[i, 0] + 0.00003, color = 'black', linewidth = 1.00)
    plt.grid()

def performance(data, open_price,buy_column,sell_column,long_result_col,short_result_col,total_result_col):
    # Variable holding period
    for i in range(len(data)):
        try:
            if data[i, buy_column] == 1:
                for a in range(i + 1, i + 1000):
                    if data[a, buy_column] == 1 or data[a, sell_column]== -1:
                        data[a, long_result_col] = data[a, open_price] -data[i, open_price]
                        break
                    else:
                        continue
            else:
                continue

        except IndexError:
            pass

    for i in range(len(data)):
        try:
            if data[i, sell_column] == -1:
                for a in range(i + 1, i + 1000):
                    if data[a, buy_column] == 1 or data[a, sell_column]== -1:
                        data[a, short_result_col] = data[i, open_price] -data[a, open_price]
                        break
                    else:
                        continue

            else:
                continue

        except IndexError:
            pass

    # Aggregating the long & short results into one column\
    data[:, total_result_col] = data[:, long_result_col] + data[:, short_result_col]

    # Profit factor
    total_net_profits = data[data[:, total_result_col] > 0,total_result_col]
    total_net_losses = data[data[:, total_result_col] < 0,total_result_col]
    total_net_losses = abs(total_net_losses)
    profit_factor = round(np.sum(total_net_profits) /np.sum(total_net_losses), 2)

    # Hit ratio
    hit_ratio = len(total_net_profits) / (len(total_net_losses) + len(total_net_profits))
    hit_ratio = hit_ratio * 100

    # Risk reward ratio
    average_gain = total_net_profits.mean()
    average_loss = total_net_losses.mean()
    realized_risk_reward = average_gain / average_loss

    # Number of trades
    trades = len(total_net_losses) + len(total_net_profits)

    print('Hit Ratio = ', hit_ratio)
    print('Profit factor = ', profit_factor)
    print('Realized RR = ', round(realized_risk_reward, 3))
    print('Number of Trades = ', trades)



# Calling the signal function
my_data = signal_HIKKAKE(my_data, 0, 1, 2, 3, 4, 5)
# Charting the latest signals
signal_chart(my_data, 0, 4, 5, window = 1000)
# Performance
my_data = performance(my_data, 0, 4, 5, 6, 7, 8)
print(my_data)
plt.show()