using System;
using System.Collections.Generic;
using System.Linq;

class Solution
{
    static void Main(String[] args)
    {
        List<int> A = new List<int>() { 1, 2, 3 };
        List<int> B = new List<int>() { 9,8,7 };
        int k = 5;

        string result = "NO";

        bool flag = false;
        int count = 0;
        for (int i = 0; i < A.Count; i++)
        {
            int saveB = int.MaxValue, indexB = -1;
            flag = false;
            for (int j = 0; j < B.Count; j++)
            {
                if (B[j] != -1 && (A[i] + B[j]) >= k)
                {
                    if (!flag)
                    {
                        count++;
                        flag = true;
                    }
                    if (saveB > B[j])
                    {
                        if (indexB != -1)
                        {
                            B[indexB] = saveB;
                        }
                        saveB = B[j];
                        indexB = j;
                        B[indexB] = -1;
                    }
                }
            }
            if (count == A.Count)
            {
                result = "YES";
            }
        }
    }
}
