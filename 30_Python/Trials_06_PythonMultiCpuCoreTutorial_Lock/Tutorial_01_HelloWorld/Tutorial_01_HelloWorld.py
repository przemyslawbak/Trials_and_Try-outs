#https://www.blog.pythonlibrary.org/2016/08/02/python-201-a-multiprocessing-tutorial/

import time
import os
from multiprocessing import Process, Lock

#using Lock

def printer(item, lock):
    """
    Prints out the item that was passed in
    """
    lock.acquire()
    try:
        print(str(item))
    finally:
        lock.release()
if __name__ == '__main__':
    starttime = time.time()
    lock = Lock()
    procs = []
    items = range(0,100)
    for item in items:
        p = Process(target=printer, args=(item, lock))
        procs.append(p) #Then in the block of code at the bottom, we create a series of Processes and start them
        p.start()
    for proc in procs:
        proc.join() #The very last loop just calls the join() method on each process, which tells Python to wait for the process to terminate

    print('That took {} seconds'.format(time.time() - starttime)) #8s