using System;
using System.Collections.Generic;
using System.Linq;

namespace ToList
{
    class Program
    {
        static void Main(string[] args)
        {
            var sampleList = new List<SampleModel>()
            {
                new SampleModel() { SampleName = "name1", SampleValue = 0.5M },
                new SampleModel() { SampleName = "name2", SampleValue = 0.8M },
                new SampleModel() { SampleName = "name3", SampleValue = 0.3M },
                new SampleModel() { SampleName = "name1", SampleValue = 1.5M },
            };

            var res = sampleList.GroupBy(x => x.SampleName).Select(x => new DataModel()
            {
                ResultValue = x.Average(y => (decimal)y.SampleValue),
                ResultName = x.Select(y => y.SampleName + "_res").First(),
            }).ToList();

            Console.ReadKey();
        }
    }
}
