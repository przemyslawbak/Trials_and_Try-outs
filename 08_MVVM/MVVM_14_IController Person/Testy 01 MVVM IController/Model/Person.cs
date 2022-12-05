using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testy_01_MVVM_IController.Model
{
    public enum HeightList : byte { s50, s55, s60, s65, s70, s75, s80, s85 };//enumerable, definicja stałych

    public class Person
    {
        //własności
        public string ID { get; private set; } //własność

        public string FirstName { get; private set; } //własność
        public string SecondName { get; private set; } //własność
        public HeightList Height { get; private set; } //własność

        //konstruktor
        public Person(string id, string firstName, string secondName, HeightList heightList)
        {
            ID = id;
            FirstName = firstName;
            SecondName = secondName;
            Height = heightList;
        }



        public static string NewHeight(HeightList height) //metoda, korzysta konwerter
                                                          //, do konwertera się odwołuje XML przez Converter=...
        {
            switch (height)
            {
                case HeightList.s50:
                    return "150cm";
                case HeightList.s55:
                    return "155cm";
                case HeightList.s60:
                    return "160cm";
                case HeightList.s65:
                    return "165cm";
                case HeightList.s70:
                    return "170cm";
                case HeightList.s75:
                    return "175cm";
                case HeightList.s80:
                    return "180cm";
                case HeightList.s85:
                    return "185cm";
                default:
                    throw new Exception("Nierozpoznana wysokość");
            }
        }

        public static HeightList ParseNewHeight(string opisHeight) //też do konwertera
        {
            switch (opisHeight)
            {
                case "150cm":
                    return HeightList.s50;
                case "155cm":
                    return HeightList.s55;
                case "160cm":
                    return HeightList.s60;
                case "165cm":
                    return HeightList.s65;
                case "170cm":
                    return HeightList.s70;
                case "175cm":
                    return HeightList.s75;
                case "180cm":
                    return HeightList.s80;
                case "185cm":
                    return HeightList.s85;
                default:
                    throw new Exception("Nierozpoznana wysokość");
            }
        }
    }


}
