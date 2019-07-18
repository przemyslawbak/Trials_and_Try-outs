using System;

namespace _06_Tablice
{
    public struct Point { public int X, Y; }
    class Program
    {
        static void Main(string[] args)
        {
            char[] vowels = new char[5]; // deklaracja tablicy pięciu znaków
            vowels[0] = 'a';
            vowels[1] = 'e';
            vowels[2] = 'i';
            vowels[3] = 'o';
            vowels[4] = 'u';
            Console.WriteLine(vowels); // e
            for (int i = 0; i < vowels.Length; i++)
                Console.WriteLine(vowels[i]); // aeiou

            char[] tablica = { 'a', 'e', 'i', 'o', 'u' };
            Console.WriteLine(tablica); // e

            //domyślna inicjalizacja
            int[] a = new int[1000];
            Console.Write(a[123]); // 0

            //wielowymiarowa
            int[,] matrix = new int[3, 3];
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    matrix[i, j] = i * 3 + j;

            var jaggedMat = new int[][] // jaggedMat jest niejawnie typu int[][]
            {
                new int[] {0,1,2},
                new int[] {3,4,5},
                new int[] {6,7,8}
            };
            Console.ReadKey();
        }
    }
}
