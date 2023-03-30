import warnings
import pandas as pd
import yfinance as yf

warnings.filterwarnings("ignore")
pd.set_option('display.max_rows', 20000)
pd.set_option('display.max_columns', 10)
pd.set_option('display.width', 1000)

symbol = 'acp.wa'
ticker = yf.Ticker(symbol)
print(ticker.info)

#Get market data
data = ticker.history(period='7d', #max for 1m -> 7d
                      interval='1m', #max 60 days
                      start=None,
                      end=None,
                      actions=True,
                      auto_adjust=True,
                      back_adjust=False)
data.info()

#View company actions
#print(ticker.actions)
#print(ticker.dividends)
#print(ticker.splits)
#print(ticker.financials)
#print(ticker.quarterly_financials)
#print(ticker.balance_sheet)

#print(ticker.sustainability)

print(data.tail(1000)) #looks LIVE


#and some other stuff.........