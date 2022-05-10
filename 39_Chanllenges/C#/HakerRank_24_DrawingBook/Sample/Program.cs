using System;
using System.Collections.Generic;
using System.Linq;

class Solution
{
    static void Main(String[] args)
    {
        int n = 6;
        int p = 5;

        int result = p / 2 < ((n % 2 == 1 ? n : n + 1) - p) / 2 ? p / 2 : ((n % 2 == 1 ? n : n + 1) - p) / 2;
    }
}
