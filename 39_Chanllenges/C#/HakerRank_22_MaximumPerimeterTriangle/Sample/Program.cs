using System;
using System.Collections.Generic;
using System.Linq;

class Solution
{
    static void Main(String[] args)
    {
        List<int> sticks = new List<int>() { 3,9,2,15,3 };

        List<List<int>> results = new List<List<int>>();

        List<List<int>> combs = GetPermutations(sticks, 3).Select(a => a.ToList()).ToList();

        results = combs.OrderBy(a => a.OrderBy(b => b).FirstOrDefault()).OrderByDescending(a => (a[0] + a[1] + a[2])).Where(a => VerifyConditions(a[0], a[1], a[2])).ToList();

        if (results.Count == 0)
        {
            results.Add(new List<int>() { -1 });
        }

        List<int> final = results.FirstOrDefault();
    }

    static IEnumerable<IEnumerable<T>> //update: K-combinations
    GetPermutations<T>(IEnumerable<T> list, int length)
    {
        if (length == 1) return list.Select(t => new T[] { t });
        return GetPermutations(list, length - 1)
            .SelectMany(t => list.Where(o => !t.Contains(o)),
                (t1, t2) => t1.Concat(new T[] { t2 }));
    }

    private static bool VerifyConditions(int v1, int v2, int v3)
    {
        if (((v1 + v2) > v3) && ((v1 + v3) > v2) && ((v2 + v3) > v1))
        {
            return true;
        }

        return false;
    }
}
