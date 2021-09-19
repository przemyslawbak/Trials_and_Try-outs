#Time Series Analysis
import pandas as pd
import numpy as np
import datetime

#datetime
date = datetime.date(year=2013, month=6, day=7)
time = datetime.time(hour=12, minute=30,
second=19, microsecond=463198)
dt = datetime.datetime(year=2013, month=6, day=7,
hour=12, minute=30, second=19,
microsecond=463198)
print(f"date is {date}")
print(f"time is {time}")
print(f"datetime is {dt}")
#time delta
td = datetime.timedelta(weeks=2, days=5, hours=10,
minutes=20, seconds=6.73,
milliseconds=99, microseconds=8)
print(td)
datetime.timedelta(days=19, seconds=37206, microseconds=829008)
print(f'new date is {date+td}')
print(f'new datetime is {dt+td}')
#timestamp
pd.Timestamp(year=2012, month=12, day=21, hour=5, minute=10, second=8, microsecond=99)
pd.Timestamp('2016/1/10')
pd.Timestamp('2014-5/10')
pd.Timestamp('Jan 3, 2019 20:45.56')
pd.Timestamp('2016-01-05T05:34:43.123456789')
pd.Timestamp(500)
pd.Timestamp(5000, unit='D')
pd.to_datetime('2015-5-13')
pd.to_datetime('2015-13-5', dayfirst=True)
pd.to_datetime('Start Date: Sep 30, 2017 Start Time: 1:30 pm', format='Start Date: %b %d, %Y Start Time: %I:%M %p')
pd.to_datetime(100, unit='D', origin='2013-1-1')
#timestamp feaures
ts = pd.Timestamp('2016-10-1 4:23:23.9')
ts.ceil('h')
ts.year, ts.month, ts.day, ts.hour, ts.minute, ts.second
ts.dayofweek, ts.dayofyear, ts.daysinmonth
ts.to_pydatetime()
td = pd.Timedelta(125.8723, unit='h') #Timedelta('5 days 05:52:20.280000')
td.round('min')
td.components #Components(days=5, hours=5, minutes=52, seconds=20, milliseconds=280, microseconds=0, nanoseconds=0)
td.total_seconds()
#Slicing time series intelligently
crime = pd.read_hdf('data/crime.h5', 'crime')
print(crime.dtypes)
crime = crime.set_index('REPORTED_DATE')
print('test')
#selecting examples
crime.loc['2016-05'].shape
crime.loc['2016'].shape
crime.loc['2016-05-12 03'].shape
crime.loc['2016 Sep, 15'].shape
crime.loc['21st October 2014 05'].shape
print(crime.loc['2016-05-12 16:45:00'])
#!!!!!!!Sorting the index will lead to large gains in performance!!!!!!!