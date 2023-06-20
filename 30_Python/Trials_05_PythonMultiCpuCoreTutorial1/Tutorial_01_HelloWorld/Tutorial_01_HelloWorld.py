#https://www.blog.pythonlibrary.org/2016/08/02/python-201-a-multiprocessing-tutorial/

import time
import os
from multiprocessing import Process, current_process

def doubler(number):
    """
    A doubling function that can be used by a process
    """
    result = number * 2
    proc_name = current_process().name
    print('{0} doubled to {1} by: {2}'.format(
        number, result, proc_name))
if __name__ == '__main__':
    starttime = time.time()
    numbers = range(0,100)
    procs = []
    
    for index, number in enumerate(numbers):
        proc = Process(target=doubler, args=(number,))
        procs.append(proc) #Then in the block of code at the bottom, we create a series of Processes and start them
        proc.start()
    for proc in procs:
        proc.join() #The very last loop just calls the join() method on each process, which tells Python to wait for the process to terminate

    print('That took {} seconds'.format(time.time() - starttime)) #7s