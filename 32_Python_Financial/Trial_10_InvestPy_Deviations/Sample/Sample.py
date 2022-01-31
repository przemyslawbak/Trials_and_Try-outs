#https://stackoverflow.com/a/69166303
#https://investpy.readthedocs.io/_api/news.html
#https://www.investing.com/economic-calendar/

import investpy
import pandas as pd
import numpy as np
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

replacementDictionary = {"B": "", "M": "", "%": "", ",": "", "K": "", "T": ""}

#2021-01-19 last
deviationScoreDictionary = {
    'TIC Net Long-Term Transactions including Swaps...' : 0.01,
    'TIC Net Long-Term Transactions' : 0.01,
    'Overall Net Capital Flow' : 0.01,
    'US Foreign Buying, T-bonds' : -0.1,
    '3-Month Bill Auction' : -100,
    '6-Month Bill Auction' : -100,
    'Treasury Secretary Yellen Speaks' : 0,
    'IEA Monthly Report' : 0,
    'Investing.com Gold Index' : -0.05,
    'Investing.com S&P 500 Index' : 0.05,
    'CFTC Soybeans speculative net positions' : 0.05,
    'CFTC Silver speculative net positions' : -0.05,
    'CFTC S&P 500 speculative net positions' : 0.05,
    'CFTC Natural Gas speculative net positions' : 0.05,
    'CFTC Nasdaq 100 speculative net positions' : 0.05,
    'CFTC Gold speculative net positions' : -0.05,
    'CFTC Crude Oil speculative net positions' : 0.05,
    'CFTC Corn speculative net positions' : 0.05,
    'CFTC Copper speculative net positions' : 0.1,
    'CFTC Aluminium speculative net positions' : 0.1,
    'U.S. Baker Hughes Oil Rig Count' : 0.2,
    'U.S. Baker Hughes Total Rig Count' : 0.2,
    'Michigan 5-Year Inflation Expectations' : 0.3,
    'Michigan Consumer Expectations' : 0.3,
    'Michigan Current Conditions' : 0.3,
    'Michigan Inflation Expectations' : 0.3,
    'Business Inventories (MoM)' : 2,
    'Manufacturing Production (MoM)' : 2,
    'Capacity Utilization Rate' : 3,
    'Industrial Production (MoM)' : 2,
    'Industrial Production (YoY)' : 2,
    'Retail Sales Ex Gas/Autos (MoM)' : 5,
    'Retail Inventories Ex Auto' : 5,
    'Retail Sales Ex Gas/Autos (YoY)' : 5,
    'Retail Control (MoM)' : 1.33,
    'Retail Sales (MoM)' : 1.33,
    'Retail Sales (YoY)' : 1.33,
    'PPI (MoM)' : 7.5,
    'PPI (YoY)' : 7.5,
    'Core PPI (YoY)' : 5,
    'Core PPI (MoM)' : 5,
    'Core Retail Sales (MoM)' : 1.4,
    'NY Empire State Manufacturing Index' : 0.14,
    }

def getEconomicData(from_date, to_date, country):

    df = investpy.economic_calendar(
    from_date=from_date,
    to_date  =to_date,
    countries=[country],
    )

    #'All Day' and 'Tentative' events remove
    df = df[df['time'] != 'All Day']
    df = df[df['time'] != 'Tentative']
    df = df[df['time'] != '']

    #combine columns: 'date' + 'time'
    df['date_time'] = pd.to_datetime(df['date'] + ' ' + df['time'], format="%d/%m/%Y %H:%M")
    df.drop(['date', 'time', 'currency', 'zone', 'id'], axis=1, inplace=True)

    #only full hours
    df['date_time'] = df['date_time'] - pd.to_timedelta(df['date_time'].dt.minute, unit='m').sort_values()

    #tz_localize time zone
    df['date_time'] = df['date_time'].dt.tz_localize('GMT').dt.tz_convert(localizeDictionary[country])

    #replace too big or too small hour values
    hours=df['date_time'].dt.hour
    tooMany=hours > 17
    tooLess=hours < 9
    hours=df['date_time'] = np.where(tooMany, df['date_time'] - pd.to_timedelta(df['date_time'].dt.hour - 17,unit='h'), df['date_time'])
    hours=df['date_time'] = np.where(tooLess, df['date_time'] + pd.to_timedelta(9 - df['date_time'].dt.hour,unit='h'), df['date_time'])

    #numeric importance
    df['importance'] = df['importance'].map(importanceDictionary).fillna(0.00)

    #replacing substrings
    df['actual'] = df['actual'].str.replace('|'.join(replacementDictionary), lambda string: replacementDictionary[string.group()]).fillna('0.0')
    df['previous'] = df['previous'].str.replace('|'.join(replacementDictionary), lambda string: replacementDictionary[string.group()]).fillna('0.0')
    df['forecast'] = df['forecast'].str.replace('|'.join(replacementDictionary), lambda string: replacementDictionary[string.group()]).fillna(df['previous'])
    df["actual"] = pd.to_numeric(df["actual"])
    df["previous"] = pd.to_numeric(df["previous"])
    df["forecast"] = pd.to_numeric(df["forecast"])

    return df

dataDf = getEconomicData('15/01/2021', '31/01/2022', 'united states')



#OK: only full hours
#OK: tz_localize time zone
#OK: combine columns: 'date' + 'time'
#OK: numeric imporance
#OK: replace too big or too small hour values
#todo: deviation dictionary for 'event' column
#todo: for deviation, compute 'previous' - 'actual' difference
#OK: remove nones?
#NO: 'All Day' and 'Tentative' events change to 09:00
#OK: 'All Day' and 'Tentative' events remove



print(dataDf.head(1000))
print(dataDf.dtypes)