#https://stackoverflow.com/a/69166303
#https://investpy.readthedocs.io/_api/news.html
#https://www.investing.com/economic-calendar/

import investpy
import pandas as pd
pd.set_option('display.max_rows', 1000)
pd.set_option('display.max_columns', 10)
pd.set_option('display.width', 1000)

def getEconomicData(from_date, to_date, country):
    timeZoneDictionary = {
        'poland':'GMT +1:00',
        'united states':'GMT -5:00',
        'germany':'GMT +1:00',
        'euro zone':'GMT +1:00',
        'japan':'GMT +9:00',
        }
    return investpy.economic_calendar(
    from_date=from_date,
    to_date  =to_date,
    countries=[country],
    time_zone = timeZoneDictionary[country],
    )

dataDf = getEconomicData('15/01/2021', '31/01/2022', 'poland')



#todo: only full hours
#todo: combine columns: 'date' + 'time'
#todo: numeric imporance
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