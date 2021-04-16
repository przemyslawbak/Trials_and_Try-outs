using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace List_Comparer
{
    class Program
    {
        static void Main(string[] args)
        {
            string shipsHeader = GenerateHeader();
            SaveShip(shipsHeader);

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

            string shipsLine = GenerateLine(ship);
            SaveShip(shipsLine);
        }

        private static void SaveShip(string shipsLine)
        {
            File.AppendAllText("_results.txt", shipsLine + Environment.NewLine);
        }

        private static string GenerateHeader()
        {
            ShipModel shipdata = new ShipModel();

            StringBuilder sb = new StringBuilder();
            sb.Append(nameof(shipdata.Name));
            sb.Append("|");
            sb.Append(nameof(shipdata.Type));
            sb.Append("|");
            sb.Append(nameof(shipdata.RegisterNo));
            sb.Append("|");
            sb.Append(nameof(shipdata.Imo));
            sb.Append("|");
            sb.Append(nameof(shipdata.CallSign));
            sb.Append("|");
            sb.Append(nameof(shipdata.Flag));
            sb.Append("|");
            sb.Append(nameof(shipdata.PortOfRegistry));
            sb.Append("|");
            sb.Append(nameof(shipdata.Owner));
            sb.Append("|");
            sb.Append(nameof(shipdata.Manager));
            sb.Append("|");
            sb.Append(nameof(shipdata.Propulsion));
            sb.Append("|");
            sb.Append(nameof(shipdata.Hull));
            sb.Append("|");
            sb.Append(nameof(shipdata.Machinery));
            sb.Append("|");
            sb.Append(nameof(shipdata.AnchorEquipment));
            sb.Append("|");
            sb.Append(nameof(shipdata.GRT));
            sb.Append("|");
            sb.Append(nameof(shipdata.NET));
            sb.Append("|");
            sb.Append(nameof(shipdata.DWT));
            sb.Append("|");
            sb.Append(nameof(shipdata.LOA));
            sb.Append("|");
            sb.Append(nameof(shipdata.LBP));
            sb.Append("|");
            sb.Append(nameof(shipdata.Breadth));
            sb.Append("|");
            sb.Append(nameof(shipdata.Depth));
            sb.Append("|");
            sb.Append(nameof(shipdata.Draught));
            sb.Append("|");
            sb.Append(nameof(shipdata.SFB));
            sb.Append("|");
            sb.Append(nameof(shipdata.Shipbuilder));
            sb.Append("|");
            sb.Append(nameof(shipdata.BuilderCountry));
            sb.Append("|");
            sb.Append(nameof(shipdata.YOB));
            sb.Append("|");
            sb.Append(nameof(shipdata.HullMaterial));
            sb.Append("|");
            sb.Append(nameof(shipdata.DecksNo));
            sb.Append("|");
            sb.Append(nameof(shipdata.HoldsNo));
            sb.Append("|");
            sb.Append(nameof(shipdata.MePower));
            sb.Append("|");
            sb.Append(nameof(shipdata.MeType));
            sb.Append("|");
            sb.Append(nameof(shipdata.MeMaker));
            sb.Append("|");
            sb.Append(nameof(shipdata.PropsNo));
            sb.Append("|");
            sb.Append(nameof(shipdata.Generators));
            sb.Append("|");
            sb.Append(nameof(shipdata.Boilers));
            sb.Append("|");
            sb.Append(nameof(shipdata.AirCompressor));
            sb.Append("|");
            sb.Append(nameof(shipdata.Speed));
            sb.Append("|");
            sb.Append(nameof(shipdata.DeckErections));
            sb.Append("|");
            sb.Append(nameof(shipdata.Bulkheads));
            sb.Append("|");
            sb.Append(nameof(shipdata.Ballast));
            sb.Append("|");
            sb.Append(nameof(shipdata.BoilerMaker));
            sb.Append("|");
            sb.Append(nameof(shipdata.CargoHandlingEquipment));
            sb.Append("|");
            sb.Append(nameof(shipdata.CargoSpaceVolume));
            sb.Append("|");
            //
            sb.Append(nameof(shipdata.RadioEquipment));
            sb.Append("|");
            sb.Append(nameof(shipdata.TechManager));
            sb.Append("|");
            sb.Append(nameof(shipdata.Status));
            sb.Append("|");
            sb.Append(nameof(shipdata.Mmsi));
            sb.Append("|");
            sb.Append(nameof(shipdata.OwnerAddress));
            sb.Append("|");
            sb.Append(nameof(shipdata.ManagerAddress));

            return sb.ToString();
        }

        private static string GenerateLine(ShipModel shipdata)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(shipdata.Name);
            sb.Append("|");
            sb.Append(shipdata.Type);
            sb.Append("|");
            sb.Append(shipdata.RegisterNo);
            sb.Append("|");
            sb.Append(shipdata.Imo);
            sb.Append("|");
            sb.Append(shipdata.CallSign);
            sb.Append("|");
            sb.Append(shipdata.Flag);
            sb.Append("|");
            sb.Append(shipdata.PortOfRegistry);
            sb.Append("|");
            sb.Append(shipdata.Owner);
            sb.Append("|");
            sb.Append(shipdata.Manager);
            sb.Append("|");
            sb.Append(shipdata.Propulsion);
            sb.Append("|");
            sb.Append(shipdata.Hull);
            sb.Append("|");
            sb.Append(shipdata.Machinery);
            sb.Append("|");
            sb.Append(shipdata.AnchorEquipment);
            sb.Append("|");
            sb.Append(shipdata.GRT);
            sb.Append("|");
            sb.Append(shipdata.NET);
            sb.Append("|");
            sb.Append(shipdata.DWT);
            sb.Append("|");
            sb.Append(shipdata.LOA);
            sb.Append("|");
            sb.Append(shipdata.LBP);
            sb.Append("|");
            sb.Append(shipdata.Breadth);
            sb.Append("|");
            sb.Append(shipdata.Depth);
            sb.Append("|");
            sb.Append(shipdata.Draught);
            sb.Append("|");
            sb.Append(shipdata.SFB);
            sb.Append("|");
            sb.Append(shipdata.Shipbuilder);
            sb.Append("|");
            sb.Append(shipdata.BuilderCountry);
            sb.Append("|");
            sb.Append(shipdata.YOB);
            sb.Append("|");
            sb.Append(shipdata.HullMaterial);
            sb.Append("|");
            sb.Append(shipdata.DecksNo);
            sb.Append("|");
            sb.Append(shipdata.HoldsNo);
            sb.Append("|");
            sb.Append(shipdata.MePower);
            sb.Append("|");
            sb.Append(shipdata.MeType);
            sb.Append("|");
            sb.Append(shipdata.MeMaker);
            sb.Append("|");
            sb.Append(shipdata.PropsNo);
            sb.Append("|");
            sb.Append(shipdata.Generators);
            sb.Append("|");
            sb.Append(shipdata.Boilers);
            sb.Append("|");
            sb.Append(shipdata.AirCompressor);
            sb.Append("|");
            sb.Append(shipdata.Speed);
            sb.Append("|");
            sb.Append(shipdata.DeckErections);
            sb.Append("|");
            sb.Append(shipdata.Bulkheads);
            sb.Append("|");
            sb.Append(shipdata.Ballast);
            sb.Append("|");
            sb.Append(shipdata.BoilerMaker);
            sb.Append("|");
            sb.Append(shipdata.CargoHandlingEquipment);
            sb.Append("|");
            sb.Append(shipdata.CargoSpaceVolume);
            sb.Append("|");
            sb.Append(shipdata.RadioEquipment);
            sb.Append("|");
            sb.Append(shipdata.TechManager);
            sb.Append("|");
            sb.Append(shipdata.Status);
            sb.Append("|");
            sb.Append(shipdata.Mmsi);
            sb.Append("|");
            sb.Append(shipdata.OwnerAddress);
            sb.Append("|");
            sb.Append(shipdata.ManagerAddress);

            return sb.ToString();
        }
    }
}
