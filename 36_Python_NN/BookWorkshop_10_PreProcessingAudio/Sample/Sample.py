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

prefix = "data_speech_commands_v0.02/zero/"
paths = [os.path.join(prefix, path) for path in os.listdir(prefix)]

audio = load_audio(paths[0])
plt.plot(audio.numpy().T)
plt.xlabel('Sample')
plt.ylabel('Value')

plt.show()