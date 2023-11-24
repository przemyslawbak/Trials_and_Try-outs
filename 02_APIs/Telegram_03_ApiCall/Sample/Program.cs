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
        await DoLogin(config.GetSenderPhoneNumber()); // initial call with user's phone_number
        var resolved = await client.Contacts_ResolveUsername(config.GetUserName()); // username without the @

        /*

        // HTML-formatted text:
        var text = $"Hello <u>dear <b>{HtmlText.Escape(client.User.first_name)}</b></u>\n" +
                   "Enjoy this <code>userbot</code> written with <a href=\"https://github.com/wiz0u/WTelegramClient\">WTelegramClient</a>";
        var entities = client.HtmlToEntities(ref text);
        var sent = await client.SendMessageAsync(InputPeer.Self, text, entities: entities);
        // if you need to convert a sent/received Message to HTML: (easier to store)
        text = client.EntitiesToHtml(sent.message, sent.entities);
        Console.WriteLine("sent test HTML message");

        // Markdown-style text:
        var text2 = $"Hello __dear *{Markdown.Escape(client.User.first_name)}*__\n" +
                    "Enjoy this `userbot` written with [WTelegramClient](https://github.com/wiz0u/WTelegramClient)";
        var entities2 = client.MarkdownToEntities(ref text2);
        var sent2 = await client.SendMessageAsync(InputPeer.Self, text2, entities: entities2);
        // if you need to convert a sent/received Message to Markdown: (easier to store)
        text2 = client.EntitiesToMarkdown(sent2.message, sent2.entities);
        Console.WriteLine("sent test Markdown-style message");

        var chats = await client.Messages_GetAllChats();
        var target = chats.chats[2074912260];
        Console.WriteLine($"Sending a message in chat {2074912260}: {target.Title}");
        await client.SendMessageAsync(target, "Hello, World");
        Console.WriteLine("sent test to chat");

        var contacts = await client.Contacts_ImportContacts(new[] { new InputPhoneContact { phone = config.GetPhoneNumber() } });
        if (contacts.imported.Length > 0)
            await client.SendMessageAsync(contacts.users[contacts.imported[0].user_id], "Helloooooooooooooo!");
        Console.WriteLine("sent test by phone number");
        */

        // plain text
        await client.SendMessageAsync(resolved, "testing 123");
        Console.WriteLine("sent test message");
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
        //Console.WriteLine($"We are logged-in as {client.User} (id {client.User.id})");
    }
}