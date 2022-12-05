import numpy as np
from keras.models import Sequential
from keras.layers import Embedding

model = Sequential()
model.add(Embedding(5, 2, input_length=5))

input_array = np.random.randint(5, size=(1, 5))

model.compile('rmsprop', 'mse')
output_array = model.predict(input_array)

print('input')
print(input_array)
#[[2 3 0 0 3]]
print('output')
print(output_array)
#[[[ 0.01566761 -0.0035669 ]
  #[ 0.00366595  0.02057955]
  #[ 0.04810288  0.01606523]
  #[ 0.04810288  0.01606523]
  #[ 0.00366595  0.02057955]]]