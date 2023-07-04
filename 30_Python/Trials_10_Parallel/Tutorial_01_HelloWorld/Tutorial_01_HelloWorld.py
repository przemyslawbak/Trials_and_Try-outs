from joblib import Parallel, delayed
import numpy as np

def fn(x):
    for i in np.linspace(0, x, 1000):
        a = x
        b = 2*x
        with open(str(x)+"_file.csv", 'w') as file:
            file.write(str(b))

        return a, b

ans = Parallel(n_jobs=-1)(delayed(fn)(x) for x in np.linspace(0,5,50))