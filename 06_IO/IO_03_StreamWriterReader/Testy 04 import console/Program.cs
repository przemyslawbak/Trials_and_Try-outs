using System;
using System.IO;
using System.Text;

namespace Testy_04_import_console
{
    //https://4programmers.net/Forum/C_i_.NET/330558-problem_z_while_i_odczytem_danych_przy_pomocy_streamreader_client_ctp
    class Program
    {
        static void Main(string[] args)
        {
            byte[] task = Encoding.ASCII.GetBytes("test encoding");

            using (StreamWriter sw = new StreamWriter("test.txt"))
            {
                sw.BaseStream.Write(task, 0, task.Length);
            }

            var stream1 = new StreamReader("test.txt", Encoding.ASCII);
            string line = string.Empty;
            while ((line = stream1.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }

            Console.WriteLine("koniec programu");

            Console.ReadLine();
        }
    }
}
