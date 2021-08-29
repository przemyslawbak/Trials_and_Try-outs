
import warnings
warnings.filterwarnings('ignore')
import gzip
import shutil
from struct import unpack
from collections import namedtuple, Counter, defaultdict
from pathlib import Path
from urllib.request import urlretrieve
from urllib.parse import urljoin
from datetime import timedelta
from time import time
import pandas as pd
import matplotlib.pyplot as plt
from matplotlib.ticker import FuncFormatter
import seaborn as sns
import ftpClient as client

sns.set_style('whitegrid')

def format_time(t):
    """Return a formatted time string 'HH:MM:SS
    based on a numeric time() value"""
    m, s = divmod(t, 60)
    h, m = divmod(m, 60)
    return f'{h:0>2.0f}:{m:0>2.0f}:{s:0>5.2f}'

data_path = Path('data') # set to e.g. external harddrive
itch_store = str(data_path / 'itch.h5')
order_book_store = data_path / 'order_book.h5'

FTP_URL = 'emi.nasdaq.com'
FTP_DIR = '/ITCH/Nasdaq ITCH'
SOURCE_FILE = '10302019.NASDAQ_ITCH50.gz'

def may_be_download(url, file, dir):
    """Download & unzip ITCH data if not yet available"""
    if not data_path.exists():
        print('Creating directory')
        data_path.mkdir()
    else: 
        print('Directory exists')

    filename = data_path / file      
    if not filename.exists():
        print('Downloading...', url)
        obj = client.PyFTPclient(url)
        obj.DownloadFile(dir + '/' + file, filename)
    else: 
        print('File exists')        

    unzipped = data_path / (filename.stem + '.bin')
    if not unzipped.exists():
        print('Unzipping to', unzipped)
        with gzip.open(str(filename), 'rb') as f_in:
            with open(unzipped, 'wb') as f_out:
                shutil.copyfileobj(f_in, f_out)
    else: 
        print('File already unpacked')
    return unzipped

file_name = may_be_download(FTP_URL, SOURCE_FILE, FTP_DIR)
date = file_name.name.split('.')[0]