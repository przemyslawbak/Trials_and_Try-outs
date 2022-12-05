using System;
using System.Collections.Generic;
using System.Linq;

class Solution
{
    static void Main(String[] args)
    {
        List<int> s = new List<int>() { 4 };
        int d = 4;
        int m = 1;

        int ways = 0;
        for (int i = 0; i < s.Count(); i++)
        {
            if ((i + m) > s.Count())
            {
                break;
            }

            int sum = 0;

            for (int j = 0; j < m; j++)
            {
                sum = sum + s[i + j];
            }

            if (sum == d)
            {
                ways++;
            }
        }


    }
}
