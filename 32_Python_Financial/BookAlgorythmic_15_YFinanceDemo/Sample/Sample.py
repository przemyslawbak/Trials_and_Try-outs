import warnings
warnings.filterwarnings('ignore')
import pandas as pd
import yfinance as yf

symbol = 'fb'
ticker = yf.Ticker(symbol)
print(ticker.info)

#Get market data
data = ticker.history(period='5d',
                      interval='1m',
                      start=None,
                      end=None,
                      actions=True,
                      auto_adjust=True,
                      back_adjust=False)
data.info()

#View company actions
# show actions (dividends, splits)
print(ticker.actions)
print(ticker.dividends)
print(ticker.splits)
print(ticker.financials)
print(ticker.quarterly_financials)
print(ticker.balance_sheet)

print(ticker.sustainability)

ticker.recommendations.info()

#and some other stuff.........