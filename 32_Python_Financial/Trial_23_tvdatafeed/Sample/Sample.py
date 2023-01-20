from tvDatafeed import TvDatafeed, Interval
import login_data

username, password = login_data.getLoginData()

tv = TvDatafeed(username, password)

crudeoil_data = tv.get_hist(symbol='CRUDEOIL',exchange='MCX',interval=Interval.in_1_hour,n_bars=5000,fut_contract=1)

print(crudeoil_data)