import numpy as np

#1) reshape
a = np.arange(9)
a = np.reshape(a, (3, 3))
print(a)
b = np.arange(10, 19)
b = np.reshape(b, (3, 3))
print(b)
c = np.append(b, a)
print(c)
c = np.reshape(c, (6, 3))
print(c)

#2) without initial reshaping
x = np.arange(9)
y = np.arange(10, 19)
z = np.append(y, x)
print(z)
print(np.array_equal(c,z))  #False
print(np.array_equiv(c,z))  #False
z = np.reshape(z, (6, 3))
print(z)

print(np.array_equal(c,z))  #True
print(np.array_equiv(c,z))  #True