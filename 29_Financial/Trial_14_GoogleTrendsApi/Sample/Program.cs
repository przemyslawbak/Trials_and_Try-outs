using GoogleTrendsApi;
using Newtonsoft.Json.Linq;
using Sample;
using System.Text.Json.Nodes;

HelperService _service = new HelperService();


var components = _service.GetIndexComponents();

foreach (var component in components)
{
    try
    {
        var trending = await Api.GetInterestOverTime(new[] { component.Key }, "", DateOptions.LastWeek, GroupOptions.All, 1163);

        if (trending != null)
        {
            var arr = (JsonArray)trending;
            var lastDayQty = arr.Count / 7;
            var collection = new List<int>();

            foreach (var x in arr)
            {
                var parsed = JObject.Parse(x.ToString());
                var yyy = (int)parsed.SelectToken("value")[0];
                collection.Add(yyy);
            }

            var lastDayCollection = collection.Skip(Math.Max(0, collection.Count() - lastDayQty));
            var lastDayAverage = lastDayCollection.Average();

            Console.WriteLine("Aver from last day for " + component.Key + ": " + lastDayAverage);
        }
    }
    catch
    {
        Console.WriteLine("skipped");
    }
}

Console.WriteLine("finito");