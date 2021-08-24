#collection of items of the same type. Items in the collection can be accessed using a zero-based index
#https://www.tutorialspoint.com/numpy/numpy_ndarray_object.htm

import numpy as np

a = np.array([1,2,3])
print(a)

b = np.array([1, 2, 3,4,5], ndmin = 2)
print(b)

#various array attributes of NumPy
#https://www.tutorialspoint.com/numpy/numpy_array_attributes.htm

#This array attribute returns a tuple consisting of array dimensions. It can also be used to resize the array.
a = np.array([[1,2,3],[4,5,6]]) 
print(a.shape)

# this resizes the ndarray
a = np.array([[1,2,3],[4,5,6]]) 
a.shape = (3,2) 
print(a)

#NumPy also provides a reshape function to resize an array.
a = np.array([[1,2,3],[4,5,6]]) 
b = a.reshape(3,2) 
print(b)

# an array of evenly spaced numbers
a = np.arange(24) 
print(a)

# this is one dimensional array 
import numpy as np 
a = np.arange(24) 
a.ndim 
# now reshape it 
b = a.reshape(2,4,3) 
print(a)
# b is having three dimensions

#The following example shows the current values of flags
x = np.array([1,2,3,4,5]) 
print(x.flags)

#NumPy - Array Creation Routines
#https://www.tutorialspoint.com/numpy/numpy_array_creation_routines.htm

#It creates an uninitialized array of specified shape and dtype
x = np.empty([3,2], dtype = int) 
print(x)

# array of five zeros. Default dtype is float 
x = np.zeros(5) 
print(x)

x = np.zeros((5,), dtype = np.int) 
print(x)

# custom type
x = np.zeros((2,2), dtype = [('x', 'i4'), ('y', 'i4')])  
print(x)

# array of five ones. Default dtype is float
x = np.ones(5) 
print(x)