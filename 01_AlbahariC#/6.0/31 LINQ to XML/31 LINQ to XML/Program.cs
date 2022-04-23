using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace _31_LINQ_to_XML
{
    class Program
    {
        static void Main(string[] args)
        {
            var bench = new XElement("bench",
            new XElement("toolbox",
            new XElement("handtool", "Młotek"),
            new XElement("handtool", "Tarnik")
            ),
            new XElement("toolbox",
            new XElement("handtool", "Piła"),
            new XElement("powertool", "Pistolet na gwoździe")
            ),
            new XComment("Ostrożnie z pistoletem na gwoździe")
            );
            foreach (XNode node in bench.Nodes())
                Console.WriteLine(node.ToString(SaveOptions.DisableFormatting) + ".");
            foreach (XElement h in bench.Elements())
                Console.WriteLine(h.Name + "=" + h.Value); // toolbox=MłotekTarnik
                                                           // toolbox=PiłaPistolet na gwoździe

            //Poniższe zapytanie LINQ znajduje skrzynkę z pistoletem na gwoździe
            IEnumerable<string> query =
            from toolbox in bench.Elements()
            where toolbox.Elements().Any(tool => tool.Value == "Pistolet na gwoździe")
            select toolbox.Value;
            //pobieramy narzędzia ręczne ze wszystkich skrzynek z narzędziami
            query =
            from toolbox in bench.Elements()
            from tool in toolbox.Elements()
            where tool.Name == "handtool"
            select tool.Value;
            //Metoda Elements zwraca też elementy o określonej nazwie
            int x = bench.Elements("toolbox").Count(); // 2
            //pobieranie potomnych
            foreach (XNode node in bench.DescendantNodes())
                Console.WriteLine(node.ToString(SaveOptions.DisableFormatting));
            //Modyfikowanie drzewa X-DOM
            XElement settings = new XElement("settings");
            settings.SetElementValue("timeout", 30); // dodaje węzeł potomny
            settings.SetElementValue("timeout", 60); // zmiana jego wartości na 60
            //Za pomocą metod AddBeforeSelf i AddAfterSelf można wstawić węzeł w dowolnie wybranym miejscu
            XElement items = new XElement("items",
            new XElement("jeden"),
            new XElement("trzy")
            );
            items.FirstNode.AddAfterSelf(new XElement("dwa"));
            //zamiana
            items = XElement.Parse("<items><jeden/><dwa/><trzy/></items>");
            items.FirstNode.ReplaceWith(new XComment("Tu była jedynka"));

            //usuwanie
            XElement contacts = XElement.Parse(
            @"<contacts>
                <customer name='Maria'/>
                <customer name='Krzysztof' archived='true'/>
                <supplier name='Sylwia'>
                    <phone archived='true'>012345678<!--poufny--></phone>
                </supplier>
            </contacts>");
            //Poniższa instrukcja usuwa wszystkich klientów
            contacts.Elements("customer").Remove();
            //Poniższa instrukcja usuwa wszystkie kontakty z archiwum
            contacts.Elements().Where(g => (bool?)g.Attribute("archived") == true).Remove();
            //następnym przykładzie usuwamy wszystkie kontakty, którym w ich drzewie towarzyszy komentarz zawierający słowo "poufny"
            contacts.Elements().Where(g => g.DescendantNodes()
                .OfType<XComment>()
                .Any(c => c.Value == "poufny")
            ).Remove();

            //praca z wartościami
            //przekazywanie prostych typów danych
            var e = new XElement("date", DateTime.Now);
            e.SetValue(DateTime.Now.AddDays(1));
            Console.Write(e.Value); // 2016-04-20T13 30 41.3746505+02 00
            //pobieranie wartości
            e = new XElement("now", DateTime.Now);
            DateTime dt = (DateTime)e;
            XAttribute a = new XAttribute("resolution", 1.234);
            double res = (double)a;
            //nullable
            //int? timeout = (int?)x.Element("timeout"); // OK; timeout to null
            //double resolution = (double?)x.Attribute("resolution") ?? 1.0;

            //treść mieszana
            XElement summary = new XElement("summary",
            new XText("Obiekt typu "),
            new XElement("bold", "nie"),
            new XText(" jest obiektem typu XNode")
            );
            var e1 = new XElement("test", "Witaj, "); e1.Add("świecie");
            var e2 = new XElement("test", "Witaj, ", "świecie");
            var f = new XElement("test", new XText("Witaj, "), new XText("świecie"));
            Console.WriteLine(f.Value); // Witaj, świecie
            Console.WriteLine(f.Nodes().Count()); // 2

            //dokumenty i deklaracje
            var styleInstruction = new XProcessingInstruction(
            "xml-stylesheet", "href='styles.css' type='text/css'");
            var docType = new XDocumentType("html",
            "-//W3C//DTD XHTML 1.0 Strict//EN",
            "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd", null);
            XNamespace ns = "http://www.w3.org/1999/xhtml";
            var root =
            new XElement(ns + "html",
            new XElement(ns + "head",
            new XElement(ns + "title", "Strona XHTML")),
            new XElement(ns + "body",
            new XElement(ns + "p", "To jest treść"))
            );
            var doc =
            new XDocument(
            new XDeclaration("1.0", "utf-8", "no"),
            new XComment("Reference a stylesheet"),
            styleInstruction,
            docType,
            root);
            doc.Save("test.html");

            Console.WriteLine(doc.Root.Name.LocalName); // html
            XElement bodyNode = doc.Root.Element(ns + "body");
            Console.WriteLine(bodyNode.Document == doc); // prawda
            Console.WriteLine(doc.Root.Parent == null); // True
            foreach (XNode node in doc.Nodes())
                Console.Write(node.Parent == null); // TrueTrueTrueTrue
            //deklaracja XML
            doc = new XDocument(
            new XDeclaration("1.0", "utf-16", "yes"),
            new XElement("test", "data")
            );
            doc.Save("test.xml");
            //przestrzenie nazw
            ns = "http://domain.com/xmlspace";
            var data = new XElement(ns + "data",
            new XElement("customer", "Kowalski"),
            new XElement("purchase", "Rower")
            );
            Console.WriteLine(data.ToString());
            foreach (XElement i in data.DescendantsAndSelf())
                if (i.Name.Namespace == "")
                    i.Name = ns + i.Name.LocalName;
            //przedrostki
            XNamespace ns1 = "http://domain.com/space1";
            XNamespace ns2 = "http://domain.com/space2";
            var mix = new XElement(ns1 + "data",
            new XElement(ns2 + "element", "value"),
            new XElement(ns2 + "element", "value"),
            new XElement(ns2 + "element", "value")
            );
            mix.SetAttributeValue(XNamespace.Xmlns + "ns1", ns1);
            mix.SetAttributeValue(XNamespace.Xmlns + "ns2", ns2);

            XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
            var nil = new XAttribute(xsi + "nil", true);
            var cust = new XElement("customers",
            new XAttribute(XNamespace.Xmlns + "xsi", xsi),
            new XElement("customer",
                new XElement("lastname", "Kowalski"),
                new XElement("dob", nil),
            new XElement("credit", nil)
            )
            );

            //projekcja do X-DOM
            var customers =
            new XElement("customers",
                new XElement("customer", new XAttribute("id", 1),
                    new XElement("name", "Sylwia"),
                    new XElement("buys", 3)
                )
            );

            Console.ReadKey();
        }
    }
}
