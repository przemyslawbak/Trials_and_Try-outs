import pandas as pd # Importing modules for use.
import numpy as np
import matplotlib.pyplot as plt # For plotting scatter plot

data = pd.read_csv('Altman_Z_2D.csv') # Load the .csv data
print(data) # Taking a look at the data.
print(data.dtypes) # Taking a look at the data.

# Bankruptcy mask (list of booleans)
bankrupt_mask = (data['Bankrupt'] == True)
# Plot the bankrupt points
plt.scatter(data['EBIT/Total Assets'][bankrupt_mask],\
data['MktValEquity/Debt'][bankrupt_mask],\
marker='x')
# Plot the nonbankrupt points
plt.scatter(data['EBIT/Total Assets'][~bankrupt_mask],\
data['MktValEquity/Debt'][~bankrupt_mask],\
marker='o')
# Formatting
plt.xlabel('EBIT/Total Assets')
plt.ylabel('MktValEquity/Debt')
plt.grid()
plt.legend(['Bankrupt','Non bankrupt'])
# Split up the data for the classifier to be trained.
# X is data
# Y is the answer we want our classifier to replicate.
X = data[['EBIT/Total Assets','MktValEquity/Debt']]
Y = data['Bankrupt']
# Import Scikit-Learn
from sklearn.tree import DecisionTreeClassifier
# Create a DecisionTreeClassifier object first
tree_clf = DecisionTreeClassifier(max_depth=2)
# Fit the Decision Tree to our training data of X and Y.
tree_clf.fit(X, Y)
# Let's see if it predicts bankruptcy for a bad company
print('Low EBIT/Total Assets and MktValEquity/Debt company go bust?', tree_clf.predict([[-20,
-10]]))
# Let's try this for a highly values, high earning company
print('High EBIT/Total Assets and MktValEquity/Debt company go bust?', tree_clf.predict([[20,
10]]))
# Contour plot.
from matplotlib.colors import ListedColormap
x1s = np.linspace(-30, 40, 100)
x2s = np.linspace(-10, 15, 100)
x1, x2 = np.meshgrid(x1s, x2s)
X_new = np.c_[x1.ravel(), x2.ravel()]
y_pred = tree_clf.predict(X_new).astype(int).reshape(x1.shape)
custom_cmap = ListedColormap(['#2F939F','#D609A8'])
plt.contourf(x1, x2, y_pred, alpha=0.3, cmap=custom_cmap)
plt.plot(X['EBIT/Total Assets'][Y==False],\
X['MktValEquity/Debt'][Y==False], "bo",
X['EBIT/Total Assets'][Y==True],\
X['MktValEquity/Debt'][Y==True], "rx")
plt.xlabel('EBIT/Total Assets')
plt.ylabel('MktValEquity/Debt')

# Test/train splitting
from sklearn.model_selection import train_test_split # need to import
X_train, X_test, y_train, y_test = train_test_split(X, Y, test_size=0.33, random_state=42)
# Have a look at the train and test sets.
print('X_train:', X_train.shape)
print('X_test:', X_test.shape)
print('y_train:', y_train.shape)
print('y_test:', y_test.shape)

# Cross validation
from sklearn.model_selection import cross_val_score
scores = cross_val_score(tree_clf2, X, Y, cv=3, scoring="accuracy")
print('Accuracy scores for the 3 models are:', scores)

from sklearn.model_selection import ShuffleSplit
from sklearn.model_selection import cross_val_score
scores = cross_val_score(tree_clf2,
X, Y,
cv=ShuffleSplit(n_splits=5,
random_state=42,
test_size=0.25,
train_size=None),
scoring="accuracy")
print(scores) # See scores

from sklearn.model_selection import cross_val_predict
# First get the predictions of Trues and Falses
scores = cross_val_predict(tree_clf2, X, Y, cv=3)
from sklearn.metrics import confusion_matrix
# Compare these predictions with the known correct answers
confusion_matrix(scores, Y)




plt.show()