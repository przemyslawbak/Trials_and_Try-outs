using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf_Services.Observer
{
    public class Observer2 : IObserver2, IObserverService
    {
        public string ObserverName { get; private set; }
        public Observer2(string name)
        {
            this.ObserverName = name;
        }
        public void Update(int someValue)
        {
            MessageBox.Show("New value: " + someValue.ToString() + " for " + ObserverName);
        }
    }
}
