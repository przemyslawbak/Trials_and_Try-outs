import os
import quandl
import seaborn as sns
import matplotlib.pyplot as plt

sns.set_style('whitegrid')

api_key =  'okzqnu3LNwxfwNpw6YUt'
oil = quandl.get('EIA/PET_RWTC_D', api_key=api_key).squeeze()

oil.plot(lw=2, title='WTI Crude Oil Price', figsize=(12, 4))
sns.despine()
plt.tight_layout();
plt.show()