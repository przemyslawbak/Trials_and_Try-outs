#https://stackoverflow.com/a/69166303
#https://investpy.readthedocs.io/_api/news.html
#https://www.investing.com/economic-calendar/

import investpy

data = investpy.economic_calendar(
    from_date='12/09/2021',
    to_date  ='13/10/2021'
)
print(data)