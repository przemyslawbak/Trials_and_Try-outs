using System;
using System.Linq;

class Solution
{
    static void Main(String[] args)
    {
        string s = "PPPQQQGGGGGGGGGGGGGGGMMMMMMMMMFFFFFFDDDERT";
        int messageLength = s.Length;
        int wrong = 0;

        for (int i = 0; i < messageLength; i++)
        {
            if ((i % 3) == 1)
            {
                if (s[i] != 'O')
                {
                    wrong++;
                }

                continue;
            }

            if (s[i] == 'S')
            {
                continue;
            }

            wrong++;
        }

        //return wrong
    }
}
