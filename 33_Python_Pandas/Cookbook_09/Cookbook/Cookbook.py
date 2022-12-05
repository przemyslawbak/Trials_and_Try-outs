import pandas as pd
import numpy as np
flights = pd.read_csv('data/flights.csv')
flights.head()
#Defining an aggregation
(flights
.groupby('AIRLINE')
.agg({'ARR_DELAY':'mean'})
)
#Grouping and aggregating with multiple columns and functions
(flights 
.groupby(['AIRLINE', 'WEEKDAY'])
['CANCELLED']
.agg('sum') #Finding the number of canceled flights for every airline per weekd
)
(flights
.groupby(['AIRLINE', 'WEEKDAY'])
[['CANCELLED', 'DIVERTED']]
.agg(['sum', 'mean']) #Finding the number and percentage of canceled and diverted flights for every airline per weekday
)
(flights
.groupby(['ORG_AIR', 'DEST_AIR'])
.agg({'CANCELLED':['sum', 'mean', 'size'],
'AIR_TIME':['mean', 'var']}) #For each origin and destination, finding the total number of flights, the number and percentage of canceled flights, and the average and variance of the airtime
)
#Removing the MultiIndex after grouping
airline_info = (flights
.groupby(['AIRLINE', 'WEEKDAY'])
.agg({'DIST':['sum', 'mean'],
'ARR_DELAY':['min', 'max']})
.astype(int)
)
airline_info.reset_index()
#Or refactor the code to make it readable. Use the pandas 0.25 functionality to flatten
(flights
.groupby(['AIRLINE', 'WEEKDAY'])
.agg(dist_sum=pd.NamedAgg(column='DIST', aggfunc='sum'),
dist_mean=pd.NamedAgg(column='DIST', aggfunc='mean'),
arr_delay_min=pd.NamedAgg(column='ARR_DELAY',
aggfunc='min'),
arr_delay_max=pd.NamedAgg(column='ARR_DELAY',
aggfunc='max'))
.astype(int)
.reset_index()
)