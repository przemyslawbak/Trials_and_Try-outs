using System;
using System.Collections.Generic;

class Solution
{
    static void Main(String[] args)
    {
        List<string> strings = new List<string>()
        {
            "ab", "ab", "abc"
        };

        List<string> queries = new List<string>()
        {
            "ab", "abc", "bc"
        };

        List<int> result = new List<int>();

        for (int i = 0; i < queries.Count; i++)
        {
            int qty = 0;

            for (int j = 0; j < strings.Count; j++)
            {
                if (queries[i].Equals(strings[j]))
                {
                    qty++;
                }
            }

            result.Add(qty);
        }
    }
}
