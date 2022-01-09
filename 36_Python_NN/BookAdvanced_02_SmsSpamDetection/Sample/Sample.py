import tensorflow as tf
import os
import io
import pandas as pd
import re

# Download the zip file
path_to_zip = tf.keras.utils.get_file('smsspamcollection.zip', origin='https://archive.ics.uci.edu/ml/machine-learning-databases/00228/smsspamcollection.zip', extract=True)
print('dupa')

# Let's see if we read the data correctly
lines = io.open(path_to_zip.replace("smsspamcollection.zip", "SMSSpamCollection")).read().strip().split('\n')
print(lines[0])

#Pre-Process Data
spam_dataset = []
count = 0

for line in lines:
  label, text = line.split('\t')
  if label.lower().strip() == 'spam':
    spam_dataset.append((1, text.strip()))
    count += 1
  else:
    spam_dataset.append(((0, text.strip())))

#Data Normalization
df = pd.DataFrame(spam_dataset, columns=['Spam', 'Message'])
def message_length(x):
    # returns total number of characters
    return len(x)           

def num_capitals(x):
  _, count = re.subn(r'[A-Z]', '', x) # only works in english
  return count

def num_punctuation(x):
  _, count = re.subn(r'\W', '', x)
  return count

df['Capitals'] = df['Message'].apply(num_capitals)
df['Punctuation'] = df['Message'].apply(num_punctuation)
df['Length'] = df['Message'].apply(message_length)

train=df.sample(frac=0.8,random_state=42) #random state is a seed value
test=df.drop(train.index)

#Model Building
# Basic 1-layer neural network model for evaluation
def make_model(input_dims=3, num_units=12):
  model = tf.keras.Sequential()

  # Adds a densely-connected layer with 12 units to the model:
  model.add(tf.keras.layers.Dense(num_units, 
                                  input_dim=input_dims, 
                                  activation='relu'))

  # Add a sigmoid layer with a binary output unit:
  model.add(tf.keras.layers.Dense(1, activation='sigmoid'))
  model.compile(loss='binary_crossentropy', optimizer='adam', 
                metrics=['accuracy'])
  return model

x_train = train[['Length', 'Punctuation', 'Capitals']]
y_train = train[['Spam']]

x_test = test[['Length', 'Punctuation', 'Capitals']]
y_test = test[['Spam']]

model = make_model()
model.fit(x_train, y_train, epochs=10, batch_size=10)
model.evaluate(x_test, y_test)
y_train_pred = model.predict_classes(x_train)
# confusion matrix
tf.math.confusion_matrix(tf.constant(y_train.Spam), y_train_pred)