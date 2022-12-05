using System;
using System.Linq;

class Solution
{
    static void Main(String[] args)
    {
        string s = "A quick brown fox jumps over the lazy dog";
        string letters = "abcdefghijklmnopqrstuvwxyz";

        string result = !letters.Except(s.ToLower()).Any() ? "pangram" : "not pangram";
    }
}
