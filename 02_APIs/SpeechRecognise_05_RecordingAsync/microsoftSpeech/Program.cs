using CSCore.Codecs.WAV;
using CSCore.SoundIn;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Timers;

namespace microsoftSpeech
{
    class Program
    {
        private WasapiCapture _capture;
        private WaveWriter _writer;
        private Timer _recordFileTimer;
        private readonly string _audioFile = "dump.wav";

        public async Task RecordAudioSampleAsync()
        {
            _recordFileTimer = new Timer();
            _capture = new WasapiLoopbackCapture();

            if (File.Exists(_audioFile))
            {
                File.Delete(_audioFile);
            }


            _recordFileTimer.Elapsed += new ElapsedEventHandler(OnFinishedRecordingEvent);
            _recordFileTimer.Interval = 8000; //recording period

            try
            {
                _capture.Initialize();
                _writer = new WaveWriter(_audioFile, _capture.WaveFormat);
                _capture.DataAvailable += (s, e) =>
                {
                    _writer.Write(e.Data, e.Offset, e.ByteCount); //saving recorded audio sample
                };

                _capture.Start(); //start recording

                _recordFileTimer.Enabled = true;

                while(_recordFileTimer.Enabled)
                {
                    await Task.Delay(100);
                }
            }
            catch
            {
                //log
            }
        }

        private void OnFinishedRecordingEvent(object sender, ElapsedEventArgs e)
        {
            _capture.Stop(); //stop recording

            _recordFileTimer.Enabled = false; //timer disabled

            _writer.Dispose();
            _capture.Dispose();

            Console.WriteLine("now really finished");
        }


        static void Main()
        {
            Program program = new Program();

            DoStuffAsync(program);
            Console.ReadKey();
        }

        private static async void DoStuffAsync(Program program)
        {
            await Task.Run(() => program.RecordAudioSampleAsync());

            Console.WriteLine("finished");
        }
    }
}
