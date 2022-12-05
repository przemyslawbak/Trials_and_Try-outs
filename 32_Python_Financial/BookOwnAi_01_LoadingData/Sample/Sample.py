import pandas as pd # Importing modules for use.
import numpy as np
import matplotlib.pyplot as plt # For plotting scatter plot

data = pd.read_csv('Altman_Z_2D.csv') # Load the .csv data
print(data) # Taking a look at the data.
print(data.dtypes) # Taking a look at the data.

# Bankruptcy mask (list of booleans)
bankrupt_mask = (data['Bankrupt'] == True)
# Plot the bankrupt points
plt.scatter(data['EBIT/Total Assets'][bankrupt_mask],\
data['MktValEquity/Debt'][bankrupt_mask],\
marker='x')
# Plot the nonbankrupt points
plt.scatter(data['EBIT/Total Assets'][~bankrupt_mask],\
data['MktValEquity/Debt'][~bankrupt_mask],\
marker='o')
# Formatting
plt.xlabel('EBIT/Total Assets')
plt.ylabel('MktValEquity/Debt')
plt.grid()
plt.legend(['Bankrupt','Non bankrupt'])
plt.show()