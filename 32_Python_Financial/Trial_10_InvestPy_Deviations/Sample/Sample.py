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
    }

importanceDictionary = {
    'low' : 0.15,
    'medium' : 0.50,
    'high' : 1.00,
    }

months = ['  \(Jun\)', '  \(Jul\)', '  \(Aug\)', '  \(Sep\)', '  \(Oct\)', '  \(Nov\)', '  \(Dec\)', '  \(Jan\)', '  \(Feb\)', '  \(Mar\)', '  \(Apr\)', '  \(May\)']

replacementDictionary = {"B": "", "M": "", "%": "", ",": "", "K": "", "T": ""}

#ref: https://www.fxstreet.com/economic-calendar

deviationScoreDictionaryJp = {
    'Monetary Base \(YoY\)' : 0.1,
    'Tankan Large Manufacturers Index' : 0.02,
    'Tankan Big Manufacturing Outlook Index' : 0.1,
    'Tankan All Small Industry CAPEX' : 0.1,
    'Tankan All Big Industry CAPEX' : 0.1,
    'BSI Large Manufacturing Conditions' : 0.02,
    'Capacity Utilization Rate' : 3,
    'GDP \(QoQ\)' : 0.5,
    'Core Machinery Orders \(YoY\)' : 0.01,
    'Machine Tool Orders \(YoY\)' : 0.01,
    'Foreign Reserves \(USD\)' : 0.01,
    'Housing Starts \(YoY\)' : 0.1,
    'Unemployment Rate' : -2,
    'Jobs/applications ratio' : 5,
    'Retail Sales \(YoY\)' : 0.1,
    'Coincident Indicator \(MoM\)' : 0.1,
    'Corporate Services Price Index \(CSPI\) \(YoY\)' : -1.42,
    'BoJ Core CPI \(YoY\)' : -2,
    'BoJ Summary of Opinions' : 0,
    'Speaks' : 0,
    'Monetary Policy Meeting Minutes' : 0,
    'BoJ Press Conference' : 0,
    'BoJ Outlook Report \(YoY\)' : 0,
    'BoJ Monetary Policy Statement' : 0,
    'BoJ Interest Rate Decision' : -12.5,
    'Imports \(YoY\)' : -0.02,
    'Exports \(YoY\) ' : 0.02,
    'Adjusted Trade Balance' : 1,
    'Industrial Production \(MoM\)' : 0.1,
    'Tertiary Industry Activity Index \(MoM\)' : 0.15,
    'Thomson Reuters IPSOS PCSI' : 0.1,
    'Foreign Bonds Buying' : -0.0001,
    'PPI \(YoY\)' : 0.1,
    'M3 Money Supply' : 0.02,
    'Economy Watchers Current Index' : 0.0005,
    'Current Account n.s.a.' : 0.5,
    'Bank Lending \(YoY\)' : 0.2,
    'Leading Index \(MoM\)' : 0.2,
    '30-Year JGB Auction' : -2,
    'Household Spending \(MoM\)' : 0.1,
    'Foreign Investments in Japanese Stocks' : 0.0005,
    'Average Cash Earnings \(YoY\)' : 0.1,
    'Household Confidence' : 0.1,
    '10-Year JGB Auction' : -5,
    'Services PMI' : 0.2,
    'CFTC JPY speculative net positions' : 0.01,
    'Manufacturing PMI' : 0.1,
    }

deviationScoreDictionaryEu = {
    'ZEW Economic Sentiment' : 0.025,
    'ECB LTRO' : 0.002,
    'Wages in euro zone \(YoY\)' : 0.1,
    'Labor Cost Index \(YoY\)' : -0.33,
    'Deposit Facility Rate' : -12.5,
    'Current Account n.s.a.' : 0.02,
    'Eurogroup Meetings' : 0.1,
    'Trade Balance' : 0.03,
    'Industrial Production \(MoM\)' : 0.2,
    'Unemployment Rate' : -2,
    'Retail Sales \(YoY\)' : 0.1,
    'Sentix Investor Confidence' : 0.05,
    'Industrial Sentiment' : 0.1,
    'Services Sentiment' : 0.1,
    'Consumer Confidence' : 0.1,
    'Business and Consumer Survey' : 0.05,
    'ECB Economic Bulletin' : 7.5,
    'PPI \(YoY\)' : 0.1,
    'Services PMI' : 0.1,
    'Markit Composite PMI' : 0.8,
    'Manufacturing PMI' : 0.2,
    'Private Sector Loans \(YoY\)' : 2,
    'Loans to Non Financial Corporations' : 0.2,
    'M3 Money Supply \(YoY\)' : 0.0002,
    'EU Economic Forecasts' : 0,
    'ECB Press Conference' : 0,
    'ECB Financial Stability Review' : 0,
    'Summit' : 0,
    'Meeting' : 0,
    'ECB Monetary Policy Statement' : 0,
    'Speaks' : 0,
    'CFTC EUR speculative net positions' : 0.0075,
    }

deviationScoreDictionaryPl = {
    'GDP \(QoQ\)' : 0.25,
    'Manufacturing PMI' : 0.2,
    'Unemployment Rate' : -4,
    'M3 Money Supply \(MoM\)' : 0.1,
    'Retail Sales \(YoY\)' : 0.1,
    'PPI \(YoY\)' : 0.3,
    'Industrial Output \(YoY\)' : 0.05,
    'Corp. Sector Wages \(YoY\)' : 0.35,
    'Core CPI \(YoY\)' : -2,
    'Current Account' : 0.0005,
    'Interest Rate Decision' : -1.25,
    'NBP Monetary Policy Meeting Mi' : 0,
    }

deviationScoreDictionaryDe = {
    'German 12-Month Bubill' : -0.5,
    'German Import Price Index \(MoM\)' : 0.06,
    'German Export Price Index \(MoM\)' : 0.25,
    'GfK German Consumer Climate' : 0.05,
    'German Buba' : 0,
    'German Ifo Business Climate Index' : 0.2,
    'German Current Assessment' : 0.2,
    'German Business Expectations' : 0.1,
    'German 30-Year Bund' : -1,
    'German PPI \(YoY\)' : 0.2,
    'German ZEW Economic Sentiment' : 0.08,
    'German ZEW Current Conditions' : 0.02,
    'German Car Registration \(MoM\)' : 0.5,
    'Germany Thomson Reuters IPSOS PCSI' : 0.1,
    'German GDP \(QoQ' : 0.25,
    'German 5-Year Bobl' : -3,
    'German Trade Balance' : 0.05,
    'German Industrial Production \(MoM\)' : 0.02,
    'German Imports \(MoM\)' : -0.1,
    'German Exports \(MoM\)' : 0.1,
    'German Factory Orders \(MoM\)' : 0.05,
    'German CPI \(MoM\)' : -1,
    'German 10-Year Bund' : -3,
    'German Services PMI' : 0.1,
    'German Composite PMI' : 0.2,
    'German 2-Year Schatz' : -10,
    'German Unemployment Rate' : -2,
    'German Manufacturing PMI' : 0.2,
    'German Retail Sales \(MoM\)' : 0.05,
    }

deviationScoreDictionaryUs = {
    'Federal Budget Balance' : 0.006,
    'Loan Officer Survey' : 0,
    'Beige Book' : 0,
    'IMF Meetings' : 0,
    'OPEC Meeting' : 0,
    'Current Account' : 0.05,
    'Fed Interest Rate Decision' : -12.5,
    'Thomson Reuters IPSOS PCSI' : 0.1,
    'All Truck Sales' : 1,
    'All Car Sales' : 1,
    'ISM NY Business Conditions' : 0.5,
    'IBD/TIPP Economic Optimism' : 0.5,
    'U.S. M2 Money Supply' : 0.2,
    'Chicago PMI' : 0.06,
    'KC Fed Manufacturing Index' : 0.06,
    'Cap Goods Ship Non Defense Ex Air \(MoM\)' : 0.1,
    'Chicago Fed National Activity' : 0.5,
    'Dallas Fed Mfg Business Index' : 0.04,
    'MBA Delinquency Rates \(QoQ\)' : -5,
    'Challenger Job Cuts \(YoY\)' : -0.005,
    'Durable Goods Orders \(MoM\)' : 0.02,
    'Redbook \(YoY\)' : 0.25,
    'Michigan 5-Year Inflation Expectations' : -5,
    'Michigan Consumer Sentiment' : 0.02,
    'Import Price Index \(MoM\)' : 3.7,
    'Export Price Index \(MoM\)' : 0.5,
    'Wholesale Trade Sales \(MoM\)' : 0.5,
    'Wholesale Inventories \(MoM\)' : -2.5,
    'Core CPI \(MoM\)' : -2,
    'WASDE Report' : 0,
    'JOLTs Job Openings' : 0.3,
    'NFIB Small Business Optimism' : 0.2,
    'CB Employment Trends Index' : 0.2,
    'Consumer Credit' : 0.05,
    'Unemployment Rate' : -2,
    'Private Nonfarm Payrolls' : 0.001,
    'Imports' : -0.02,
    'Exports' : 0.02,
    'Average Weekly Hours' : 3,
    'Average Hourly Earnings \(MoM\)' : 0.3,
    'Factory Orders \(MoM\)' : 0.5,
    'Unit Labor Costs \(QoQ\)' : -0.15,
    'Nonfarm Productivity \(QoQ\)' : 0.2,
    'ISM Non-Manufacturing Prices' : 0.1,
    'ISM Non-Manufacturing PMI' : 0.02,
    'ISM Non-Manufacturing New Orders' : 0.01,
    'ISM Non-Manufacturing Employment' : 0.05,
    'Markit Composite PMI' : 0.1,
    'ADP Nonfarm Employment Change' : 0.0004,
    'IBD/TIPP Economic Optimism' : 0.2,
    'Total Vehicle Sales' : 0.5,
    'ISM Manufacturing Prices' : 0.1,
    'ISM Manufacturing PMI' : 0.1,
    'Construction Spending \(MoM\)' : 0.8,
    'FOMC Member ' : 0,
    'OPEC Crude Oil Production ' : 1,
    'Real Personal Consumption \(MoM\)' : 0.25,
    'Personal Spending \(MoM\)' : 0.2,
    'Employment Wages \(QoQ\)' : 0.05,
    'Employment Cost Index \(QoQ\)' : -1,
    'Core PCE Price Index \(MoM\)' : 1,
    'Real Consumer Spending' : 0.1,
    'Trade Balance' : 0.1,
    'Pending Home Sales \(MoM\)' : -0.05,
    'New Home Sales \(MoM\)' : 0.01,
    'MBA Mortgage Applications \(WoW\)' : 0.05,
    'MBA 30-Year Mortgage Rate' : -5,
    'Goods Orders Non Defense Ex Air \(MoM\)' : 0.5,
    'Durable Goods Orders \(MoM\)' : 0.1,
    '30-Year Bond Auction' : -2,
    '10-Year Bond Auction' : -5,
    '20-Year Bond Auction' : -2,
    '7-Year Note Auction' : -1,
    '5-Year Note Auction' : -3,
    '3-Year Note Auction' : -4,
    '2-Year Note Auction' : -5,
    '10-Year TIPS Auction' : -1,
    '8-Week Bill Auction' : -20,
    '4-Week Bill Auction' : -10,
    '6-Month Bill Auction' : -20,
    '3-Month Bill Auction' : -15,
    '52-Week Bill Auction' : -10,
    'Texas Services Sector Outlook' : 0.1,
    'Richmond Services Index' : 1,
    'Richmond Manufacturing Shipments' : 1,
    'Richmond Manufacturing Index' : 1,
    'CB Consumer Confidence' : 0.02,
    'House Price Index \(MoM\)' : 0.5,
    'S&P/CS HPI Composite - 20 n.s.a. \(MoM\)' : 1.5,
    'CFTC Natural Gas speculative net positions' : -0.01,
    'CFTC Wheat speculative net positions' : 0.01,
    'CFTC Soybeans speculative net positions' : 0.01,
    'CFTC S&P 500 speculative net positions' : 0.01,
    'CFTC Nasdaq 100 speculative net positions' : 0.01,
    'CFTC Gold speculative net positions' : -0.005,
    'CFTC Silver speculative net positions' : -0.1,
    'CFTC Crude Oil speculative net positions' : -0.01,
    'CFTC Corn speculative net positions' : 0.02,
    'CFTC Copper speculative net positions' : 0.05,
    'CFTC Aluminium speculative net positions' : 0.5,
    'Gasoline Inventories' : 0.1,
    'Heating Oil Stockpiles' : 0.5,
    'Gasoline Production' : 0.2,
    'Distillate Fuel Production' : 1,
    'Crude Oil Imports' : 1,
    'Crude Oil Inventories' : 0.01,
    'API Weekly Crude Oil Stock' : 0.03,
    'Natural Gas Storage' : 0.02,
    'Existing Home Sales \(MoM\)' : 0.0525,
    'Services PMI' : 0.1,
    'Manufacturing PMI' : 0.2,
    'Philadelphia Fed Manufacturing Index' : 0.01,
    'Jobless Claims 4-Week Avg.' : -0.006,
    'Initial Jobless Claims' : -0.001,
    'Housing Starts \(MoM\)' : 0.02,
    'Continuing Jobless Claims' : -0.006,
    'Building Permits \(MoM\)' : 0.03,
    'NAHB Housing Market Index' : 0.33,
    'Treasury Secretary Yellen Speaks' : 0,
    'OPEC Monthly Report' : 0,
    'IEA Monthly Report' : 0,
    'U.S. Baker Hughes Total Rig Count' : 0.05,
    'Business Inventories \(MoM\)' : 2,
    'Manufacturing Production \(MoM\)' : 0.2,
    'Capacity Utilization Rate' : 0.5,
    'Industrial Production \(MoM\)' : 0.1,
    'Retail Sales \(MoM\)' : 0.1,
    'Retail Inventories Ex Auto' : 0.5,
    'Retail Control \(MoM\)' : 0.1,
    'PPI \(MoM\)' : 0.5,
    'NY Empire State Manufacturing Index' : 0.014,
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

def computeDeviations(df, dictionary):
    df['event'] = df['event'].str.replace('|'.join(months), '')
    df['eventName'] = df['event']
    df['event'].replace(dictionary, regex=True, inplace=True)
    df["event"] = pd.to_numeric(df["event"], errors='coerce')
    df = df[df['event'].notna()]
    df['diffPrev'] = (df['actual'] - df['previous']) / 4
    df['diffForec'] = df['actual'] - df['forecast']
    df['deviation'] = ((df['diffPrev'] + df['diffForec']) * df['event'] * df['importance']).round(2)
    df.drop(['diffPrev', 'diffForec', 'event', 'forecast', 'actual', 'previous'], axis=1, inplace=True)
    df['happening'] = np.where(df['deviation'] == 0.00, 1, 0)
    df['happening'] = df['happening'] * df['importance']

    return df.sort_values(by=['eventName']).sort_values(by=['importance']) #todo: remove .sort_values

dataDfJp = getEconomicData('01/01/2021', '31/01/2022', 'united states')
dataDfJp = computeDeviations(dataDfJp, deviationScoreDictionaryUs)


#OK: only full hours
#OK: tz_localize time zone
#OK: combine columns: 'date' + 'time'
#OK: numeric imporance
#OK: replace too big or too small hour values
#OK: deviation dictionary for 'event' column
#OK: for deviation, compute 'previous' - 'actual' difference
#NO: double check update values for deviationScoreDictionaryUs
#OK: remove months from event names
#todo: compare weights for starndard indicators for all countries, ex. PPI
#OK: after finished find huge values with errors, if any
#todo: compute sum of n last rows for the result
#NO: UK?
#OK: remove nones?
#NO: 'All Day' and 'Tentative' events change to 09:00
#OK: 'All Day' and 'Tentative' events remove



print(dataDfJp.head(10000))
print(dataDfJp.dtypes)