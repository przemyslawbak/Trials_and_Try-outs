#import the pandas library and aliasing as pd
import pandas as pd
import numpy as np
data = np.array(['a','b','c','d'])
s = pd.Series(data)
print(s)

#import the pandas library and aliasing as pd
data = {'a' : 0., 'b' : 1., 'c' : 2.}
s = pd.Series(data)
print(s)

#import the pandas library and aliasing as pd
data = {'a' : 0., 'b' : 1., 'c' : 2.}
s = pd.Series(data,index=['b','c','d','a'])
print(s)

#import the pandas library and aliasing as pd
s = pd.Series(5, index=[0, 1, 2, 3])
print s