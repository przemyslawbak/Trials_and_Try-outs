import tensorflow as tf
import os
import matplotlib.pyplot as plt

    # Load audio at 44.1kHz sample-rate
def load_audio(file_path, sample_rate=44100):
    audio = tf.io.read_file(file_path)
    audio, sample_rate = tf.audio.decode_wav\
    (audio,\
    desired_channels=-1,\
    desired_samples=sample_rate)
    return tf.transpose(audio)

def apply_mfccs(audio, sample_rate=44100, num_mfccs=13):
    stfts = tf.signal.stft(audio, frame_length=1024, \
    frame_step=256, \
    fft_length=1024)
    spectrograms = tf.abs(stfts)
    num_spectrogram_bins = stfts.shape[-1]#.value
    lower_edge_hertz, upper_edge_hertz, \
    num_mel_bins = 80.0, 7600.0, 80
    linear_to_mel_weight_matrix = \
    tf.signal.linear_to_mel_weight_matrix\
    (num_mel_bins, num_spectrogram_bins, \
    sample_rate, lower_edge_hertz, upper_edge_hertz)
    mel_spectrograms = tf.tensordot\
    (spectrograms, \
    linear_to_mel_weight_matrix, 1)
    mel_spectrograms.set_shape\
    (spectrograms.shape[:-1].concatenate\
    (linear_to_mel_weight_matrix.shape[-1:]))
    log_mel_spectrograms = tf.math.log\
    (mel_spectrograms + 1e-6)
    #Compute MFCCs from log_mel_spectrograms
    mfccs = tf.signal.mfccs_from_log_mel_spectrograms\
    (log_mel_spectrograms)[..., :num_mfccs]
    return mfccs

prefix = "data_speech_commands_v0.02/zero/"
paths = [os.path.join(prefix, path) for path in os.listdir(prefix)]

audio = load_audio(paths[0])
plt.plot(audio.numpy().T)
plt.xlabel('Sample')
plt.ylabel('Value')

mfcc = apply_mfccs(audio)
plt.pcolor(mfcc.numpy()[0])
plt.xlabel('MFCC log coefficient')
plt.ylabel('Sample Value')

AUTOTUNE = tf.data.experimental.AUTOTUNE
def prep_ds(ds, shuffle_buffer_size=1024, \
    batch_size=64):
    # Randomly shuffle (file_path, label) dataset
    ds = ds.shuffle(buffer_size=shuffle_buffer_size)
    # Load and decode audio from file paths
    ds = ds.map(load_audio, num_parallel_calls=AUTOTUNE)
    # generate MFCCs from the audio data
    ds = ds.map(apply_mfccs)
    # Repeat dataset forever
    ds = ds.repeat()
    # Prepare batches
    ds = ds.batch(batch_size)
    # Prefetch
    ds = ds.prefetch(buffer_size=AUTOTUNE)
    return ds

ds = tf.data.Dataset.from_tensor_slices(paths)
train_ds = prep_ds(ds)

for x in train_ds.take(1):\
    print(x)

plt.show()