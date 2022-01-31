#https://stackoverflow.com/a/69166303
#https://investpy.readthedocs.io/_api/news.html
#https://www.investing.com/economic-calendar/

import investpy
import pandas as pd
pd.set_option('display.max_rows', 1000)
pd.set_option('display.max_columns', 10)
pd.set_option('display.width', 1000)

localizeDictionary = {
    'poland':'Europe/Berlin',
    'united states':'US/Eastern',
    'germany':'Europe/Berlin',
    'euro zone':'Europe/Berlin',
    'japan':'Japan',
    }

importanceDictionary = {
    'low' : 0.15,
    'medium' : 0.40,
    'high' : 1.00,
    }

replacementDictionary = {"B": "", "M": "", "%": "", ",": ""}

def getEconomicData(from_date, to_date, country):

    df = investpy.economic_calendar(
    from_date=from_date,
    to_date  =to_date,
    countries=[country],
    )

    #'All Day' events remove
    df = df[df['time'] != 'All Day']

    #combine columns: 'date' + 'time'
    df['date_time'] = pd.to_datetime(df['date'] + ' ' + df['time'])
    df.drop('date', axis=1, inplace=True)
    df.drop('time', axis=1, inplace=True)

    #only full hours
    df['date_time'] = df['date_time'] - pd.to_timedelta(df['date_time'].dt.minute, unit='m')

    #tz_localize time zone
    df['date_time'] = df['date_time'].dt.tz_localize('GMT').dt.tz_convert(localizeDictionary[country])

    #numeric imporance
    df['importance'] = df['importance'].map(importanceDictionary).fillna(0.00)

    #replacing substrings
    df['actual'] = df['actual'].str.replace('|'.join(replacementDictionary), lambda string: replacementDictionary[string.group()]).fillna('0.0')
    df['previous'] = df['previous'].str.replace('|'.join(replacementDictionary), lambda string: replacementDictionary[string.group()]).fillna('0.0')
    df['forecast'] = df['forecast'].str.replace('|'.join(replacementDictionary), lambda string: replacementDictionary[string.group()]).fillna(df['previous'])
    df["actual"] = pd.to_numeric(df["actual"])
    df["previous"] = pd.to_numeric(df["previous"])
    df["forecast"] = pd.to_numeric(df["forecast"])


    return df

dataDf = getEconomicData('15/01/2021', '31/01/2022', 'poland')



#OK: only full hours
#OK: tz_localize time zone
#OK: combine columns: 'date' + 'time'
#OK: numeric imporance
#todo: for deviation, compute 'previous' - 'actual' difference
#todo: deviation dictionary for 'event' column
#OK: remove nones?
#NO: 'All Day' events change to 09:00
#OK: 'All Day' events remove



print(dataDf.head(1000))
print(dataDf.dtypes)