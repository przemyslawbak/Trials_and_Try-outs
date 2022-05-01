using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;

class Solution
{
    static void Main(String[] args)
    {
        List<string[]> list = new List<string[]>();
        int times = 5;

        for (int i = 0; i < times; i++)
        {
            try
            {
                list.Add(Console.ReadLine().Split(';')); //NOTE: various input rows
            }
            catch
            {

            }
        }

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i][0] == "S")
            {
                if (list[i][1] == "M")
                {
                    list[i][2] = list[i][2].Replace("()", "");
                }

                string[] split = Regex.Split(list[i][2], @"(?<!^)(?=[A-Z])");
                Console.WriteLine(string.Join(" ", split).ToLower());

            }
            else
            {
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                if (list[i][1] == "M")
                {
                    list[i][2] = list[i][2] + "()";
                }

                list[i][2] = textInfo.ToTitleCase(list[i][2]).Replace(" ", "");

                if (list[i][1] != "C")
                {
                    list[i][2] = Char.ToLower(list[i][2][0]) + list[i][2].Substring(1);
                }

                Console.WriteLine(list[i][2]);
            }
        }
    }
}
