using System;
using System.Collections.Generic;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> scores = new List<int>()
            {
                10, 5, 20, 20, 4, 5, 2, 25, 1
            };

            List<int> res = new List<int>();

            int most = 0;
            int least = 0;

            int max = int.MinValue;
            int min = int.MaxValue;

            foreach (int score in scores)
            {
                if (score > max)
                {
                    max = score;
                    most++;
                }

                if (score < min)
                {
                    min = score;
                    least++;
                }
            }
            res.Add(most - 1);
            res.Add(least - 1);

            Console.WriteLine((most - 1) + " " + (least - 1));
        }
    }
}
