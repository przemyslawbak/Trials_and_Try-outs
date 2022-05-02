using System;
using System.Buffers.Binary;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

class Solution
{
    static void Main(String[] args)
    {
        List<int> arr = new List<int>()
        {
            1,4,6,3,2,7,4,2,1,6,4,3,2,8,6,2
        };

        int[] range = new int[100];

        foreach (int value in arr)
        {
            range[value]++;
        }

       range.ToList();
    }
}
