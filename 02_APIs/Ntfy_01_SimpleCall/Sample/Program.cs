using Sample;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var config = new ConfigProvider();
        var channel = config.GetChannel();
        var url = "https://ntfy.sh/" + channel;

        HttpClient client = new HttpClient();



        var emojis = GetEmojis();

        var res = await client.PostAsync(url, new StringContent("test " + emojis[0]));

        Console.WriteLine("sent test message");
    }

    public static string[] GetEmojis()
    {
        return new string[]
            {
                "♋",
                "⏫",
                "🌄",
                "🌈",
                "🌋",
                "🌌",
                "🎆",
                "🎡",
                "🎠",
                "🎰",
                "👔",
                "💌",
                "💟",
                "💩",
                "💰",
                "💼",
                "📊",
                "🗻",
                "🗽",
                "✅",
                "🚹",
                "🚺",
                "🚼",
                "🅰",
                "🅱",
                "🅾",
                "🅿",
                "🆚",
                "🈯",
                "®",
                "⚠",
                "⛔",
                "❌",
                "😜",
                "😡",
                "🙈",
                "✂",
                "✈",
                "❄",
                "🚀",
                "🚥",
                "🚧",
                "🚩",
                "🚻",
                "🇩🇪",
                "🇵🇱",
                "🇬🇧",
                "⌛",
                "☀",
                "☁",
                "☎",
                "☔",
                "♦",
                "♨",
                "♻",
                "⚡",
                "⚓",
                "⚽",
                "⛄",
                "🃏",
                "🌵",
                "🍁",
                "🍄",
                "🍔",
                "🍺",
                "🎅",
                "🎎",
                "🎨",
                "🏊",
                "🏡",
                "🐌",
                "🐯",
                "🐸",
                "👑",
                "👹",
                "👾",
                "💥",
            };
    }
}