using System;
using System.Collections.Generic;
using System.Linq;

class Solution
{
    static void Main(String[] args)
    {
        //reverse cols and rows to have n x n maximum value
        var matrix = new List<List<int>>()
        {
            new List<int> { 112, 42, 83, 119 },
            new List<int> { 56, 125, 56, 49 },
            new List<int> { 15, 78, 101, 43 },
            new List<int> { 62, 98, 114, 108 }
        };

        int rows = matrix.Count();
        int cols = rows;
        int n = rows / 2;
        List<int> results = new List<int>();

        for (int i = 0; i < rows; i++)
        {
            results.Add(ComputeResults(matrix, n));
        }


    }

    private static int ComputeResults(List<List<int>> matrix, int n)
    {
        int result = 0;

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                result = result + matrix[i][j];
            }
        }

        return result;
    }
}
