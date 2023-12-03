using Newtonsoft.Json;
using Sample;

internal class Program
{
    private static HttpClient _client = new HttpClient();

    private static async Task Main(string[] args)
    {
        HelperService _service = new HelperService();
        UrlLocker _locker = new UrlLocker();

        Console.WriteLine("Sending request to get JSON...");
        HttpResponseMessage response = await _client.GetAsync(_locker.GetApiSymbolsUrl());
        response.EnsureSuccessStatusCode();
        var jsonString = await response.Content.ReadAsStringAsync();

        Console.WriteLine("Writing JSON to file...");
        File.WriteAllText("_json.txt", jsonString);

        Console.WriteLine("Deserializing JSON string...");
        var symbols = JsonConvert.DeserializeObject<List<SymbolModel>>(jsonString);

        //NOW:
        //todo: get OHLCV for all symbols
        //todo: detect trends (up, down, horizontal) https://www.investopedia.com/articles/active-trading/041814/four-most-commonlyused-indicators-trend-trading.asp
        //todo: detect trend strenght (MACD, RSI, OBV)
        //todo: add data about the duration of the trend?
        //todo: compute normal distribution velocity (5%, 20%, 50%)
        //todo: add distribution data for each trend (average if appears more than one time)
        //todo: save result as a CSV

        //LATER:
        //

        Console.WriteLine("FINITO");
    }
}

