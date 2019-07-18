using System;
using System.Collections.Generic;
using System.Text;

namespace XamNUnit
{
    public class ReverseService
    {
        public string ReverseWord(string word)
        {
            char[] arr = word.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
    }
}
