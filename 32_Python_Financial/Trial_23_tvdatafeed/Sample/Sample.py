from tvDatafeed import TvDatafeed, Interval
import login_data
import appModels
import warnings
import pandas as pd

warnings.filterwarnings("ignore")
pd.set_option('display.max_rows', 20000)
pd.set_option('display.max_columns', 10)
pd.set_option('display.width', 1000)

#docs: https://github.com/StreamAlpha/tvdatafeed

daysBack = 7
username, password = login_data.getLoginData()

tv = TvDatafeed(username, password)

#symbols = tv.search_symbol('GOLD')
#for symbol in symbols:
#    print()
#    #print('symbol:' + symbol['symbol'] + ' exchange:' + symbol['exchange'] + ' type:' + symbol['type'] + ' provider_id:' + symbol['provider_id'] + ' description:' + symbol['description']) 
#    print()

sth = appModels.TvSearchModel('aapl', 'nasdaq', False)


if not sth.futures:
    data = tv.get_hist(symbol=sth.symbol,exchange=sth.exchange,interval=Interval.in_1_hour,n_bars=20000)
else:
    data = tv.get_hist(symbol=sth.symbol,exchange=sth.exchange,interval=Interval.in_1_hour,n_bars=20000, fut_contract=1)

data = data.reset_index()
first_day_timestamp =pd.to_datetime(pd.date_range(end='today', periods=daysBack + 1)[0]).floor('d')
last_day_timestamp =pd.to_datetime(pd.date_range(end='today', periods=daysBack + 1)[-1]).floor('d')

mask = (data['datetime'] > first_day_timestamp) & (data['datetime'] <= last_day_timestamp)
data = data.loc[mask]

data = data.rename(columns={'volume': 'Volume', 'datetime' : 'time'})

#todo: time zone

print(data.tail(1000))