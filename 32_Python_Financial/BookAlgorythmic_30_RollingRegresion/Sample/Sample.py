import warnings
warnings.filterwarnings('ignore')
from pathlib import Path
import numpy as np
import arviz
import theano
import pymc3 as pm
from sklearn.preprocessing import scale
import yfinance as yf
import matplotlib.pyplot as plt
import seaborn as sns

# model_path = Path('models')
sns.set_style('whitegrid')

size = 200
true_intercept = 1
true_slope = 2

x = np.linspace(0, 1, size)
true_regression_line = true_intercept + true_slope * x
y = true_regression_line + np.random.normal(scale=.5, size=size)

x_shared = theano.shared(x)

with pm.Model() as linear_regression: # model specification

    # Define priors
    sd = pm.HalfCauchy('sigma', beta=10, testval=1) # unique name for each variable
    intercept = pm.Normal('intercept', 0, sd=20)
    slope = pm.Normal('slope', 0, sd=20)

    # Define likelihood
    likelihood = pm.Normal('y', mu=intercept + slope * x_shared, sd=sd, observed=y)

pm.model_to_graphviz(linear_regression)
with linear_regression:
    # Inference
    trace = pm.sample(draws=2500, # draw 2500 samples from posterior using NUTS sampling
                      tune=1000, 
                      cores=1) 

arviz.plot_posterior(trace);
plt.show() #not showing