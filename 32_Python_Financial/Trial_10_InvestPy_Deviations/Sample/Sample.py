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

#ref: https://www.fxstreet.com/economic-calendar
#2021-01-27 last 'united states'
deviationScoreDictionary = {
    'Factory orders ex transportation (MoM)' : 0.4,
    'Factory Orders (MoM)' : 0.4,
    'NonfarmUnit Labor Costs (QoQ)' : -0.33,
    'Nonfarm Productivity (QoQ)' : 0.45,
    'ISM Non-Manufacturing Prices' : 0.4,
    'ISM Non-Manufacturing PMI' : 0.75,
    'ISM Non-Manufacturing New Orders' : 0.18,
    'ISM Non-Manufacturing Employment' : 0.13,
    'Markit Composite PMI' : 2.5,
    'ADP Nonfarm Employment Change' : 0.004,
    'IBD/TIPP Economic Optimism' : 0.4,
    'Total Vehicle Sales' : 0.5,
    'ISM Manufacturing Prices' : 0.26,
    'ISM Manufacturing PMI' : 0.5,
    'Construction Spending (MoM)' : 1.6,
    'FOMC Member ' : 0,
    'OPEC Crude Oil Production ' : 10,
    'Real Personal Consumption (MoM)' : 10,
    'Personal Spending (MoM)' : 10,
    'Employment Wages (QoQ)' : -10,
    'Employment Cost Index (QoQ)' : -10,
    'Core PCE Price Index (YoY)' : 7,
    'Core PCE Price Index (MoM)' : 12.5,
    'Real Consumer Spending' : 0.1,
    'Goods Trade Balance' : 0.1,
    'Pending Home Sales (MoM)' : -5,
    'Pending Home Sales Index' : -1,
    'New Home Sales' : 0.01,
    'PCE Prices' : 0.7,
    'Core PCE Prices' : 4,
    'MBA Mortgage Applications (WoW)' : 1,
    'MBA 30-Year Mortgage Rate' : -5,
    'Durable Goods Orders (MoM)' : 1,
    'Goods Orders Non Defense Ex Air (MoM)' : 1.75,
    'Durables Excluding Defense (MoM)' : 0.35,
    'Durable Goods Orders (MoM)' : 0.35,
    '30-Year Bond Auction' : 5,
    '10-Year Bond Auction' : 15,
    '20-Year Bond Auction' : 10,
    '20-Year Bond Auction' : 10,
    '5-Year Note Auction' : 15,
    '2-Year Note Auction' : 20,
    '10-Year TIPS Auction' : 10,
    '8-Week Bill Auction' : 40,
    '4-Week Bill Auction' : 50,
    '6-Month Bill Auction' : 25,
    '3-Month Bill Auction' : 30,
    '52-Week Bill Auction' : 20,
    'Texas Services Sector Outlook' : 0.1,
    'Richmond Services Index' : 0.1,
    'Richmond Manufacturing Shipments' : 0.1,
    'Richmond Manufacturing Index' : 0.1,
    'CB Consumer Confidence' : 1,
    'House Price Index' : 0.5,
    'House Price Index (MoM)' : 0.5,
    'House Price Index (YoY)' : 0.5,
    'S&P/CS HPI Composite - 20 s.a. (MoM)' : 1.5,
    'S&P/CS HPI Composite - 20 n.s.a. (YoY)' : 1.5,
    'S&P/CS HPI Composite - 20 n.s.a. (MoM)' : 1.5,
    'CFTC Natural Gas speculative net positions' : -0.05,
    'CFTC Wheat speculative net positions' : 0.05,
    'CFTC Soybeans speculative net positions' : 0.05,
    'CFTC S&P 500 speculative net positions' : 0.05,
    'CFTC Nasdaq 100 speculative net positions' : 0.05,
    'CFTC Gold speculative net positions' : -0.05,
    'CFTC Silver speculative net positions' : -0.1,
    'CFTC Crude Oil speculative net positions' : -0.05,
    'CFTC Corn speculative net positions' : 0.1,
    'CFTC Copper speculative net positions' : 1,
    'CFTC Aluminium speculative net positions' : 1,
    'Gasoline Inventories' : 0.1,
    'Heating Oil Stockpiles' : 0.1,
    'Gasoline Production' : 0.1,
    'EIA Weekly Distillates Stocks' : 0.1,
    'Distillate Fuel Production' : 0.1,
    'Cushing Crude Oil Inventories' : 0.1,
    'Crude Oil Imports' : 0.1,
    'Crude Oil Inventories' : 0.3,
    'API Weekly Crude Oil Stock' : 0.3,
    'Natural Gas Storage' : 0.09,
    'Existing Home Sales (MoM)' : 0.25,
    'Existing Home Sales' : 5.25,
    'Services PMI' : 1,
    'Manufacturing PMI' : 0.72,
    'Philadelphia Fed Manufacturing Index' : 0.1,
    'Philadelphia Fed Manufacturing Index' : 0.1,
    'Jobless Claims 4-Week Avg.' : -0.006,
    'Initial Jobless Claims' : -0.01,
    'Housing Starts' : 10,
    'Housing Starts (MoM)' : 10,
    'Continuing Jobless Claims' : -0.006,
    'Building Permits' : 3.3,
    'Building Permits (MoM)' : 3.3,
    'NAHB Housing Market Index' : 0.33,
    'Treasury Secretary Yellen Speaks' : 0,
    'IEA Monthly Report' : 0,
    'U.S. Baker Hughes Oil Rig Count' : 0.1,
    'U.S. Baker Hughes Total Rig Count' : 0.1,
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
#todo: after finished find huge values with errors
#OK: remove nones?
#NO: 'All Day' and 'Tentative' events change to 09:00
#OK: 'All Day' and 'Tentative' events remove



print(dataDf.head(1000))
print(dataDf.dtypes)