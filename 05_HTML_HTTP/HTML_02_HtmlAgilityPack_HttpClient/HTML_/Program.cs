using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HTML_
{
    //https://stackoverflow.com/a/41781403/11027921
    class Program
    {
        static void Main(string[] args)
        {
            DoSomething();
        }

        private static void DoSomething()
        {
            CallSomethingAsync();
        }

        private static void CallSomethingAsync()
        {
            var htmlAsTask = GetHtmlDocumentAsync();
            AgilityParseHtml(htmlAsTask.Result);
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

        private static async Task<string> GetHtmlDocumentAsync()
        {
            string result = "";

            string url = "https://koniewyscigowe.pl/horse/260-trim";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);

                result = await response.Content.ReadAsStringAsync();
            }

            return result;
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
    }
}
