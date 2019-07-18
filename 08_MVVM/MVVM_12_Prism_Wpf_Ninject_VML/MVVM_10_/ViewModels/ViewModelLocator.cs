using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_10_.ViewModels
{
    public class ViewModelLocator
    {
        public MainWindowViewModel MainWindowViewModel
        {
            get { return IocKernel.Get<MainWindowViewModel>(); }
        }

        public ChildView1ViewModel ChildView1ViewModel
        {
            get { return IocKernel.Get<ChildView1ViewModel>(); }
        }
    }
}
