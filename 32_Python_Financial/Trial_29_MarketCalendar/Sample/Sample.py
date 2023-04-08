import pandas_market_calendars as mcal

# Create a calendar (XWAR -> GPW)
xwar = mcal.get_calendar('XWAR')

early = xwar.schedule(start_date='2022-07-01', end_date='2023-07-10')
print(early)