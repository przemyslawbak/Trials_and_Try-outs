import warnings
warnings.filterwarnings('ignore')
import pandas as pd
import numpy as np
from statsmodels.api import OLS, add_constant
import pandas_datareader.data as web
from linearmodels.asset_pricing import LinearFactorModel
import matplotlib.pyplot as plt
import seaborn as sns