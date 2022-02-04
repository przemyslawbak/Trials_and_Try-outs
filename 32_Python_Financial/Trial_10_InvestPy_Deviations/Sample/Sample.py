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
    'medium' : 0.40,
    'high' : 1.00,
    }

replacementDictionary = {"B": "", "M": "", "%": "", ",": "", "K": "", "T": ""}

#ref: https://www.fxstreet.com/economic-calendar

deviationScoreDictionaryJp = {
    'BSI Large Manufacturing Conditions' : 0.04,
    'Capacity Utilization Rate' : 3,
    'GDP (QoQ)' : 5,
    'Core Machinery Orders (YoY)' : 0.4,
    'Machine Tool Orders (YoY)' : 5,
    'Foreign Reserves (USD)' : 0.01,
    'Housing Starts (YoY)' : 10,
    'Unemployment Rate' : 0.6,
    'Jobs/applications ratio' : -2.66,
    'Retail Sales (YoY)' : 5,
    'Coincident Indicator (MoM)' : 0.8,
    'Corporate Services Price Index (CSPI) (YoY)' : -1.42,
    'BoJ Core CPI (YoY)' : -12.5,
    'BoJ Summary of Opinions' : 0,
    'Speaks' : 0,
    'Monetary Policy Meeting Minutes' : 0,
    'BoJ Press Conference' : 0,
    'BoJ Outlook Report (YoY)' : 0,
    'BoJ Monetary Policy Statement' : 0,
    'BoJ Interest Rate Decision' : -12.5,
    'Imports (YoY)' : -0.1,
    'Exports (YoY) ' : 0.1,
    'Adjusted Trade Balance' : 1,
    'Industrial Production (MoM)' : 0.2,
    'Tertiary Industry Activity Index (MoM)' : 0.37,
    'Thomson Reuters IPSOS PCSI' : 1,
    'Foreign Bonds Buying' : -0.005,
    'PPI (YoY)' : 7.5,
    'M3 Money Supply' : 1.4,
    'Economy Watchers Current Index' : 0.12,
    'Current Account n.s.a.' : 5,
    'Bank Lending (YoY)' : 1.6,
    'Leading Index (MoM)' : 0.6,
    '30-Year JGB Auction' : -5,
    'Household Spending (MoM)' : 0.3,
    'Foreign Investments in Japanese Stocks' : 0.005,
    'Average Cash Earnings (YoY)' : 1.1,
    'Household Confidence' : 1,
    '10-Year JGB Auction' : -11,
    'Services PMI' : 1,
    'CFTC JPY speculative net positions' : 0.05,
    'Manufacturing PMI' : 0.5,
    }

deviationScoreDictionaryEu = {
    'ZEW Economic Sentiment' : 0.4,
    'ECB LTRO' : 0.05,
    'Wages in euro zone (YoY)' : 2.33,
    'Labor Cost Index (YoY)' : -0.33,
    'Deposit Facility Rate' : -12.5,
    'Current Account n.s.a.' : 0.2,
    'Eurogroup Meetings' : 0.1,
    'Trade Balance' : 0.1,
    'Industrial Production (MoM)' : 2,
    'Unemployment Rate' : 0.6,
    'Retail Sales (YoY)' : 5,
    'Sentix Investor Confidence' : 1,
    'Industrial Sentiment' : 1,
    'Services Sentiment' : 0.3,
    'Consumer Confidence' : 1,
    'Business and Consumer Survey' : 1.7,
    'ECB Economic Bulletin' : 7.5,
    'PPI (YoY)' : 7.5,
    'Services PMI' : 1,
    'Markit Composite PMI' : 2.5,
    'Manufacturing PMI' : 0.5,
    'Private Sector Loans (YoY)' : 12.5,
    'Loans to Non Financial Corporations' : 12.5,
    'M3 Money Supply (YoY)' : 1.4,
    'EU Economic Forecasts' : 0,
    'ECB Press Conference' : 0,
    'ECB Financial Stability Review' : 0,
    'Summit' : 0,
    'Meeting' : 0,
    'ECB Monetary Policy Statement' : 0,
    'Speaks' : 0,
    'CFTC EUR speculative net positions' : 0.05,
    }

deviationScoreDictionaryPl = {
    'GDP (QoQ)' : 5,
    'Manufacturing PMI' : 0.5,
    'Unemployment Rate' : -2,
    'M3 Money Supply (MoM)' : 1.4,
    'Retail Sales (YoY)' : 5,
    'PPI (YoY)' : 7.5,
    'Industrial Output (YoY)' : 0.7,
    'Employment Growth (YoY)' : 5,
    'Corp. Sector Wages (YoY)' : 2.33,
    'Core CPI (YoY)' : -12.5,
    'Current Account' : 0.002,
    'Interest Rate Decision' : -12.5,
    'NBP Monetary Policy Meeting Mi' : 0,
    }

deviationScoreDictionaryDe = {
    'German 12-Month Bubill' : -20,
    'German Import Price Index (MoM)' : 3.7,
    'German Export Price Index (MoM)' : 0.5,
    'GfK German Consumer Climate' : 1,
    'German Buba' : 0,
    'German Ifo Business Climate Index' : 1,
    'German Current Assessment' : 1,
    'German Business Expectations' : 1.5,
    'German 30-Year Bund' : -5,
    'German PPI (YoY)' : 7.5,
    'German ZEW Economic Sentiment' : 0.4,
    'German ZEW Current Conditions' : 0.4,
    'German Car Registration (MoM)' : 5,
    'Germany Thomson Reuters IPSOS PCSI' : 1,
    'German GDP (QoQ' : 5,
    'German 5-Year Bobl' : -15,
    'German Trade Balance' : 0.1,
    'German Industrial Production (MoM)' : 2,
    'German Imports (MoM)' : -0.1,
    'German Exports (MoM)' : 0.1,
    'German Factory Orders (MoM)' : 0.4,
    'German CPI (MoM)' : -12.5,
    'German 10-Year Bund' : -11,
    'German Services PMI' : 1,
    'German Composite PMI' : 2.5,
    'German 2-Year Schatz' : -20,
    'German Unemployment Rate' : -2,
    'German Manufacturing PMI' : 0.86,
    'German Retail Sales (MoM)' : 5,
    }

deviationScoreDictionaryUs = {
    'Federal Budget Balance' : 0.006,
    'Loan Officer Survey' : 0,
    'Beige Book' : 0,
    'IMF Meetings' : 0,
    'OPEC Meeting' : 0,
    'Current Account' : 0.15,
    'Fed Interest Rate Decision' : -12.5,
    'Thomson Reuters IPSOS PCSI' : 1,
    'All Truck Sales' : 5,
    'All Car Sales' : 5,
    'ISM NY Business Conditions' : 0.5,
    'IBD/TIPP Economic Optimism' : 0.5,
    'U.S. M2 Money Supply' : -12.5,
    'Chicago PMI' : 0.3,
    'KC Fed Manufacturing Index' : 0.06,
    'Cap Goods Ship Non Defense Ex Air (MoM)' : 2,
    'Chicago Fed National Activity' : 1.22,
    'Dallas Fed Mfg Business Index' : 0.02,
    'MBA Delinquency Rates (QoQ)' : -5,
    'Challenger Job Cuts (YoY)' : -0.05,
    'Durable Goods Orders (MoM)' : 5,
    'Redbook (YoY)' : 5,
    'Michigan 5-Year Inflation Expectations' : -5,
    'Michigan Consumer Sentiment' : 0.5,
    'Import Price Index (MoM)' : 3.7,
    'Export Price Index (MoM)' : 0.5,
    'Wholesale Trade Sales (MoM)' : 5,
    'Wholesale Inventories (MoM)' : -5,
    'Core CPI (MoM)' : -12.5,
    'WASDE Report' : 0,
    'JOLTs Job Openings' : 2.66,
    'NFIB Small Business Optimism' : 0.2,
    'CB Employment Trends Index' : 0.1,
    'Consumer Credit' : 0.1,
    'Unemployment Rate' : -2,
    'Private Nonfarm Payrolls' : 0.01,
    'Imports' : -0.1,
    'Exports' : 0.1,
    'Average Weekly Hours' : 12.5,
    'Average Hourly Earnings (MoM)' : 3.6,
    'Factory Orders (MoM)' : 0.4,
    'Unit Labor Costs (QoQ)' : -0.33,
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
    'Real Personal Consumption (MoM)' : 5,
    'Personal Spending (MoM)' : 5,
    'Employment Wages (QoQ)' : 2.33,
    'Employment Cost Index (QoQ)' : -10,
    'Core PCE Price Index (MoM)' : 12.5,
    'Real Consumer Spending' : 0.1,
    'Trade Balance' : 0.1,
    'Pending Home Sales (MoM)' : -5,
    'New Home Sales (MoM)' : 0.01,
    'MBA Mortgage Applications (WoW)' : 1,
    'MBA 30-Year Mortgage Rate' : -5,
    'Goods Orders Non Defense Ex Air (MoM)' : 1.75,
    'Durable Goods Orders (MoM)' : 0.35,
    '30-Year Bond Auction' : -5,
    '10-Year Bond Auction' : -11,
    '20-Year Bond Auction' : -8,
    '7-Year Note Auction' : -13,
    '5-Year Note Auction' : -15,
    '3-Year Note Auction' : -17,
    '2-Year Note Auction' : -20,
    '10-Year TIPS Auction' : -10,
    '8-Week Bill Auction' : -40,
    '4-Week Bill Auction' : -50,
    '6-Month Bill Auction' : -25,
    '3-Month Bill Auction' : -30,
    '52-Week Bill Auction' : -20,
    'Texas Services Sector Outlook' : 0.1,
    'Richmond Services Index' : 0.1,
    'Richmond Manufacturing Shipments' : 0.1,
    'Richmond Manufacturing Index' : 0.1,
    'CB Consumer Confidence' : 1,
    'House Price Index (MoM)' : 0.5,
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
    'Distillate Fuel Production' : 0.1,
    'Crude Oil Imports' : 0.1,
    'Crude Oil Inventories' : 0.3,
    'API Weekly Crude Oil Stock' : 0.3,
    'Natural Gas Storage' : 0.09,
    'Existing Home Sales (MoM)' : 5.25,
    'Services PMI' : 1,
    'Manufacturing PMI' : 0.72,
    'Philadelphia Fed Manufacturing Index' : 0.1,
    'Jobless Claims 4-Week Avg.' : -0.006,
    'Initial Jobless Claims' : -0.01,
    'Housing Starts (MoM)' : 10,
    'Continuing Jobless Claims' : -0.006,
    'Building Permits (MoM)' : 3.3,
    'NAHB Housing Market Index' : 0.33,
    'Treasury Secretary Yellen Speaks' : 0,
    'OPEC Monthly Report' : 0,
    'IEA Monthly Report' : 0,
    'U.S. Baker Hughes Total Rig Count' : 0.1,
    'Business Inventories (MoM)' : 2,
    'Manufacturing Production (MoM)' : 2,
    'Capacity Utilization Rate' : 3,
    'Industrial Production (MoM)' : 2,
    'Retail Sales (MoM)' : 5,
    'Retail Inventories Ex Auto' : 5,
    'Retail Control (MoM)' : 1.33,
    'PPI (MoM)' : 7.5,
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

dataDf = getEconomicData('01/01/2021', '31/01/2022', 'japan')



#OK: only full hours
#OK: tz_localize time zone
#OK: combine columns: 'date' + 'time'
#OK: numeric imporance
#OK: replace too big or too small hour values
#todo: deviation dictionary for 'event' column
#todo: for deviation, compute 'previous' - 'actual' difference
#todo: after finished find huge values with errors
#todo: UK?
#OK: remove nones?
#NO: 'All Day' and 'Tentative' events change to 09:00
#OK: 'All Day' and 'Tentative' events remove



print(dataDf.head(10000))
print(dataDf.dtypes)