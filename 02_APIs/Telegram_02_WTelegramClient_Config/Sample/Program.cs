using Sample;
using TL;
using WTelegram;

internal class Program
{
    private static Client client;
    private static async Task Main(string[] args)
    {
        var config = new ConfigProvider();
        client = new WTelegram.Client(config.GetApiId(), config.GetApiHash()); // this constructor doesn't need a Config method
        await DoLogin(config.GetPhoneNumber()); // initial call with user's phone_number

        var myself = await client.LoginUserIfNeeded();
        Console.WriteLine($"We are logged-in as {myself} (id {myself.id})");
        Console.WriteLine("Press any key...");
        Console.ReadKey();
    }

    private static async Task DoLogin(string loginInfo) // (add this method to your code)
    {
        while (client.User == null)
            switch (await client.Login(loginInfo)) // returns which config is needed to continue login
            {
                case "verification_code": Console.Write("Code: "); loginInfo = Console.ReadLine(); break;
                case "name": loginInfo = "John Doe"; break;    // if sign-up is required (first/last_name)
                case "password": loginInfo = "secret!"; break; // if user has enabled 2FA
                default: loginInfo = null; break;
            }
        Console.WriteLine($"We are logged-in as {client.User} (id {client.User.id})");
    }
}