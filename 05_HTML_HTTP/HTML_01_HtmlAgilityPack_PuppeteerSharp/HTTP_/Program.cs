using HtmlAgilityPack;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTP_
{
    class Program
    {
        private static String url = "https://koniewyscigowe.pl/horse/260-trim";
        static void Main(string[] args)
        {
            var htmlAsTask = LoadAndWaitForSelector(url, "html");
            htmlAsTask.Wait();
            Console.WriteLine(htmlAsTask.Result);
            AgilityParseHtml(htmlAsTask.Result);

            Console.ReadKey();
        }

        private static void AgilityParseHtml(string result)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(result);
            //horses name
            HtmlNode horsesName = doc.DocumentNode.SelectSingleNode("/html/body/main/section[1]/div/div[2]/div[2]/header/div/h1");
            try
            {
                if (horsesName != null)
                {
                    string theName = horsesName.OuterHtml.ToString();
                    theName = theName.Split('>')[1].Split('<')[0].Trim(' ');

                    theName = MakeTitleCase(theName);
                }
            }
            catch (Exception e)
            {

            }
        }

        private static string MakeTitleCase(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                TextInfo myCI = new CultureInfo("en-US", false).TextInfo; //creates CI
                name = name.ToLower().Trim(' '); //takes to lower, to take to TC later
                name = myCI.ToTitleCase(name); //takes to TC
            }

            return name;
        }

        public static async Task<string> LoadAndWaitForSelector(String url, String selector)
        {
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                ExecutablePath = @"c:\Program Files (x86)\Google\Chrome\Application\chrome.exe"
            });
            using (Page page = await browser.NewPageAsync())
            {
                await page.GoToAsync(url);
                await page.WaitForSelectorAsync(selector);
                return await page.GetContentAsync();
            }
        }
    }
}
