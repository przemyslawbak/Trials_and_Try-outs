from tvDatafeed import TvDatafeed, Interval
import login_data
import appModels

#docs: https://github.com/StreamAlpha/tvdatafeed

username, password = login_data.getLoginData()

tv = TvDatafeed(username, password)

#symbols = tv.search_symbol('GOLD')
#for symbol in symbols:
#    print()
#    #print('symbol:' + symbol['symbol'] + ' exchange:' + symbol['exchange'] + ' type:' + symbol['type'] + ' provider_id:' + symbol['provider_id'] + ' description:' + symbol['description']) 
#    print()

sth = appModels.TvSearchModel('NATURALGAS', 'CURRENCYCOM', False)


if not sth.futures:
    data = tv.get_hist(symbol=sth.symbol,exchange=sth.exchange,interval=Interval.in_1_hour,n_bars=20000)
else:
    data = tv.get_hist(symbol=sth.symbol,exchange=sth.exchange,interval=Interval.in_1_hour,n_bars=20000, fut_contract=1)

print(data)