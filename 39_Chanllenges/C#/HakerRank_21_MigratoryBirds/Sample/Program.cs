using System;
using System.Collections.Generic;
using System.Linq;

class Solution
{
    static void Main(String[] args)
    {
        List<int> arr = new List<int>() { 1, 1,1, 2, 2,2, 3,3,3,3 };

        int result = arr
            .OrderBy(id => id)
            .GroupBy(id => id)
            .OrderByDescending(id => id.Count())
            .Select(g => g.Key)
            .FirstOrDefault();
    }
}
