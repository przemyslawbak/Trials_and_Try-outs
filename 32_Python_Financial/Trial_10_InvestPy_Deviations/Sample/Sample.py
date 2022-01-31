#https://stackoverflow.com/a/69166303
#https://investpy.readthedocs.io/_api/news.html
#https://www.investing.com/economic-calendar/

import investpy
import pandas as pd
pd.set_option('display.max_rows', 1000)
pd.set_option('display.max_columns', 10)
pd.set_option('display.width', 1000)


timeZoneDictionary = {
    'poland':'GMT +1:00',
    'united states':'GMT -5:00',
    'germany':'GMT +1:00',
    'euro zone':'GMT +1:00',
    'japan':'GMT +9:00',
    }

importanceDictionary = {
    'NaN' : 0.00,
    'low' : 0.15,
    'medium' : 0.40,
    'high' : 1.00,
    }

def getEconomicData(from_date, to_date, country):

    df = investpy.economic_calendar(
    from_date=from_date,
    to_date  =to_date,
    countries=[country],
    time_zone = timeZoneDictionary[country],
    )

    #combine columns: 'date' + 'time'
    df['time'] = df['time'].replace('All Day', '09:00')
    df['date_time'] = pd.to_datetime(df['date'] + ' ' + df['time'])
    df.drop('date', axis=1, inplace=True)
    df.drop('time', axis=1, inplace=True)

    #numeric imporance
    df['importance'] = df['importance'].map(importanceDictionary)

    return df

dataDf = getEconomicData('15/01/2021', '31/01/2022', 'poland')



#todo: only full hours
#todo: tz_localize time zone
#OK: combine columns: 'date' + 'time'
#OK: numeric imporance
#todo: for deviation, compute 'previous' - 'actual' difference
#todo: deviation dictionary for 'event' column
#todo: remove nones?
#todo: 'All Day' events change to 09:00
#todo: for 'united states" use 'GMT -5:00'
#todo: for 'poland" use 'GMT +1:00'
#todo: for 'germany" use 'GMT +1:00'
#todo: for 'euro zone" use 'GMT +1:00'
#todo: for 'japan" use 'GMT +9:00'




print(dataDf.head(1000))
print(dataDf.dtypes)