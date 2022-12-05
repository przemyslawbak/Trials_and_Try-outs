using System;
using System.Text;
using System.Speech.Recognition;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            SpeechRecognitionEngine sre = new SpeechRecognitionEngine();
            Grammar gr = new DictationGrammar();
            sre.LoadGrammar(gr);
            sre.SetInputToWaveFile("C:sample.wav");
            sre.BabbleTimeout = new TimeSpan(Int32.MaxValue);
            sre.InitialSilenceTimeout = new TimeSpan(Int32.MaxValue);
            sre.EndSilenceTimeout = new TimeSpan(100000000);
            sre.EndSilenceTimeoutAmbiguous = new TimeSpan(100000000);
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                try
                {
                    var recText = sre.Recognize();
                    if (recText == null)
                    {
                        break;
                    }

                    sb.Append(recText.Text);
                }
                catch (Exception ex)
                {
                    //handle exception
                    //...

                    break;
                }
            }
            Console.WriteLine(sb.ToString());
            Console.WriteLine("wciśnij dowolny klawisz");
            Console.ReadKey();
        }
    }
}
