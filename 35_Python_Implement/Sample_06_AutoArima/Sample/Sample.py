import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from statsmodels.tsa.stattools import adfuller
import statsmodels.api as sm
from pmdarima.arima import auto_arima

futures = 40

#https://www.analyticsvidhya.com/blog/2020/10/how-to-create-an-arima-model-for-time-series-forecasting-in-python/

# Import data
df = pd.read_csv('GPW_DLY WIG20, 15.csv', usecols=["close"])
df.reset_index(drop=True)

# For non-seasonal data
#p=1, d=1, q=0 or 1
model=sm.tsa.statespace.SARIMAX(df['close'],order=(3,2,1))
print(model)
model_fit=model.fit()
print(model_fit.summary())

#best 1,1,4
arima_model = auto_arima(df, start_p=1, start_q=1,
                           max_p=4, max_d=5, max_q=5, m=1,
                           start_P=0, seasonal=False,
                           d=1, D=1, trace=True,
                           error_action='ignore',  
                           suppress_warnings=True, 
                           stepwise=False)

#future
future_dates=[df.index[-1]+ x for x in range(0,futures)]
future_datest_df=pd.DataFrame(index=future_dates[1:],columns=df.columns)

future_datest_df.tail()

future_df=pd.concat([df,future_datest_df])

future_df['forecast'] = model_fit.predict(start = len(df.index), end = len(df.index) + futures, dynamic= True)
future_df[['close', 'forecast']].plot(figsize=(12, 8))



plt.show()