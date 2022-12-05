using GeoAPI.CoordinateSystems.Transformations;
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

            var polygon = new PointModel[] { //Baltic Sea coordinates
                new PointModel() { X = 37.8, Y = 62.6 },
                new PointModel() { X = 19, Y = 52.7 },
                new PointModel() { X = 9.3, Y = 54.2 },
                new PointModel() { X = 10.8, Y = 58.1 },
                new PointModel() { X = 27.1, Y = 67.9 },
                new PointModel() { X = 23.2, Y = 62},
            };

            var point = new PointModel() { X = 30.8, Y = 61.7 };

        }

        public LineString SimpleLineString()
        {
            //A simple line connected from one coordinate to the next
            //Example: A route that goes from LA to San Fran
            //Note: Have to have at least two points you know because its a LINEstring

            Coordinate coord1 = new Coordinate(74.6523332, 21.213213);
            Coordinate coord2 = new Coordinate(80.2321312, 25.563213);
            Coordinate coord3 = new Coordinate(85.6522352, 25.983223);

            Coordinate[] coordArr = new Coordinate[] { coord1, coord2, coord3 };
            return new LineString(coordArr);
        }

        public Polygon SimplePolygon()
        {
            //A polygon as you may know is a enclosed linestring (it has a name yes)
            // Now you can represent many different types of polygons
            var polygon = new Polygon(new LinearRing(new Coordinate[]
            {
                new Coordinate(1.0, 1.0),
                new Coordinate(1.05, 1.1),
                new Coordinate(1.1, 1.1),
                new Coordinate(1.1, 1.05),
                new Coordinate(1, 1),
            }));
            return polygon;
        }

        public MultiLineString MultiLineString()
        {
            //Multi...Line Strings are you guessed it just a few line strings
            //Jammed into an array
            //Ex: LA to San Fran is one route and then San Fran to Portland can
            // be another route but stored into one data structure

            LineString ls1 = SimpleLineString();
            LineString ls2 = SimpleLineString();
            LineString ls3 = SimpleLineString();

            LineString[] lsArr = new LineString[] { ls1, ls2, ls3 };
            MultiLineString mls = new MultiLineString(lsArr);

            var lineStr = mls[0]; //Return Geometry You can cast to LineString
            lineStr = (LineString)lineStr;

            return mls;
        }

        public MultiPolygon SimpleMultiPolygon()
        {
            //This one is ez just a bunch of polygons....jammed into an array! allows you
            //to bundle your polygons
            Polygon p1 = SimplePolygon();
            Polygon p2 = SimplePolygon();
            Polygon p3 = SimplePolygon();

            MultiPolygon multiPolygon = new MultiPolygon(new Polygon[]
            {
        p1, p2, p3
            });
            return multiPolygon;
        }

        //is point inside polygon?
        //https://github.com/NetTopologySuite/NetTopologySuite/issues/264
        //https://stackoverflow.com/questions/53820355/fast-find-if-points-belong-to-polygon-nettopologysuite-geometries-c-net-cor


        //MORE:
        //compare 2 coordinates
        //distances between points
        //distances between geometries
    }
}



