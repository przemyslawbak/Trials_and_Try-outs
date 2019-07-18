using MVVM_09_Prism.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_09_Prism
{
    class Program
    {
        static void Main(string[] args)
        {
            MainViewModel mainViewModel = new MainViewModel();
            Child1ViewModel child1 = new Child1ViewModel();
            Child2ViewModel child2 = new Child2ViewModel();
            mainViewModel.UpdateName("Name1");

            Console.WriteLine(child1.Name);
            Console.WriteLine(child2.Name);

            Console.ReadKey();
        }
    }
}
