using CSCore.Codecs.WAV;
using CSCore.SoundIn;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Intent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace microsoftSpeech
{
    class Program
    {
        private string przetworzoneAudio;
        private WasapiCapture capture;
        private Timer aTimer;
        private WaveWriter writer;
        public void CSCoreAudioRecording()
        {
            using (capture = new WasapiLoopbackCapture())
            {
                aTimer = new Timer();
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent); //jak czas minie, wyłącz nagrywanie
                aTimer.Interval = 8000; //czas nagrywania

                //inicjalizacja urządzenia do nagrywania
                capture.Initialize();

                using (writer = new WaveWriter("dump.wav", capture.WaveFormat))
                {
                    capture.DataAvailable += (s, e) =>
                    {
                        //save the recorded audio
                        writer.Write(e.Data, e.Offset, e.ByteCount);
                    };
                    //start recording
                    capture.Start();
                    aTimer.Enabled = true;
                    Console.WriteLine("Rozpoczęto nagrywanie.");
                    Console.ReadKey();
                }
            }
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            //stop recording
            capture.Stop();
            aTimer.Enabled = false;
            writer.Dispose();
            capture.Dispose();

            Console.WriteLine("Zakończono nagrywanie.");
        }


        static void Main()
        {
            Program program = new Program();
            //program.CSCoreAudioRecording();
            //program.FromTheFileSpeechAsync().Wait();
            //RecognizeSpeechAsync().Wait();
            if (!string.IsNullOrEmpty(program.przetworzoneAudio))
            {
                string parsed = program.przetworzoneAudio.ToLower();
                string pattern = Regex.Replace(parsed, @"[.?!,]", "");
                Console.WriteLine("parsed:" + pattern);
            }
        }
    }
}
