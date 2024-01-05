

using System.Net;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var url = "https://jooble.org/api/";
        var key = "xxx";

        //create request object
        WebRequest request = HttpWebRequest.Create(url + key);
        //set http method
        request.Method = "POST";
        //set content type
        request.ContentType = "application/json";
        //create request writer
        var writer = new StreamWriter(request.GetRequestStream());
        //write request body
        writer.Write("{ keywords: 'it', location: 'Washington'}");
        //close writer
        writer.Close();
        //get response reader
        var response = request.GetResponse();
        var reader = new StreamReader(response.GetResponseStream());
        //read response
        while (!reader.EndOfStream)
        {
            Console.WriteLine(reader.ReadLine());
        }
    }
}