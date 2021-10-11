import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from statsmodels.tsa.stattools import adfuller
import statsmodels.api as sm

#https://www.analyticsvidhya.com/blog/2020/10/how-to-create-an-arima-model-for-time-series-forecasting-in-python/

# Import data
df = pd.read_csv('GPW_DLY WIG20, 15.csv', usecols=["close"])
df.reset_index(drop=True)

#Let’s check that if the given dataset is stationary or not, For that we use adfuller.
test_result=adfuller(df['close'])
def adfuller_test(close):
    result=adfuller(close)
    labels = ['ADF Test Statistic','p-value','#Lags Used','Number of Observations']
    for value,label in zip(result,labels):
        print(label+' : '+str(value) )
    if result[1] <= 0.05:
        print("strong evidence against the null hypothesis(Ho), reject the null hypothesis. Data is stationary")
    else:
        print("weak evidence against null hypothesis,indicating it is non-stationary ")

adfuller_test(df['close'])

# For non-seasonal data
#p=1, d=1, q=0 or 1
model=sm.tsa.statespace.ARIMA(df['close'],order=(3,2,1))
model_fit=model.fit()
model_fit.summary()

#future
future_dates=[df.index[-1]+ x for x in range(0,2000)]
future_datest_df=pd.DataFrame(index=future_dates[1:],columns=df.columns)

future_datest_df.tail()

future_df=pd.concat([df,future_datest_df])

future_df['forecast'] = model_fit.predict(start = len(df.index), end = len(df.index) + 2000, dynamic= True)
future_df[['close', 'forecast']].plot(figsize=(12, 8))



plt.show()