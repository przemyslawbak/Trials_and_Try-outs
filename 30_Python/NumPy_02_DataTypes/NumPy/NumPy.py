#NumPy supports a much greater variety of numerical types than Python does
#https://www.tutorialspoint.com/numpy/numpy_data_types.htm

import numpy as nu

# using array-scalar type
dt = np.dtype(np.int32) 
print(dt)

#int8, int16, int32, int64 can be replaced by equivalent string 'i1', 'i2','i4', etc.
dt = np.dtype('i4')
print(dt)

# using endian notation
dt = np.dtype('>i4') 
print(dt)

# first create structured data type
dt = np.dtype([('age',np.int8)]) 
print(dt)

# now apply it to ndarray object
dt = np.dtype([('age',np.int8)]) 
a = np.array([(10,),(20,),(30,)], dtype = dt) 
print(a)

# file name can be used to access content of age column
dt = np.dtype([('age',np.int8)]) 
a = np.array([(10,),(20,),(30,)], dtype = dt) 
print (a['age'])