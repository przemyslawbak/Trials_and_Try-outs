﻿using System;
using System.Collections;

namespace TutorialsPt_01f_System.Collections
{
    class Program //https://www.tutorialspoint.com/csharp/csharp_bitarray.htm
    {
        static void Main(string[] args)
        {
            //creating two  bit arrays of size 8
            BitArray ba1 = new BitArray(8);
            BitArray ba2 = new BitArray(8);

            byte[] a = { 60 };
            byte[] b = { 13 };

            //storing the values 60, and 13 into the bit arrays
            ba1 = new BitArray(a); //wartości bitowe dla 60
            ba2 = new BitArray(b); //wartości bitowe dla 13

            //content of ba1
            Console.WriteLine("Bit array ba1: 60");

            for (int i = 0; i < ba1.Count; i++)
            {
                Console.Write("{0, -6} ", ba1[i]);
            }
            Console.WriteLine();

            //content of ba2
            Console.WriteLine("Bit array ba2: 13");

            for (int i = 0; i < ba2.Count; i++)
            {
                Console.Write("{0, -6} ", ba2[i]);
            }
            Console.WriteLine();
            BitArray ba3 = new BitArray(8);
            ba3 = ba1.And(ba2); //porównanie z fcją logizną ORAZ

            //content of ba3
            Console.WriteLine("Bit array ba3 after AND operation: 12");

            for (int i = 0; i < ba3.Count; i++)
            {
                Console.Write("{0, -6} ", ba3[i]);
            }
            Console.WriteLine();
            ba3 = ba1.Or(ba2); //porównanie z fcją logizną LUB

            //content of ba3
            Console.WriteLine("Bit array ba3 after OR operation: 61");

            for (int i = 0; i < ba3.Count; i++)
            {
                Console.Write("{0, -6} ", ba3[i]);
            }
            Console.WriteLine();

            Console.ReadKey();
        }
    }
}
