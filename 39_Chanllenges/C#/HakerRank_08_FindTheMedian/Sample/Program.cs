using System;
using System.Collections.Generic;
using System.Linq;

class Solution
{
    static void Main(String[] args)
    {
        List<int> arr = new List<int>()
        {
            0,1,2,3,4,6,5,3
        };

        arr = arr.OrderBy(l => l).ToList();

        int median = arr[(arr.Count - 1) / 2];
        Console.WriteLine(median);
    }
}
