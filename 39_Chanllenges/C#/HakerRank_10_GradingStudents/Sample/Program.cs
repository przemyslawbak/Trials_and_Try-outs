using System;
using System.Collections.Generic;
using System.Linq;

class Solution
{
    static void Main(String[] args)
    {
        List<int> grades = new List<int>()
        {
            23, 80, 96, 18, 73, 78, 60, 60, 15, 45, 15, 10, 5, 46, 87, 33, 60, 14, 71, 65, 2, 5, 97, 0
        };

        List<int> results = grades
            .Select(g => ((5 * (int)Math.Ceiling(((double)g) / 5)) - g) < 3 ? g < 38 ? g : (5 * (int)Math.Ceiling(((double)g) / 5)) : g)
            .ToList();
    }
}
