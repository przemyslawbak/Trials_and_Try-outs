using System;
using System.Buffers.Binary;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

class Solution
{
    static void Main(String[] args)
    {
        List<List<int>> arr = new List<List<int>>()
        {
            new List<int>() { 11, 2, 4 },
            new List<int>() { 4, 5, 6 },
            new List<int>() { 10, 8, -12 }
        };

        int sum1 = 0;
        int sum2 = 0;

        for (int i = 0; i < arr.Count; i++)
        {
            sum1 = sum1 + arr[i][i];
            sum2 = sum2 + arr[i][arr.Count - i - 1];
        }

        var res = Math.Abs(sum1 - sum2);
    }
}
