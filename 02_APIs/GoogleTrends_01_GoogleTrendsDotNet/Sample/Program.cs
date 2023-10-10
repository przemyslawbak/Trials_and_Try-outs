using GoogleTrends;
using GoogleTrends.Models.Explore;
using GoogleTrends.Models.ParameterTypes;

var trendsClient = new GoogleTrendsClient();

ExploreResponse exploreData = await trendsClient.Explore.ExploreQuery("aapl", SearchType.WebSearch,
            queryTime: QueryTimes.PastWeek, geoSearch: GeoId.WorldWide); //exception

Console.WriteLine("done");