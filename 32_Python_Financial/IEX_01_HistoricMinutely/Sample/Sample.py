from datetime import datetime
from iexfinance.stocks import get_historical_intraday
import pandas as pd

date = datetime(2019, 5, 10)

df = get_historical_intraday("AAPL", date) #The IEX Cloud API key must be provided

print(df)