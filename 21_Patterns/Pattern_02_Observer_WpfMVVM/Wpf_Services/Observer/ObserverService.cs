using System.Windows;

namespace Wpf_Services.Observer
{
    public class ObserverService : IObserverService
    {
        public string ObserverName { get; private set; }
        public ObserverService(string name)
        {
            this.ObserverName = name;
        }
        public void Update(int someValue)
        {
            MessageBox.Show("New value: " + someValue.ToString() + " for " + ObserverName);
        }
    }
}
