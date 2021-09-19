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
#!!!!!!!NOTE:Sorting the index will lead to large gains in performance!!!!!!!
#Filtering columns with time data
#Using methods that only work with a DatetimeIndex
crime.between_time('2:00', '5:00', include_end=False)
crime.at_time('5:47')
crime_sort = crime.sort_index()
crime_sort.first(pd.offsets.MonthBegin(6))
crime_sort.first(pd.offsets.MonthEnd(6))
crime_sort.loc[:'2012-06']
crime_sort.first('5B') # 5 business days
crime_sort.first('5D') # 5 days
crime_sort.first('7W') # 7 weeks, with weeks ending on Sunday
crime_sort.first('3QS') # 3rd quarter start
crime_sort.first('A') # one year end
#Counting the number of weekly crimes
crime_sort = (pd.read_hdf('data/crime.h5', 'crime')
.set_index('REPORTED_DATE')
.sort_index()
)
crime_sort.resample('W') #we need to form a group for each week
print(crime_sort
.resample('W')
.size()
)
weekly_crimes = (crime_sort
.groupby(pd.Grouper(freq='W'))
.size()
) #alternative
len(crime_sort.loc[:'2012-1-8']) #see first day
len(crime_sort.loc['2012-1-9':'2012-1-15']) #see several days between
#Aggregating weekly crime and traffic accidents separately
(crime
.resample('Q')
[['IS_CRIME', 'IS_TRAFFIC']]
.sum()
)
#Measuring crime by weekday and year with the .dt attribute
(crime
['REPORTED_DATE']
.dt.day_name()
.value_counts()
)
#sorting by year and day of the week
(crime
.groupby([crime['REPORTED_DATE'].dt.year.rename('year'),
crime['REPORTED_DATE'].dt.day_name().
rename('day')])
.size()
.unstack('day')
)
#loading Denver population data
denver_pop = pd.read_csv('data/denver_pop.csv', index_col='Year')
den_100k = denver_pop.div(100_000).squeeze() #display data per 100 000 population
normalized = (crime
.groupby([crime['REPORTED_DATE'].dt.year.rename('year'),
crime['REPORTED_DATE'].dt.day_name().
rename('day')])
.size()
.unstack('day')
.pipe(update_2017)
.reindex(columns=days)
.div(den_100k, axis='index')
.astype(int)
)
