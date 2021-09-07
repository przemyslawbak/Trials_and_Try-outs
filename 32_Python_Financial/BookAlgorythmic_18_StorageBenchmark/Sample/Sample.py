import warnings
warnings.filterwarnings('ignore')
from pathlib import Path
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import seaborn as sns
import random
import string

sns.set_style('whitegrid')
results = {}
data_type = 'Numeric'

#Generate Test Data
def generate_test_data(nrows=10000, numerical_cols=1000, text_cols=0, text_length=10):
    s = "".join([random.choice(string.ascii_letters)
                 for _ in range(text_length)])
    data = pd.concat([pd.DataFrame(np.random.random(size=(nrows, numerical_cols))), pd.DataFrame(np.full(shape=(nrows, text_cols), fill_value=s))], axis=1, ignore_index=True)
    data.columns = [str(i) for i in data.columns]
    return data

df = generate_test_data(numerical_cols=1000, text_cols=1000)
df.info()

#Parquet
parquet_file = Path('test.parquet')
df.to_parquet(parquet_file) #A suitable version of pyarrow or fastparquet is required for parquet support
size = parquet_file.stat().st_size
#Reading Parquet
df = pd.read_parquet(parquet_file)
read = _
parquet_file.unlink()
#Writing Parquet
df.to_parquet(parquet_file)
parquet_file.unlink()
write = _
#Results Parquet
results['Parquet'] = {'read': np.mean(read.all_runs), 'write': np.mean(write.all_runs), 'size': size}

#exception: _ not defined, can not continue