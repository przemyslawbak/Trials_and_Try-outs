#Fast Fourier Transform
#https://www.tutorialspoint.com/scipy/scipy_fftpack.htm

#Importing the fft and inverse fft functions from fftpackage
from scipy.fftpack import fft

#create an array with random n numbers
x = np.array([1.0, 2.0, 1.0, -1.0, 1.5])

#Applying the fft function
y = fft(x)
print(y)

#Discrete Cosine Transform
from scipy.fftpack import dct
print(dct(np.array([4., 3., 5., 10., 5., 3.])))