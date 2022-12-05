using System;
using System.Collections.Generic;
using System.Linq;

class Solution
{
    static void Main(String[] args)
    {
        List<int> sticks = new List<int>() { 3,9,2,15,3 };

        List<List<int>> results = new List<List<int>>();

        List<List<int>> combs = GetDifferentCombinations(sticks, 3).Select(a => a.ToList()).ToList();

        results = combs.OrderBy(a => a.OrderBy(b => b).FirstOrDefault()).OrderByDescending(a => (a[0] + a[1] + a[2])).Where(a => VerifyConditions(a[0], a[1], a[2])).ToList();

        if (results.Count == 0)
        {
            results.Add(new List<int>() { -1 });
        }

        List<int> final = results.FirstOrDefault().OrderBy(a => a).ToList();
    }

    private static void InitIndexes(int[] indexes)
    {
        for (int i = 0; i < indexes.Length; i++)
        {
            indexes[i] = i;
        }
    }

    private static void SetIndexes(int[] indexes, int lastIndex, int count)
    {
        indexes[lastIndex]++;
        if (lastIndex > 0 && indexes[lastIndex] == count)
        {
            SetIndexes(indexes, lastIndex - 1, count - 1);
            indexes[lastIndex] = indexes[lastIndex - 1] + 1;
        }
    }

    private static List<T> TakeAt<T>(int[] indexes, IEnumerable<T> list)
    {
        List<T> selected = new List<T>();
        for (int i = 0; i < indexes.Length; i++)
        {
            selected.Add(list.ElementAt(indexes[i]));
        }
        return selected;
    }

    private static bool AllPlacesChecked(int[] indexes, int places)
    {
        for (int i = indexes.Length - 1; i >= 0; i--)
        {
            if (indexes[i] != places)
                return false;
            places--;
        }
        return true;
    }

    public static IEnumerable<List<T>> GetDifferentCombinations<T>(IEnumerable<T> collection, int count)
    {
        int[] indexes = new int[count];
        int listCount = collection.Count();
        if (count > listCount)
            throw new InvalidOperationException($"{nameof(count)} is greater than the collection elements.");
        InitIndexes(indexes);
        do
        {
            var selected = TakeAt(indexes, collection);
            yield return selected;
            SetIndexes(indexes, indexes.Length - 1, listCount);
        }
        while (!AllPlacesChecked(indexes, listCount));

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
