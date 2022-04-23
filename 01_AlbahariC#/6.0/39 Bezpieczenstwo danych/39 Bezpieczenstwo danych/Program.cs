using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace _39_Bezpieczenstwo_danych
{
    class Program
    {
        static void Main(string[] args)
        {
            //szyfrowanie symetryczne
            byte[] key = { 145, 12, 32, 245, 98, 132, 98, 214, 6, 77, 131, 44, 221, 3, 9, 50 };
            byte[] iv = { 15, 122, 132, 5, 93, 198, 44, 31, 9, 39, 241, 49, 250, 188, 80, 7 };
            byte[] data = { 1, 2, 3, 4, 5 }; // to szyfrujemy
            using (SymmetricAlgorithm algorithm = Aes.Create())
            using (ICryptoTransform encryptor = algorithm.CreateEncryptor(key, iv))
            using (Stream f = File.Create("encrypted.bin"))
            using (Stream c = new CryptoStream(f, encryptor, CryptoStreamMode.Write))
                c.Write(data, 0, data.Length);
            //rozszyfrowanie
            byte[] decrypted = new byte[5];
            using (SymmetricAlgorithm algorithm = Aes.Create())
            using (ICryptoTransform decryptor = algorithm.CreateDecryptor(key, iv))
            using (Stream f = File.OpenRead("encrypted.bin"))
            //using (Stream c = new CryptoStream(f, decryptor, CryptoStreamMode.Read)) <- nie dziala
                //for (int b; (b = c.ReadByte()) > –1; )
            //Console.Write(b + " "); // 1 2 3 4 5

            //RandomNumberGenerator - silne kryptograficznie liczby
            key = new byte[16];
            iv = new byte[16];
            RandomNumberGenerator rand = RandomNumberGenerator.Create();
            rand.GetBytes(key);
            rand.GetBytes(iv);

            //szyfrowanie w pamięci użycie
            byte[] kiv = new byte[16];
            RandomNumberGenerator.Create().GetBytes(kiv);
            string encrypted = Encrypt("Yeah!", kiv, kiv);
            Console.WriteLine(encrypted); // R1/5gYvcxyR2vzPjnT7yaQ==
            string decryptedd = Decrypt(encrypted, kiv, kiv);
            Console.WriteLine(decryptedd); // Yeah!

            //Tworzenie łańcuchów strumieni szyfrowania
            // w celach demonstracyjnych używamy domyślnych klucza i wektora inicjalizującego
            using (Aes algorithm = Aes.Create())
            {
                using (ICryptoTransform encryptor = algorithm.CreateEncryptor())
                using (Stream f = File.Create("serious.bin"))
                using (Stream c = new CryptoStream(f, encryptor, CryptoStreamMode.Write))
                using (Stream d = new DeflateStream(c, CompressionMode.Compress))
                using (StreamWriter w = new StreamWriter(d))
                w.WriteLine("Niewielkie i bezpieczne!"); //<- też możliwe asynchroniczne
                using (ICryptoTransform decryptor = algorithm.CreateDecryptor())
                using (Stream f = File.OpenRead("serious.bin"))
                using (Stream c = new CryptoStream(f, decryptor, CryptoStreamMode.Read))
                using (Stream d = new DeflateStream(c, CompressionMode.Decompress))
                using (StreamReader r = new StreamReader(d))
                    Console.WriteLine(r.ReadLine()); // niewielkie i bezpieczne! <- też możliwe asynchroniczne
            }

            //szyfrowanie kluczem publicznym i podpisywaniem
            //RSA
            byte[] dataa = { 1, 2, 3, 4, 5 }; // to szyfrujemy
            using (var rsa = new RSACryptoServiceProvider())
            {
                byte[] encryptedq = rsa.Encrypt(dataa, true);
                byte[] decryptedq = rsa.Decrypt(encryptedq, true);
            }
            //dla aplikacji o wysokich wymaganiach bezpieczeństwa
            //using (var rsa = new RSACryptoServiceProvider(2048))
            Console.ReadKey();
        }
        //szyfrowanie w pamięci - Przy użyciu klasy MemoryStream można dokonywać szyfrowania i deszyfrowania w całości w pami
        public static byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
        {
            using (Aes algorithm = Aes.Create())
            using (ICryptoTransform encryptor = algorithm.CreateEncryptor(key, iv))
                return Crypt(data, encryptor);
        }
        public static byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
        {
            using (Aes algorithm = Aes.Create())
            using (ICryptoTransform decryptor = algorithm.CreateDecryptor(key, iv))
                return Crypt(data, decryptor);
        }
        //przeciążone wersje metod, które przyjmują i zwracają łańcuchy:
        public static string Encrypt(string data, byte[] key, byte[] iv)
        {
            return Convert.ToBase64String(
            Encrypt(Encoding.UTF8.GetBytes(data), key, iv));
        }
        public static string Decrypt(string data, byte[] key, byte[] iv)
        {
            return Encoding.UTF8.GetString(
            Decrypt(Convert.FromBase64String(data), key, iv));
        }
        static byte[] Crypt(byte[] data, ICryptoTransform cryptor)
        {
            MemoryStream m = new MemoryStream();
            using (Stream c = new CryptoStream(m, cryptor, CryptoStreamMode.Write))
                c.Write(data, 0, data.Length);
            return m.ToArray();
        }
    }
}
