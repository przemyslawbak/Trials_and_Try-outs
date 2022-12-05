using System;

namespace Delegates_03_Event
{
    //https://www.youtube.com/watch?v=KVp_E-hTG0k
    class Program
    {
        static void Main(string[] args)
        {
            var tower = new ClockTower();
            var john = new Person("John", tower);
            tower.ChimeFivePM();
            Console.ReadKey();
        }
    }
    public class Person
    {
        private string _name;
        private ClockTower _tower;
        public Person(string name, ClockTower tower)
        {
            _name = name;
            _tower = tower;
            //teraz musi obserwować event:
            _tower.Chime += (object seder, ClockTowerEventArgs args) =>
            {
                Console.WriteLine("{0} hear the clock chime", _name);
                switch (args.Time)
                {
                    case 5: Console.WriteLine("5 am");
                        break;
                    case 17: Console.WriteLine("5 pm");
                        break;
                }
            };
        }
    }
    public class ClockTowerEventArgs : EventArgs
    {
        public int Time { get; set; }
    }
    //chcemy poinformować osobę o tym że mamy godzin 5AM lub 5PM
    public delegate void ChimeEventHandler(object sender, ClockTowerEventArgs args);
    public class ClockTower
    {
        public event ChimeEventHandler Chime;
        public void ChimeFivePM()
        {
            Chime(this, new ClockTowerEventArgs { Time = 17 });
        }
        public void ChimeFiveAM()
        {
            Chime(this, new ClockTowerEventArgs { Time = 5 } );
        }
    }
}
