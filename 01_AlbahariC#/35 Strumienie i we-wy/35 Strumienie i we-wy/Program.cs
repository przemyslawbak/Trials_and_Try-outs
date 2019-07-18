using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;

namespace _35_Strumienie_i_we_wy
{
    class Program
    {
        //wykorzystujemy strumień pliku w celu przeprowadzenia operacji odczytu, zapisu i wyszukiwania
        static void Main(string[] args)
        {
            // utworzenie w katalogu bieżącym pliku o nazwie test.txt
            using (Stream s = new FileStream("test.txt", FileMode.Create))
            {
                Console.WriteLine(s.CanRead); // prawda
                Console.WriteLine(s.CanWrite); // prawda
                Console.WriteLine(s.CanSeek); // prawda
                s.WriteByte(101);
                s.WriteByte(102);
                byte[] block = { 1, 2, 3, 4, 5 };
                s.Write(block, 0, block.Length); // zapis bloku 5 bajtów
                Console.WriteLine(s.Length); // 7
                Console.WriteLine(s.Position); // 7
                s.Position = 0; // przejście z powrotem na początek
                Console.WriteLine(s.ReadByte()); // 101
                Console.WriteLine(s.ReadByte()); // 102
                                                 // odczyt danych ze strumienia i umieszczenie ich w bloku tablicy
                Console.WriteLine(s.Read(block, 0, block.Length)); // 5
                                                                   // przyjmując założenie, że ostatni odczyt zwrócił wartość 5, znajdujemy się
                                                                   // na końcu pliku i dlatego teraz wywołanie Read() zwróci wartość 0
                Console.WriteLine(s.Read(block, 0, block.Length)); // 0
            }

            //to samo ale Async
            Console.WriteLine("Async:");
            AsyncDemo();

            //StreamReader i StreamWriter
            //StreamWriter zapisuje dwa wiersze tekstu w pliku, a następnie egzemplarz StreamReader odczytuje tekst zapisany w pliku
            using (FileStream fss = File.Create("testy.txt"))
            using (TextWriter writer = new StreamWriter(fss))
            {
                writer.WriteLine("Wiersz1");
                writer.WriteLine("Wiersz2");
            }
            using (FileStream fs = File.OpenRead("testy.txt"))
            using (TextReader reader = new StreamReader(fs))
            {
                Console.WriteLine(reader.ReadLine()); // Wiersz1
                Console.WriteLine(reader.ReadLine()); // Wiersz2
            }
            using (TextWriter writer = File.AppendText("testt.txt"))
                writer.WriteLine("Wiersz3");
            using (TextReader reader = File.OpenText("testt.txt"))
                while (reader.Peek() > -1)
                    Console.WriteLine(reader.ReadLine()); // Wiersz1
                                                          // Wiersz2
                                                          // Wiersz3
            using (TextWriter w = File.CreateText("but.txt")) // użycie domyślnego
                w.WriteLine("but—"); // kodowania znaków UTF-8
            using (Stream s = File.OpenRead("but.txt"))
                for (int b; (b = s.ReadByte()) > -1;)
                    Console.WriteLine(b);
            //kompresja strumienia
            using (Stream s = File.Create("compressed.bin"))
            using (Stream ds = new DeflateStream(s, CompressionMode.Compress))
                for (byte i = 0; i < 100; i++)
                    ds.WriteByte(i);
            using (Stream s = File.OpenRead("compressed.bin"))
            using (Stream ds = new DeflateStream(s, CompressionMode.Decompress))
                for (byte i = 0; i < 100; i++)
                    Console.WriteLine(ds.ReadByte()); // zapis od 0 do 99
            byte[] data = new byte[1000]; // W przypadku pustej tablicy można się spodziewać

            //kompresja w pamięci
                                          // uzyskania dobrego współczynnika kompresji!
            var ms = new MemoryStream();
            using (Stream ds = new DeflateStream(ms, CompressionMode.Compress))
                ds.Write(data, 0, data.Length);
            byte[] compressed = ms.ToArray();
            Console.WriteLine(compressed.Length); // 11
                                                  // dekompresja na postać tablicy danych
            ms = new MemoryStream(compressed);
            using (Stream ds = new DeflateStream(ms, CompressionMode.Decompress))
                for (int i = 0; i < 1000; i += ds.Read(data, i, 1000 - i)) ;

        }
        //Asynchroniczny odczyt lub zapis to po prostu kwestia wywołania ReadAsync() lub WriteAsync()
        async static void AsyncDemo()
        {
            using (Stream s = new FileStream("test.txt", FileMode.Create))
            {
                byte[] block = { 1, 2, 3, 4, 5 };
                await s.WriteAsync(block, 0, block.Length); // zapis asynchroniczny
                s.Position = 0; // przejście z powrotem na początek
                                // odczyt danych ze strumienia i umieszczenie ich w bloku tablicy
                Console.WriteLine(await s.ReadAsync(block, 0, block.Length)); // 5
            }
        }
    }
}
