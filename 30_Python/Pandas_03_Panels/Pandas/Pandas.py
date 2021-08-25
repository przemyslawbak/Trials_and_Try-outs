#A panel is a 3D container of data. The term Panel data is derived from econometrics and is partially responsible for the name pandas − pan(el)-da(ta)-s.
#https://www.tutorialspoint.com/python_pandas/python_pandas_panel.htm

# creating an empty panel
import pandas as pd
import numpy as np
data = {'Item1' : pd.DataFrame(np.random.randn(4, 3)), 
   'Item2' : pd.DataFrame(np.random.randn(4, 2))}
p = pd.Panel(data) #error https://stackoverflow.com/a/65600307
print(p.minor_xs(1))

# creating an empty panel
data = {'Item1' : pd.DataFrame(np.random.randn(4, 3)), 
   'Item2' : pd.DataFrame(np.random.randn(4, 2))}
p = pd.Panel(data) #error https://stackoverflow.com/a/65600307
print(p['Item1'])