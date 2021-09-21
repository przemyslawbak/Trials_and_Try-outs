import warnings
warnings.filterwarnings('ignore')
from pathlib import Path
import pickle
import pandas as pd
import numpy as np
from scipy import stats
import pandas_datareader.data as web
from sklearn.preprocessing import scale
from sklearn.model_selection import train_test_split
from sklearn.feature_selection import mutual_info_classif
from sklearn.metrics import roc_auc_score
import theano
import pymc3 as pm
import arviz
from pymc3.variational.callbacks import CheckParametersConvergence
import statsmodels.formula.api as smf
import matplotlib.pyplot as plt
from matplotlib import animation
import seaborn as sns

print('test')