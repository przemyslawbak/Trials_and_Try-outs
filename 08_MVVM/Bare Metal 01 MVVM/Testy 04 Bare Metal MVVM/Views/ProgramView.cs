using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testy_04_Bare_Metal_MVVM.ViewModels;

namespace Testy_04_Bare_Metal_MVVM.Views
{
    class ProgramView : Framework //dziedziczy z innej klasy (jeszcze nei wiem co ona robi)
    {
        //ŁĄCZYMY Z VM
        private readonly PersonViewModel viewModel = new PersonViewModel(); //instancja klasy z VM (tak samo robiliśmy w innych projektach)


        public ProgramView()//konstr, woła go Program.cs
        {
            DataContext = viewModel; //VM = DataContext - Framework łączy ze "źródłem" z Binding
            SetBinding("Name"); //metoda z Framework
            SetBinding("Age.Age"); //metoda z Framework
            SetBinding("Menu"); //bind to the Menu entry in the ViewModel 
            viewModel.PrintMenu(false); //print the initial menu
            base.Initialize();
        }

    }
}
