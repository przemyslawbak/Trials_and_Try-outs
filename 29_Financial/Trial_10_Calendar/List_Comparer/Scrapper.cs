using System.Net.Http;
using System.Threading.Tasks;

namespace List_Comparer
{
    internal class Scrapper
    {
        private static HttpClient _client = new HttpClient();   
        public async Task<string> GetHtml(string url)
        {
            return await GetHtmlDocumentAsync(url);
        }

        public async Task<string> GetHtmlDocumentAsync(string url)
        {
            string html = string.Empty;
            var httpClient = _client;

            using (HttpResponseMessage response = await httpClient.GetAsync(url))
            {
                using (HttpContent content = response.Content)
                {
                    html = await content.ReadAsStringAsync();
                }
            }

            return html;
        }
    }
}
