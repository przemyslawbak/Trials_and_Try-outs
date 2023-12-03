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

        Console.WriteLine("FINITO");
    }
}

