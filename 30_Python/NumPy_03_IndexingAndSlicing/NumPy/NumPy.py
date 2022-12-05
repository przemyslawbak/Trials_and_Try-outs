#Three types of indexing methods are available − field access, basic slicing and advanced indexing.
#https://www.tutorialspoint.com/numpy/numpy_indexing_and_slicing.htm

#Basic slicing is an extension of Python's basic concept of slicing to n dimensions
a = np.arange(10) 
s = slice(2,7,2) 
print(a[s])

# slice single item
a = np.arange(10) 
b = a[5] 
print(b)

# slice items starting from index 
a = np.arange(10) 
print(a[2:])