import pandas_datareader as pdr

#data = pdr.get_data_fred('GS10')

#print(data)

#for options no data and I have no fuckin idea why
df = pdr.stooq.StooqDailyReader(symbols='OW20B231850.PL')
df = df.read()
print(df.head())

#more: https://pandas-datareader.readthedocs.io/en/latest/readers/index.html