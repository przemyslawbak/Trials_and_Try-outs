using GoogleTrendsApi;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;

var xxx = await Api.GetInterestOverTime(new[] { "aapl" }, "", DateOptions.LastWeek, GroupOptions.All, 0); //cool

if (xxx != null)
{
    var arr = (JsonArray)xxx;
    var lastDayQty = arr.Count / 7;
    var collection = new List<int>();

    foreach (var x in arr)
    {
        var parsed = JObject.Parse(x.ToString());
        var yyy = (int)parsed.SelectToken("value")[0];
        collection.Add(yyy);
    }

    var lastDayCollection = collection.Skip(Math.Max(0, collection.Count() - lastDayQty));
    var aver = lastDayCollection.Average();

    Console.WriteLine("Aver from last day: " + aver);
}

Console.WriteLine();