using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;

class Solution
{
    static void Main(String[] args)
    {
        List<int> ar = new List<int>()
        {
            1, 3, 2, 6, 1, 2
        };

        int k = 3;
        int n = ar.Count;

        int result = 0;

        for (int i = 0; i < n; i++)
        {
            for (int j = i; j < n; j++)
            {
                if (i == j)
                    continue;

                if ((ar[i] + ar[j]) % k == 0)
                {
                    Console.WriteLine(ar[i] + " " + ar[j]);
                    result++;
                }
            }
        }

        Console.WriteLine(result);
    }
}
