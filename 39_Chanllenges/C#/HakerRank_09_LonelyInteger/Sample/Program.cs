using System;
using System.Collections.Generic;
using System.Linq;

class Solution
{
    static void Main(String[] args)
    {
        List<int> a = new List<int>()
        {
            1,2,3,4,3,2,1
        };

        int result = a.GroupBy(x => x)
              .Where(g => g.Count() == 1)
              .Select(y => y.Key).FirstOrDefault();
    }
}
