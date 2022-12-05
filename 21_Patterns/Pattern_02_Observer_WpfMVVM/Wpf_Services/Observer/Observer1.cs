using System.Windows;

namespace Wpf_Services.Observer
{
    public class Observer1 : IObserver1, IObserverService
    {
        public string ObserverName { get; private set; }
        public Observer1(string name)
        {
            this.ObserverName = name;
        }
        public void Update(int someValue)
        {
            MessageBox.Show("New value: " + someValue.ToString() + " for " + ObserverName);
        }
    }
}
