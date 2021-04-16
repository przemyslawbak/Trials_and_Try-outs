using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace List_Comparer
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> inputLines = new List<string>(File.ReadAllLines("_input.csv"));

            for (int i = 0; i < inputLines.Count; i++)
            {
                if (inputLines[i][0] == '1')
                {
                    string[] shipLines = new[] {
                        inputLines[i],
                        inputLines[i + 1],
                        inputLines[i + 2],
                        inputLines[i + 3],
                        inputLines[i + 4],
                        inputLines[i + 5],
                        inputLines[i + 6],
                        inputLines[i + 7],
                        inputLines[i + 8],
                        inputLines[i + 9],
                        inputLines[i + 10],
                        inputLines[i + 11]
                    };

                    ParseShipAndSave(shipLines);
                }
            }

        }

        private static void ParseShipAndSave(string[] shipLines)
        {
            ShipModel ship = new ShipModel();
            List<string> cells = new List<string>();
            List<KeyValuePair<int, string>> pairs = new List<KeyValuePair<int, string>>();

            foreach (string line in shipLines)
            {
                cells.AddRange(line.Split(';'));
            }

            string[] cellHeads = new string[] { "1", "2", "3", "4", "5", "6", "11" };
            for (int i = 0; i < cells.Count; i++)
            {
                if (cellHeads.Any(cells[i].Equals))
                {
                    pairs.Add(new KeyValuePair<int, string>(int.Parse(cells[i]), cells[i + 1]));
                }
            }

            //1
            ship.RegisterNo = pairs.Where(kvp => kvp.Key == 1).ToList()[0].Value;
            ship.Name = pairs.Where(kvp => kvp.Key == 1).ToList()[1].Value;
            ship.GRT = pairs.Where(kvp => kvp.Key == 1).ToList()[2].Value;
            ship.Hull = pairs.Where(kvp => kvp.Key == 1).ToList()[3].Value;
            ship.YOB = pairs.Where(kvp => kvp.Key == 1).ToList()[4].Value.Split('-')[0];
            ship.Shipbuilder = pairs.Where(kvp => kvp.Key == 1).ToList()[5].Value.Split(',')[0];
            ship.BuilderCountry = pairs.Where(kvp => kvp.Key == 1).ToList()[5].Value.Split(',')[1];

            //2
            ship.Imo = pairs.Where(kvp => kvp.Key == 2).ToList()[0].Value;
            ship.NET = pairs.Where(kvp => kvp.Key == 2).ToList()[2].Value;
            ship.MeType = pairs.Where(kvp => kvp.Key == 2).ToList()[3].Value;

            //3
            ship.Flag = pairs.Where(kvp => kvp.Key == 3).ToList()[0].Value;
            ship.DWT = pairs.Where(kvp => kvp.Key == 3).ToList()[1].Value;
            ship.Type = pairs.Where(kvp => kvp.Key == 3).ToList()[1].Value; //???????????????????
            ship.Manager = pairs.Where(kvp => kvp.Key == 3).ToList()[4].Value;

            //4
            ship.LBP = pairs.Where(kvp => kvp.Key == 4).ToList()[0].Value;
            ship.CallSign = pairs.Where(kvp => kvp.Key == 4).ToList()[2].Value;
            ship.MePower = pairs.Where(kvp => kvp.Key == 4).ToList()[3].Value;
            ship.Owner = pairs.Where(kvp => kvp.Key == 4).ToList()[4].Value;

            //5
            ship.LOA = pairs.Where(kvp => kvp.Key == 5).ToList()[0].Value;
            ship.DecksNo = pairs.Where(kvp => kvp.Key == 5).ToList()[0].Value; //???????????????????
            ship.Boilers = pairs.Where(kvp => kvp.Key == 5).ToList()[3].Value;
            ship.PortOfRegistry = pairs.Where(kvp => kvp.Key == 5).ToList()[4].Value;

            //6
            ship.Breadth = pairs.Where(kvp => kvp.Key == 6).ToList()[0].Value;
            ship.Speed = pairs.Where(kvp => kvp.Key == 6).ToList()[1].Value; //???????????????????
            ship.Mmsi = pairs.Where(kvp => kvp.Key == 6).ToList()[2].Value;
            ship.Generators = pairs.Where(kvp => kvp.Key == 6).ToList()[3].Value;

            //7
            ship.Depth = pairs.Where(kvp => kvp.Key == 7).ToList()[0].Value;
            ship.HoldsNo = pairs.Where(kvp => kvp.Key == 7).ToList()[1].Value;

            //8
            ship.Draught = pairs.Where(kvp => kvp.Key == 8).ToList()[0].Value;

            //11
            ship.CargoHandlingEquipment = pairs.Where(kvp => kvp.Key == 11).ToList()[0].Value;
        }
    }
}
