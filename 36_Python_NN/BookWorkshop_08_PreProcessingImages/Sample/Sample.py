import tensorflow as tf
import pandas as pd
import matplotlib.pyplot as plt
from tensorflow.keras.preprocessing.image import ImageDataGenerator
import matplotlib.pyplot as plt

#rescaling images:
#datagenerator = ImageDataGenerator(rescale = 1./255)

train_datagen = ImageDataGenerator(rescale = 1./255)
training_set = train_datagen.flow_from_directory\
('image_data', target_size = (64, 64), batch_size = 25, class_mode = 'binary')
def show_batch(image_batch, label_batch):
    lookup = {v: k for k, v in  training_set.class_indices.items()}
    label_batch = [lookup[label] for label in  label_batch]
    plt.figure(figsize=(10,10))
    for n in range(25):
        ax = plt.subplot(5,5,n+1)
        plt.imshow(image_batch[n])
        plt.title(label_batch[n].title())
        plt.axis('off')