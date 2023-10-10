using Trendsdotnet;

using (TrendsClient client = new TrendsClient())
{
    string[] terms = new string[] { "aapl", "kghm" };
    var res = await client.GetInterestOverTime(terms, DateTime.Now.AddDays(-7), DateTime.Now, Resolution.WEEK); //exception
}