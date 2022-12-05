#https://stackoverflow.com/a/69166303
#https://investpy.readthedocs.io/_api/news.html
#https://www.investing.com/economic-calendar/

import investpy
import pandas as pd
pd.set_option('display.max_rows', 1000)
pd.set_option('display.max_columns', 10)
pd.set_option('display.width', 1000)

data = investpy.economic_calendar(
    from_date='15/01/2022',
    to_date  ='31/01/2022',
    countries=['poland', 'germany', 'united states', 'japan'],
    time_zone = 'GMT -5:00',
)
print(data.head(1000))