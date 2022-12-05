using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AsyncWpf
{
    //Listing 6-11/12. Updating ObservableCollection via SynchronizationContext. Updating ObservableCollection Using Collection-Based Locking
    public class ViewModel
    {
        private object valuesLock = new object();
        private SynchronizationContext uiCtx = SynchronizationContext.Current; //w .NET 4.5 nie jest potrzebne

        public ViewModel()
        {
            Values = new ObservableCollection<int>();
            //To request that the data binding performs synchronization prior to accessing the collection, make
            //a call to the static method BindingOperations.EnableCollectionSynchronization
            BindingOperations.EnableCollectionSynchronization(Values, valuesLock);
            Task.Factory.StartNew(GenerateValues);
        }

        private void GenerateValues()
        {
            var rnd = new Random();

            while (true)
            {
                uiCtx.Post(_ => Values.Add(rnd.Next(1, 100)), null);
                lock (Values)
                    Values.Add(rnd.Next(1, 100));
                Thread.Sleep(1000);
            }
        }

        public ObservableCollection<int> Values { get; set; }
    }
}
