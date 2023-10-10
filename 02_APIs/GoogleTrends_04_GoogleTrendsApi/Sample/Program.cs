using GoogleTrendsApi;

var xxx = await Api.GetInterestOverTime(new[] { "happy" }, "", DateOptions.LastHour, GroupOptions.All, 14); //cool

Console.WriteLine();