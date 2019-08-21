using CSCore.Codecs.WAV;
using CSCore.SoundIn;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Intent;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Timers;

namespace microsoftSpeech
{
    //credits: https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/how-to-recognize-intents-from-speech-csharp
    class Program
    {
        private WasapiCapture _capture;
        private WaveWriter _writer;
        private Timer _recordFileTimer;
        private static readonly string _audioFile = "sample2.wav";

        static void Main()
        {
            Program program = new Program();

            DoStuffAsync(program);
            Console.ReadKey();
        }

        private static async void DoStuffAsync(Program program)
        {
            string result = await ProcessAudioAsync();

            Console.WriteLine("finished");
        }
        public static bool CheckAudioFile()
        {
            if (new FileInfo(_audioFile).Length > 10000)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task<string> ProcessAudioAsync()
        {
            string key = "<KEY>";
            string region = "<REGION>";
            SpeechConfig configRecognizer = SpeechConfig.FromSubscription(key, region);
            string processedAudio = "";

            bool isRecorded = CheckAudioFile();

            if (isRecorded)
            {
                using (AudioConfig audioInput = AudioConfig.FromWavFileInput(_audioFile))
                using (IntentRecognizer recognizer = new IntentRecognizer(configRecognizer, audioInput))
                {
                    TaskCompletionSource<int> stopRecognition = new TaskCompletionSource<int>();

                    recognizer.Recognized += (s, e) =>
                    {

                        if (e.Result.Reason == ResultReason.RecognizedSpeech)
                        {
                            processedAudio = e.Result.Text;
                        }
                    };

                    recognizer.Canceled += (s, e) => {
                        if (e.Reason == CancellationReason.Error)
                        {
                            //log
                        }
                        stopRecognition.TrySetResult(0);
                    };

                    recognizer.SessionStarted += (s, e) => {

                        //log
                    };

                    recognizer.SessionStopped += (s, e) => {

                        //log

                        stopRecognition.TrySetResult(0);
                    };

                    await recognizer.StartContinuousRecognitionAsync();

                    Task.WaitAny(new[] { stopRecognition.Task });

                    await recognizer.StopContinuousRecognitionAsync();
                }

                //log

                return processedAudio;
            }
            else
            {
                //log

                return processedAudio;
            }
        }
    }
}
