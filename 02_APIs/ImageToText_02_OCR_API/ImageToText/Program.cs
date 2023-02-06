using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ImageToText
{
    public class Rootobject
    {
        public Parsedresult[] ParsedResults { get; set; }
        public int OCRExitCode { get; set; }
        public bool IsErroredOnProcessing { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }
    }

    public class Parsedresult
    {
        public object FileParseExitCode { get; set; }
        public string ParsedText { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Program obj = new Program();
            obj.ConvertImageToTextAsync().Wait();

            Console.ReadKey();
        }
        public async Task ConvertImageToTextAsync()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(1, 1, 1);
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent("94918c9c5c88957"), "apikey"); //Added api key in form data
                form.Add(new StringContent("eng"), "language");

                string image = "chart.png";

                byte[] imageData = File.ReadAllBytes(image);

                form.Add(new ByteArrayContent(imageData, 0, imageData.Length), "image", "image.jpg");

                HttpResponseMessage response = await httpClient.PostAsync("https://api.ocr.space/Parse/Image", form);

                string strContent = await response.Content.ReadAsStringAsync();

                Rootobject ocrResult = JsonConvert.DeserializeObject<Rootobject>(strContent);

                if (ocrResult.OCRExitCode == 1)
                {
                    for (int i = 0; i < ocrResult.ParsedResults.Count(); i++)
                    {
                        string res = ocrResult.ParsedResults[i].ParsedText;
                        //res = res.Replace(" ", "").Trim();
                        Console.WriteLine(res);
                    }
                }
                else
                {
                    Console.WriteLine("ERROR: " + strContent);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
