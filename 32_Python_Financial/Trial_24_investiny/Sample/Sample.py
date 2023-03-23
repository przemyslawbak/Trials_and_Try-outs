from investiny import historical_data
import warnings
import pandas as pd

warnings.filterwarnings("ignore")
pd.set_option('display.max_rows', 20000)
pd.set_option('display.max_columns', 10)
pd.set_option('display.width', 1000)

#docs: https://github.com/alvarobartt/investiny

#403 forbidden
data = historical_data(investing_id=6408, from_date="09/01/2022", to_date="10/01/2022") # Returns AAPL historical data as JSON (without date)



print(data.tail(1000))