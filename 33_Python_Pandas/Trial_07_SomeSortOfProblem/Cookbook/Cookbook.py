import pandas as pd
import signals_model
from os import listdir
from os.path import isfile, join

pd.set_option('display.max_rows', 1000)
pd.set_option('display.max_columns', 10)
pd.set_option('display.width', 1000)

models_accuracies_path = 'models_accuracies.csv'

#sorting models accuracies
dfAcc = pd.read_csv(models_accuracies_path, sep='|')
modelFilePaths = []
modelFilePaths.append(dfAcc['file_name'].iloc[0])
dfAcc = dfAcc[(dfAcc['val_acc'] >= 0.85) & (dfAcc['acc'] >= 0.85)]
dfAcc = dfAcc[(dfAcc['val_loss'] <= 0.01) & (dfAcc['loss'] <= 0.01)]
dfAcc = dfAcc[(dfAcc['val_mse'] <= 0.01) & (dfAcc['mse'] <= 0.01)]
dfAcc = dfAcc.sort_values(['val_mse', 'mse', 'val_acc', 'acc', 'val_loss', 'loss'], ascending=[True, True, False, False, True, True])
rng = 10
if dfAcc.shape[0] < rng:
    rng = dfAcc.shape[0]
for index, row in dfAcc.iterrows():
    modelFilePaths.append(row['file_name'])
modelFilePaths = list(set(modelFilePaths))