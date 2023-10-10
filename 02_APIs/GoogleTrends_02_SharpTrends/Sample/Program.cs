using SharpTrends.DailyTrends;

var client = new SharpTrends.Client();

List<TrendingSearch> data = client.DailyTrends();

foreach (var i in data) //unable to search own :(
{
    Console.WriteLine($"{i.Title}:" +
        $"\n\tDate:\t{i.Date}" +
        $"\n\tTraffic:\t{i.Traffic:E2}" +
        $"\n\tRelated:\t{String.Join(", ", i.RelatedQueries)}" +
        $"\n\tArticles:\t{i.Articles.Count}");
}