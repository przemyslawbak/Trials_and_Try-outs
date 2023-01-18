import pandas_datareader as pdr

data = pdr.get_data_fred('GS10')

#print(data)

#stooq (no H1)
df = pdr.stooq.StooqDailyReader(symbols='PKN.PL')
df = df.read()
print(df.head())

#more: https://pandas-datareader.readthedocs.io/en/latest/readers/index.html