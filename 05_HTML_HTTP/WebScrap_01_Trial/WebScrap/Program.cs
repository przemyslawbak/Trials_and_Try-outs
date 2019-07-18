using HtmlAgilityPack;
using System;
using System.Linq;

namespace WebScrap
{
    //https://www.c-sharpcorner.com/article/web-scraping-in-c-sharp/
    class Program
    {
        static void Main(string[] args)
        {
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            for (var i = 1; i < 11; i++)
            {
                Console.WriteLine("page" + i);
                HtmlAgilityPack.HtmlDocument doc = web.Load("https://www.vesselfinder.com/vessels?page=" + i);
                HtmlNode[] nodes = doc.DocumentNode.SelectNodes("/html/body/div/div/main/div/section/table/tbody/tr/td[3]").ToArray();
                foreach (HtmlNode item in nodes)
                {
                    Console.WriteLine(item.InnerHtml);
                }
            }
            Console.ReadKey();
        }
    }
}
