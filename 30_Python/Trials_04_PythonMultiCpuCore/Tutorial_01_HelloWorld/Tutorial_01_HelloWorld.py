#https://urban-institute.medium.com/using-multiprocessing-to-make-python-code-faster-23ea5ef996ba

#import time

#def basic_func(x):
#    if x == 0:
#        return 'zero'
#    elif x%2 == 0:
#        return 'even'
#    else:
#        return 'odd'
    
#starttime = time.time()
#for i in range(0,10):
#    y = i*i
#    time.sleep(2)
#    print('{} squared results in a/an {} number'.format(i, basic_func(y)))
    
#print('That took {} seconds'.format(time.time() - starttime)) #over 20s

import time
import multiprocessing 

def basic_func(x):
    if x == 0:
        return 'zero'
    elif x%2 == 0:
        return 'even'
    else:
        return 'odd'

def multiprocessing_func(x):
    y = x*x
    time.sleep(2)
    print('{} squared results in a/an {} number'.format(x, basic_func(y)))
    
if __name__ == '__main__':
    starttime = time.time()
    processes = []
    for i in range(0,10):
        p = multiprocessing.Process(target=multiprocessing_func, args=(i,))
        processes.append(p)
        p.start()
        
    for process in processes:
        process.join()
        
    print('That took {} seconds'.format(time.time() - starttime)) #over 2s