using System.Threading.Tasks;

namespace SMTP
{
    //config: https://www.hmailserver.com/documentation/v5.6/?page=scenario_single_server_static_ip
    class Program
    {
        static async Task Main(string[] args)
        {
            await SampleMailClient.SendAsync();
        }
    }
}
