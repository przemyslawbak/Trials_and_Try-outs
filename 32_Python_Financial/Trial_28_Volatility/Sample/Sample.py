## Computing Volatility
# Load the required modules and packages

import numpy as np
import pandas as pd
import yfinance as yf
import matplotlib.pyplot as plt

#https://www.w3schools.com/python/pandas/pandas_plotting.asp

NIFTY = yf.download('^GDAXI',start='2023-4-1', end='2023-4-8', interval = '1m')

# Compute the logarithmic returns using the Closing price
NIFTY['Log_Ret'] = np.log(NIFTY['Close'] / NIFTY['Close'].shift(1))

# Compute Volatility using the pandas rolling standard deviation function
NIFTY['Volatility'] = NIFTY['Log_Ret'].rolling(window=50).std() * np.sqrt(252)
print(NIFTY.tail(15))

# Plot the NIFTY Price series and the Volatility
NIFTY[['Close', 'Volatility']].plot(subplots=True, color='blue',figsize=(8, 6))
plt.show()