#https://stackoverflow.com/a/69166303
#https://investpy.readthedocs.io/_api/news.html
#https://www.investing.com/economic-calendar/

import investpy
import pandas as pd
import numpy as np
pd.set_option('display.max_rows', 10000)
pd.set_option('display.max_columns', 10)
pd.set_option('display.width', 1000)

localizeDictionary = {
    'poland':'Europe/Berlin',
    'united states':'US/Eastern',
    'germany':'Europe/Berlin',
    'euro zone':'Europe/Berlin',
    'japan':'Japan',
    'united kingdom':'Europe/London',
    }

importanceValueDictionary = {
    'low' : 0.15,
    'medium' : 0.50,
    'high' : 1.00,
    }

eventMonths = ['  \(Jun\)', '  \(Jul\)', '  \(Aug\)', '  \(Sep\)', '  \(Oct\)', '  \(Nov\)', '  \(Dec\)', '  \(Jan\)', '  \(Feb\)', '  \(Mar\)', '  \(Apr\)', '  \(May\)']

replacementDictionary = {"B": "", "M": "", "%": "", ",": "", "K": "", "T": ""}

importanceEventDictionary = {
    #max 0.3x
    'GDP' : 1.00,
    'Interest Rate' : 1.00,
    'Unemployment Rate' : 1.00,
    #max 0.2x
    'Manufacturing PMI' : 0.50,
    'CPI' : 0.50,
    'Retail Sales' : 0.50,
    'Industrial Production' : 0.50,
    'Services PMI' : 0.50,
    'Trade Balance' : 0.50,
    '10-Year' : 0.50,
    #max 0.1x
    'Money Supply' : 0.15,
    'Thomson Reuters IPSOS PCSI' : 0.15,
    'PPI' : 0.15,
    '30-Year' : 0.15,
    'Current Account' : 0.15,
    }

deviationScoreDictionaryUk = {
    'GDP \(QoQ\)' : 0.5,
    'BoE Interest Rate Decision' : -12.5,
    'Manufacturing PMI' : 0.1,
    'Core CPI \(YoY\)' : -2,
    'Retail Sales \(YoY\)' : 0.1,
    'Unemployment Rate' : -2,
    'Industrial Production \(MoM\)' : 0.2,
    'Services PMI' : 0.2,
    'Trade Balance' : 0.05,
    '10-Year Treasury Gilt Auction' : -5,
    'M4 Money Supply' : 0.0002,
    'Thomson Reuters IPSOS PCSI' : 0.1,
    'Core PPI Output \(MoM\)' : 0.1,
    '30-Year Treasury Gilt Auction' : -2,
    'Current Account' : 0.02,
    }

deviationScoreDictionaryJp = {
    'GDP \(QoQ\)' : 0.5,
    'BoJ Core CPI \(YoY\)' : -2,
    'Retail Sales \(YoY\)' : 0.1,
    'Unemployment Rate' : -2,
    'Industrial Production \(MoM\)' : 0.1,
    'M3 Money Supply' : 0.02,
    'BoJ Interest Rate Decision' : -12.5,
    'Services PMI' : 0.2,
    'Adjusted Trade Balance' : 1,
    'Thomson Reuters IPSOS PCSI' : 0.1,
    'PPI \(YoY\)' : 0.1,
    'Manufacturing PMI' : 0.1,
    '30-Year JGB Auction' : -2,
    '10-Year JGB Auction' : -5,
    'Current Account n.s.a.' : 0.5,
    }

deviationScoreDictionaryEu = {
    'Retail Sales \(MoM\)' : 0.1,
    'Unemployment Rate' : -2,
    'Industrial Production \(MoM\)' : 0.2,
    'M3 Money Supply \(YoY\)' : 0.0002,
    'Interest Rate Decision' : -12.5,
    'Services PMI' : 0.1,
    'Trade Balance' : 0.03,
    'PPI \(YoY\)' : 0.1,
    'Manufacturing PMI' : 0.2,
    'Current Account n.s.a.' : 0.02,
    }

deviationScoreDictionaryPl = {
    'GDP \(QoQ\)' : 0.25,
    'Core CPI \(YoY\)' : -2,
    'Retail Sales \(YoY\)' : 0.1,
    'Unemployment Rate' : -4,
    'M3 Money Supply \(MoM\)' : 0.1,
    'Interest Rate Decision' : -1.25,
    'PPI \(YoY\)' : 0.3,
    'Manufacturing PMI' : 0.2,
    'Current Account' : 0.0005,
    }

deviationScoreDictionaryDe = {
    'German GDP \(QoQ' : 0.25,
    'German CPI \(YoY\)' : -1,
    'German Retail Sales \(YoY\)' : 0.05,
    'German Unemployment Rate' : -2,
    'German Industrial Production \(MoM\)' : 0.02,
    'German Services PMI' : 0.1,
    'German Trade Balance' : 0.05,
    'Germany Thomson Reuters IPSOS PCSI' : 0.1,
    'German PPI \(MoM\)' : 0.2,
    'German Manufacturing PMI' : 0.2,
    'German 30-Year Bund' : -1,
    'German 10-Year Bund' : -3,
    'Current Account' : 0.02,
    }

deviationScoreDictionaryUs = {
    'GDP \(QoQ\)' : 0.05,
    'Core CPI \(YoY\)' : -0.5,
    'Core Retail Sales \(MoM\)' : 0.075,
    'U6 Unemployment Rate' : -0.4,
    'Industrial Production \(MoM\)' : 0.15,
    'U.S. M2 Money Supply' : 0.2,
    'Fed Interest Rate Decision' : -18.75,
    'Services PMI' : 0.075,
    'Goods Trade Balance' : 0.0375,
    'Thomson Reuters IPSOS PCSI' : 0.12,
    'Core PPI \(MoM\)' : 0.5,
    'ISM Manufacturing PMI' : 0.1,
    '30-Year Bond Auction' : -1,
    '10-Year Bond Auction' : -5,
    'Current Account' : 0.05,
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

    #drop unwanted columns
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
    df['importance'] = df['event']
    df['importance'].replace(importanceEventDictionary, regex=True, inplace=True)
    df["importance"] = pd.to_numeric(df["importance"], errors='coerce')
    df = df[df['importance'].notna()]

    #replacing substrings
    df['actual'] = df['actual'].str.replace('|'.join(replacementDictionary), lambda string: replacementDictionary[string.group()]).fillna('0.0')
    df['previous'] = df['previous'].str.replace('|'.join(replacementDictionary), lambda string: replacementDictionary[string.group()]).fillna('0.0')
    df['forecast'] = df['forecast'].str.replace('|'.join(replacementDictionary), lambda string: replacementDictionary[string.group()]).fillna(df['previous'])
    df["actual"] = pd.to_numeric(df["actual"])
    df["previous"] = pd.to_numeric(df["previous"])
    df["forecast"] = pd.to_numeric(df["forecast"])

    return df

def computeDeviations(df, dictionary):
    #set events from dictionary
    df['event'] = df['event'].str.replace('|'.join(eventMonths), '')
    df['eventName'] = df['event']
    df['event'].replace(dictionary, regex=True, inplace=True)
    df["event"] = pd.to_numeric(df["event"], errors='coerce')
    df = df[df['event'].notna()]

    #compute diff columns
    df['diffPrev'] = (df['actual'] - df['previous']) / 4
    df['diffForec'] = df['actual'] - df['forecast']

    #compute deviation
    df['deviation'] = ((df['diffPrev'] + df['diffForec']) * df['event'] * df['importance']).round(2)
    df['happening'] = np.where(df['deviation'] == 0.00, 1, 0)
    df['happening'] = df['happening'] * df['importance']

    #drop unwanted columns
    df.drop(['diffPrev', 'diffForec', 'event', 'forecast', 'actual', 'previous'], axis=1, inplace=True)


    return df.sort_values(['eventName', 'importance', 'deviation']) #todo: remove .sort_values

dataDfJp = getEconomicData('01/01/2021', '31/01/2022', 'united states')
dataDfJp = computeDeviations(dataDfJp, deviationScoreDictionaryUs)

#OK: create own importance weights based on key words
#OK: compare weights for starndard indicators for all countries, ex. PPI
#todo: add happenings?
#todo: compute sum of n last rows for the result
#todo: find peaks for specific event names



print(dataDfJp.head(10000))
print(dataDfJp.dtypes)