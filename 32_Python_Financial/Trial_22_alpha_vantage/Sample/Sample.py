from alpha_vantage.timeseries import TimeSeries
ts = TimeSeries(key='XZVOUCI0LT0VD2EL')
# Get json object with the intraday data and another with  the call's metadata
data, meta_data = ts.get_intraday('^NYA')
print(data)
print(meta_data)