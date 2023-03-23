﻿using GeoAPI.CoordinateSystems.Transformations;
using GeoAPI.CoordinateSystems;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using Sample;

namespace Activator
{
    //examples from https://developer.trimblemaps.com/engineering-blog/brief-intro-to-nettopology-in-net-core/

    class Program
    {
        static void Main(string[] args)
        {
            var poland = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            TimeSpan polandOffset = poland.GetUtcOffset(DateTime.UtcNow);
            Console.WriteLine(polandOffset);

            var pl = DateTime.UtcNow + polandOffset;
            Console.WriteLine(pl);
        }
    }
}



