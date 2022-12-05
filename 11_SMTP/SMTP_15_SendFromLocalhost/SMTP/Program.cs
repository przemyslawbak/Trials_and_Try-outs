using System.Threading.Tasks;

namespace SMTP
{
    //config: https://www.hmailserver.com/documentation/v5.6/?page=scenario_single_server_static_ip
    //CONCLUSION: https://4programmers.net/Forum/Webmastering/359634-lokalny_serwer_smtp_i_wysylanie_wiadomosci?p=1834349#id1834349
    class Program
    {
        static async Task Main(string[] args)
        {
            await SampleMailClient.SendAsync();
        }
    }
}
