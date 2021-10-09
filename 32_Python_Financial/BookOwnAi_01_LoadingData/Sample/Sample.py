import pandas as pd # Importing modules for use.
import numpy as np
import matplotlib.pyplot as plt # For plotting scatter plot
data = pd.read_csv('Altman_Z_2D.csv', index_col=0) # Load the .csv data
print(data.head(5)) # Taking a look at the data.