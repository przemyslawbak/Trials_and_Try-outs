using Ninject.Modules;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_10_.Startup
{
    public class IocConfiguration : NinjectModule
    {
        public override void Load()
        {
            Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope(); // Reuse same storage every time
        }
    }
}
