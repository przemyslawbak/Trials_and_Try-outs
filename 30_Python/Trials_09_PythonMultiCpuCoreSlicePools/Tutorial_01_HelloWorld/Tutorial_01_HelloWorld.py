#https://stackoverflow.com/a/66555259

from multiprocessing.pool import Pool, ThreadPool
from multiprocessing import cpu_count
import pandas as pd

def worker(process_pool, index, split_range):
    out = process_pool.map(myfunction, range(*split_range))
    pd.DataFrame(out).to_csv(f'filename.csv', header=False, index=False)
    print('saved...........')

def myfunction(x): # toy example function
    print(str(x))
    return(x ** 2)

def split(start, stop, n):
    k, m = divmod(stop - start, n)
    return [(i * k + min(i, m),(i + 1) * k + min(i + 1, m)) for i in range(n)]

def main():
    RANGE_START = 0
    RANGE_STOP = 10000
    ITEMS_IN_SPIT = 100
    N_SPLITS = int(abs(RANGE_STOP / ITEMS_IN_SPIT))
    n_processes = min(N_SPLITS, cpu_count())
    split_ranges = split(RANGE_START, RANGE_STOP, N_SPLITS) # [(0, 100), (100, 200), ... (400, 500)]
    process_pool = Pool(n_processes)
    thread_pool = ThreadPool(N_SPLITS)
    for index, split_range in enumerate(split_ranges):
        thread_pool.apply_async(worker, args=(process_pool, index, split_range))
    # wait for all threading tasks to complete:
    thread_pool.close()
    thread_pool.join()

# required for Windows:
if __name__ == '__main__':
    main()