#https://www.blog.pythonlibrary.org/2016/08/02/python-201-a-multiprocessing-tutorial/

import multiprocessing as mp
import time
import os
from multiprocessing import Process, current_process

fn = 'myfile.txt'

def doubler(q, number):
    odd = False
    """
    A doubling function that can be used by a process
    """
    result = number * 2
    #proc_name = current_process().name
    #print('{0} doubled to {1} by: {2}'.format(number, result, proc_name))

    if number % 2 == 0:
        odd = False
    else:
        odd = True

    return 'doubled ' + str(result), str(odd)

def listener(q, txt):
    '''listens for messages on the q, writes to file. '''

    with open(fn, 'a') as f:
        f.write(txt + '\n')
        f.close()
            
if __name__ == '__main__':
    starttime = time.time()
    numbers = range(0,5000)
    jobs = []

    #mp config:
    manager = mp.Manager()
    q = manager.Queue()
    pool = mp.Pool(mp.cpu_count() + 2)

    #put listener to work first

    for index, number in enumerate(numbers):
        print(str(number))
        job = pool.apply_async(doubler, (q, number))
        jobs.append(job)
        number_str, odd_str = job.get()
        pool.apply_async(listener, (q, number_str + ' ' + odd_str))

    #now we are done, kill the listener
    q.put('kill')
    pool.close()
    pool.join()

    print('That took {} seconds'.format(time.time() - starttime)) #7s