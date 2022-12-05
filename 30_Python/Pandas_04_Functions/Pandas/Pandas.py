import pandas as pd
import numpy as np

#axes
s = pd.Series(np.random.randn(4))
print ("The axes are:")
print(s.axes)
#size
print(s.size)
#values
print(s.values)
#first rows
print(s.head(2))
#last rows
print(s.tail(2))
#dimention
print(s.ndim)

#check for empty
s = pd.Series(np.random.randn(4))
print ("Is the Object empty?")
print(s.empty)

#dimemtions
s = pd.Series(np.random.randn(4))
print(s)

print ("The dimensions of the object:")
print(s.ndim)

#Transpose - The rows and columns will interchange
# Create a Dictionary of series
d = {'Name':pd.Series(['Tom','James','Ricky','Vin','Steve','Smith','Jack']),
   'Age':pd.Series([25,26,25,23,30,29,23]),
   'Rating':pd.Series([4.23,3.24,3.98,2.56,3.20,4.6,3.8])}

# Create a DataFrame
df = pd.DataFrame(d)
print ("The transpose of the data series is:")
print(df.T)