using System;
using System.Collections.Generic;
using System.Linq;

class Solution
{
    static void Main(String[] args)
    {
        int n = 7;
        List<int> ar = new List<int>() { 1,2,1,2,1,3,2 };
        List<int> prs = new List<int>(ar);
        int pairs = 0;

        for (int i = 0; i < ar.Count; i++)
        {
            int searched = ar[i];
            int itemi = prs.Where(p => p == ar[i]).FirstOrDefault();
            prs.Remove(itemi);

            for (int j = 0; j < prs.Count; j++)
            {
                if (searched == prs[j])
                {
                    int itemj = prs.Where(p => p == prs[j]).FirstOrDefault();
                    prs.Remove(itemj);
                    pairs++;
                    break;
                }
            }
        }
    }
}
