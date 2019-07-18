using System;
using System.Text.RegularExpressions;

namespace _40_Regex
{
    class Program
    {
        static void Main(string[] args)
        {
            //Match
            Console.WriteLine(Regex.Match("color", @"colou?r").Success); // prawda
            Console.WriteLine(Regex.Match("colour", @"colou?r").Success); // prawda
            Console.WriteLine(Regex.Match("colouur", @"colou?r").Success); // fałsz
            Match m = Regex.Match("Dowolny ulubiony kolor", @"kolor?");
            Console.WriteLine(m.Success); // prawda
            Console.WriteLine(m.Index); // 4
            Console.WriteLine(m.Length); // 6
            Console.WriteLine(m.Value); // kolor
            Console.WriteLine(m.ToString()); // kolor
            Match m1 = Regex.Match("Tylko jeden kolor? Mam na myśli dwa kolory!", @"kolory?");
            Match m2 = m1.NextMatch();
            Console.WriteLine(m1); // kolor
            Console.WriteLine(m2); // kolory
            foreach (Match n in Regex.Matches ("Tylko jeden kolor? Mam na myśli dwa kolory!", @"kolory?"))
                Console.WriteLine(n);
            Console.WriteLine(Regex.IsMatch("Karol", "Kar(ol|olina)?")); // prawda

            //Kompilowane wyrażenia regularne
            Regex r = new Regex(@"pomidory?", RegexOptions.Compiled);
            Console.WriteLine(r.Match("pomidor")); // pomidor
            Console.WriteLine(r.Match("pomidory")); // pomidory

            //Typ wyliczeniowy RegexOptions
            Console.WriteLine(Regex.Match("a", "A", RegexOptions.IgnoreCase)); // a
            Console.WriteLine(Regex.Match("a", "A", RegexOptions.IgnoreCase
            | RegexOptions.CultureInvariant));
            Console.WriteLine(Regex.Match("a", @"(?i)A")); // a
            Console.WriteLine(Regex.Match("AAAa", @"(?i)a(?-i)a")); // Aa

            //znaki sterujące
            Console.WriteLine(Regex.Match("co takiego?", @"co takiego\?")); // co takiego? (prawidłowo)
            Console.WriteLine(Regex.Match("co takiego?", @"co takiego?")); // co takiego (nieprawidłowo)
            Console.WriteLine(Regex.Escape(@"?")); // \?
            Console.WriteLine(Regex.Unescape(@"\?")); // ?>
            Console.WriteLine(Regex.Match("\\", "\\\\")); // \
            Console.Write(Regex.IsMatch("witaj, świecie", @"witaj, świecie")); // prawda
            Console.Write(Regex.Matches("To jest to.", "[Tt]o").Count); // 2
            Console.Write(Regex.Match("quiz qwerty", "q[^aeiou]").Index); // 5
            Console.Write(Regex.Match("b1-c4", @"[a-h]\d-[a-h]\d").Success); // prawda
            Console.Write(Regex.IsMatch("Tak, proszę", @"\p{P}")); // prawda
            Console.Write(Regex.Match("cv15.doc", @"cv\d*\.doc").Success); // prawda
            Console.Write(Regex.Match("cvjoint.doc", @"cv.*\.doc").Success); // prawda
            Console.Write(Regex.Matches("wolno! naprawdę wooolno!", "wo+lno").Count); // 2
            Regex bp = new Regex(@"\d{2,3}/\d{2,3}");
            Console.WriteLine(bp.Match("Zwykle mam ciśnienie 160/110")); // 160/110
            Console.WriteLine(bp.Match("Teraz mam ciśnienie zaledwie 115/75")); // 115/75

            //Kwantyfikator zachłanny kontra leniwy
            string html = "<i>Domyślnie</i> kwantyfikatory są <i>zachłannymi</i> bestiami";
            foreach (Match o in Regex.Matches(html, @"<i>.*</i>"))
                Console.WriteLine(o);
            foreach (Match p in Regex.Matches(html, @"<i>.*?</i>"))
                Console.WriteLine(p);

            //Asercje o zerowej wielkości
            Console.WriteLine(Regex.Match("powiedzmy jakieś 25 mil dalej", @"\d+\s(?=mil)"));
            Console.WriteLine(Regex.Match("powiedzmy jakieś 25 mil dalej", @"\d+\s(?=miles).*"));
            string regex = "(?i)dobra(?!.*(jednak|ale))";
            Console.WriteLine(Regex.IsMatch("Dobra robota! Ale...", regex)); // fałsz
            Console.WriteLine(Regex.IsMatch("Dobra robota! Dziękujemy!", regex)); // prawda
            string regexx = "(?i)(?<!jednak.*)dobrze";
            Console.WriteLine(Regex.IsMatch("Jednak dobrze, sądziliśmy...", regexx)); // fałsz
            Console.WriteLine(Regex.IsMatch("Bardzo dobrze, dziękujemy!", regexx)); // prawda

            //kotwice
            Console.WriteLine(Regex.Match("Nie teraz", "^[Nn]ie")); // Nie
            Console.WriteLine(Regex.Match("f = 0.2F", "[Ff]$")); // F
            string fileNames = "a.txt" + "\r\n" + "b.doc" + "\r\n" + "c.txt";
            string s = @".+\.txt(?=\r?$)";
            foreach (Match z in Regex.Matches(fileNames, s, RegexOptions.Multiline))
                Console.Write(z + " ");
            MatchCollection emptyLines = Regex.Matches(s, "^(?=\r?$)", RegexOptions.Multiline);
            MatchCollection blankLines = Regex.Matches(s, "^[ \t]*(?=\r?$)", RegexOptions.Multiline);
            Console.WriteLine(Regex.Match("x", "$").Length); // 0
            foreach (Match mw in Regex.Matches("Wesele na wzgórzu", @"\b\w+\b"))
                Console.WriteLine(mw);
            int one = Regex.Matches("Wesele na wzgórzu", @"\bna\b").Count; // 1
            int two = Regex.Matches("Wesele na wzgórzu", @"na").Count; // 2
            string text = "Nie trać (sic) głowy";
            Console.Write(Regex.Match(text, @"\b\w+\b\s(?=\(sic\))")); // trać

            //grupy
            Match ma = Regex.Match("206-465-1918", @"(\d{3})-(\d{3}-\d{4})");
            Console.WriteLine(ma.Groups[1]); // 206
            Console.WriteLine(ma.Groups[2]); // 465-1918
            Console.WriteLine(ma.Groups[0]); // 206-465-1918
            Console.WriteLine(ma); // 206-465-1918
            foreach (Match mq in Regex.Matches("pop pope peep", @"\b(\w)\w+\1\b"))
                Console.Write(m + " "); // pop peep

            //Nazwane grupy
            string regEx =
                @"\b" + // granica słowa
                @"(?'litera'\w)" + // dopasowanie pierwszej litery i nazwanie jej 'litera'
                @"\w+" + // dopasowanie środkowych liter
                @"\k'litera'" + // dopasowanie ostatniej litery wskazanej przez 'litera'
                @"\b"; // granica słowa
            foreach (Match me in Regex.Matches("bob pope peep", regEx))
                Console.Write(me + " "); // bob peep
            string regFind =
                @"<(?'znacznik'\w+?).*>" + // dopasowanie pierwszego znacznika i nadanie mu nazwy 'znacznik'
                @"(?'tresc'.*?)" + // dopasowanie treści i nadanie jej nazwy 'tresc'
                @"</\k'znacznik'>"; // dopasowanie ostatniego znacznika wskazanego przez 'znacznik'
            Match mr = Regex.Match("<h1>witaj</h1>", regFind);
            Console.WriteLine(mr.Groups["tag"]); // h1
            Console.WriteLine(mr.Groups["text"]); // witaj

            //Zastępowanie i dzielenie tekstu
            string find = @"\bkota\b";
            string replace = "psa";
            Console.WriteLine(Regex.Replace("kotwica przygniotła kota", find, replace));
            string textt = "10 plus 20 daje 30";
            Console.WriteLine(Regex.Replace(textt, @"\d+", @"<$0>"));
            string regFindd =
                @"<(?'znacznik'\w+?).*>" + // dopasowanie pierwszego znacznika i nadanie mu nazwy 'znacznik'
                @"(?'tresc'.*?)" + // dopasowanie treści i nadanie jej nazwy 'tresc'
                @"</\k'znacznik'>"; // dopasowanie ostatniego znacznika wskazanego przez 'znacznik'
            string regReplace =
            @"<${znacznik}" + // <znacznik
            @"value=""" + // value="
            @"${tresc}" + // tresc
            @"""/>"; // "/>
            Console.Write(Regex.Replace("<msg>witaj</msg>", regFindd, regReplace));

            //Delegat MatchEvaluator
            Console.WriteLine(Regex.Replace("5 to mniej niż 10", @"\d+", im => (int.Parse(im.Value) * 10).ToString()));

            //Podział tekstu
            foreach (string sv in Regex.Split("a5b7c", @"\d"))
                Console.Write(sv + " "); // a b c
            foreach (string sr in Regex.Split("jedenDwaTrzy", @"(?=[A-Z])"))
                Console.Write(sr + " "); // jeden Dwa Trzy

            //Receptury wyrażeń regularnych
            string ssNum = @"\d{3}-\d{2}-\d{4}";
            Console.WriteLine(Regex.IsMatch("123-45-6789", ssNum)); // prawda
            string phone = @"(?x)
            ( \d{3}[-\s] | \(\d{3}\)\s? )
            \d{3}[-\s]?
            \d{4}";
            Console.WriteLine(Regex.IsMatch("123-456-7890", phone)); // prawda
            Console.WriteLine(Regex.IsMatch("(123) 456-7890", phone)); // prawda
            string rw = @"(?m)^\s*(?'name'\w+)\s*=\s*(?'value'.*)\s*(?=\r?$)";
            string texwt =
            @"id = 3
            secure = true
            timeout = 30";
            foreach (Match am in Regex.Matches(texwt, rw))
                Console.WriteLine(am.Groups["name"] + " wynosi " + am.Groups["value"]);

            //Weryfikacja silnego hasła
            string qr = @"(?x)^(?=.* ( \d | \p{P} | \p{S} )).{6,}";
            Console.WriteLine(Regex.IsMatch("abc12", qr)); // fałsz
            Console.WriteLine(Regex.IsMatch("abcdef", qr)); // fałsz
            Console.WriteLine(Regex.IsMatch("ab88yz", qr)); // prawda

            //Wiersze o długości co najmniej 80 znaków
            string wr = @"(?m)^.{80,}(?=\r?$)";
            string fifty = new string('x', 50);
            string eighty = new string('x', 80);
            string wtext = eighty + "\r\n" + fifty + "\r\n" + eighty;
            Console.WriteLine(Regex.Matches(wtext, wr).Count); // 2

            //Przetwarzanie daty i godziny (N/N/N H:M:S AM/PM)
            string dr = @"(?x)(?i)
            (\d{1,4}) [./-]
            (\d{1,2}) [./-]
            (\d{1,4}) [\sT]
            (\d+):(\d+):(\d+) \s? (A\.?M\.?|P\.?M\.?)?";
            string dtext = "01/02/2008 5:20:50 PM";
            foreach (Group g in Regex.Match(dtext, dr).Groups)
                Console.WriteLine(g.Value + " ");

            //Dopasowanie liczb rzymskich
            string ddr =
            @"(?i)\bm*" +
            @"(d?c{0,3}|c[dm])" +
            @"(l?x{0,3}|x[lc])" +
            @"(v?i{0,3}|i[vx])" +
            @"\b";
            Console.WriteLine(Regex.IsMatch("MCMLXXXIV", ddr));

            //Usunięcie powtórzonych słów
            string rdd = @"(?'dupe'\w+)\W\k'duplikat'";
            string textdd = "Na samym samym początku...";
            Console.WriteLine(Regex.Replace(textdd, rdd, "${duplikat}"));

            //Licznik słów
            string rff = @"\b(\w|[-'])+\b";
            string textff = "McDonald's oraz Coca-Cola to marki amerykańskie";
            Console.WriteLine(Regex.Matches(textff, rff).Count); // 6

            //Dopasowanie wartości GUID
            string rgg =
            @"(?i)\b" +
            @"[0-9a-fA-F]{8}\-" +
            @"[0-9a-fA-F]{4}\-" +
            @"[0-9a-fA-F]{4}\-" +
            @"[0-9a-fA-F]{4}\-" +
            @"[0-9a-fA-F]{12}" +
            @"\b";
            string texggt = "Klucz elementu to {3F2504E0-4F89-11D3-9A0C-0305E82C3301}.";
            Console.WriteLine(Regex.Match(texggt, rgg).Index); // 12

            //Przetwarzanie znacznika XML/HTML
            string rzz =
            @"<(?'znacznik'\w+?).*>" + // dopasowanie pierwszego znacznika i nadanie mu nazwy 'znacznik'
            @"(?'tresc'.*?)" + // dopasowanie treści i nadanie jej nazwy 'tresc'
            @"</\k'znacznik'>"; // dopasowanie ostatniego znacznika wskazanego przez 'znacznik'
            string texzzt = "<h1>witaj</h1>";
            Match mzz = Regex.Match(texzzt, rzz);
            Console.WriteLine(mzz.Groups["tag"]); // h1
            Console.WriteLine(mzz.Groups["text"]); // witaj

            Console.ReadKey();
        }
    }
}
