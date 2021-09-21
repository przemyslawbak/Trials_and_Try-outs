import warnings
warnings.filterwarnings('ignore')
from time import time
from pathlib import Path
from tqdm import tqdm 
import numpy as np
from numpy.linalg import LinAlgError
import pandas as pd
from sklearn.model_selection import StratifiedKFold, GridSearchCV
from sklearn.metrics import confusion_matrix
from sklearn.tree import  DecisionTreeClassifier
from sklearn.linear_model import LogisticRegressionCV
from statsmodels.tsa.stattools import adfuller, coint
from statsmodels.tsa.vector_ar.vecm import coint_johansen
from statsmodels.tsa.api import VAR
import matplotlib.pyplot as plt
import seaborn as sns

pd.set_option('display.float_format', lambda x: f'{x:,.2f}')
DATA_PATH = Path('..', 'data')
STORE = DATA_PATH / 'assets.h5'

