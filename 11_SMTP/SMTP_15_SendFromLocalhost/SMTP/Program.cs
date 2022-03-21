using System.Threading.Tasks;

namespace SMTP
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await SampleMailClient.SendAsync();
        }
    }
}
