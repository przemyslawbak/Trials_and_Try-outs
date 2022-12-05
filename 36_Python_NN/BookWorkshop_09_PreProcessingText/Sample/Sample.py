import tensorflow as tf
import tensorflow_hub as hub

#pretrained model
#   model_url = "tf2-preview_gnews-swivel-20dim_1.tar.gz"
#loading model
#   hub_layer = hub.KerasLayer(model_url, \
#   input_shape=[], dtype=tf.string, \
#   trainable=True)
#The data type of the input data, indicated by the dtype parameter, should be
#used as input for the KerasLayer class, as well as a Boolean argument indicating
#whether the weights are trainabl
#   hub_layer(data)

#exercise
df = tf.data.experimental.make_csv_dataset\
('drugsComTest_raw.tsv', \
batch_size=1, field_delim='\t')
def prep_ds(ds, shuffle_buffer_size=1024, \
batch_size=32):

    # Shuffle the dataset
    ds = ds.shuffle(buffer_size=shuffle_buffer_size)
    # Repeat the dataset
    ds = ds.repeat()
    # Batch the dataset
    ds = ds.batch(batch_size)
    return ds

ds = prep_ds(df, batch_size=5)
#for x in ds.take(1):\
#    print(x)    

#loading pretrained model
print('embedding...')
embedding = "https://tfhub.dev/google/tf2-preview/gnews-swivel-20dim/1"
print('processing...')
hub_layer = hub.KerasLayer(embedding, input_shape=[], \
dtype=tf.string, \
trainable=True)

for x in ds.take(1):\
    print(hub_layer(tf.reshape(x['review'],[-1])))      