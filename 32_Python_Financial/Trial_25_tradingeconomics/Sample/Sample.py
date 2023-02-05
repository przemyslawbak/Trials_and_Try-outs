import tradingeconomics as te

#docs: http://docs.tradingeconomics.com/#snapshot-markets10

te.login()

#base:

indyki = te.getIndicatorData(output_type='df')
print('indyki')
print(indyki)

countries = te.getAllCountries(output_type='df')
print('countries')
print(countries)

gdp = te.getIndicatorData(country='poland', indicators='gdp') #HTTP Error 403: No Access to this feature.
print('gdp')
print(gdp)

#peers = te.getPeers(country='poland', output_type='df') #HTTP Error 403: No Access to this feature.
#print('peers')
#print(peers)

#category = te.getPeers(country='poland', category='money', output_type='df') #HTTP Error 403: No Access to this feature.
#print('category')
#print(category)

historical = te.getHistoricalData(country='poland', indicator='gdp') #HTTP Error 403: No Access to this feature.
print('historical')
print(historical)

ratings = te.getRatings(output_type='df')
print('ratings')
print(ratings)

#calendar

#calendar = te.getCalendarData(importance='1', output_type='df', country='poland') #HTTP Error 403: No Access to this feature.
#print('calendar')
#print(calendar)

inflation = te.getCalendarData(category='inflation rate', initDate='2018-02-01', endDate='2018-02-02', output_type='df')
print('inflation')
print(inflation)

#multiple = te.getCalendarData(country=['poland','united kingdom'], initDate='2022-02-10', endDate='2022-03-15', importance='1', output_type='df') #HTTP Error 403: No Access to this feature.
#print('multiple')
#print(multiple)

ids = te.getCalendarId(id = ['160025',  '174108',  '160030'], output_type=  'df')
print('ids')
print(ids)

#markets

markets = te.getMarketsData(marketsField = 'commodities', output_type = 'df')
print('markets')
print(markets)

history = te.getHistorical(symbol='aapl:us',output_type='df')
print('history')
print(history)

#financial

financial = te.getFinancialsData(country = 'poland', output_type = 'df') #None
print('financial')
print(financial)

assets = te.getHistoricalFinancials(symbol = 'aapl:us', category = 'assets', output_type = 'df')
print('assets')
print(assets)

#forecasts

forecast = te.getForecastData(country='poland', indicator= 'gdp', output_type='df') #HTTP Error 403: No Access to this feature.
print('forecast')
print(forecast)

#news

#streaming

#search

#Eurostat

#World Bank

#Comtrade (?)

#Federal Reserve

