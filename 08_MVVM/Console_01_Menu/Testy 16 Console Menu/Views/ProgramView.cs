using System;
using System.Collections.Generic;
using System.Text;
using Testy_16_Console_Menu.ViewModels;

namespace Testy_16_Console_Menu.Views
{
    class ProgramView
    {
        private readonly MenuViewModel viewModel = new MenuViewModel();
        public ProgramView()
        {
            viewModel.PrintMenu(false); //print the initial menu
            viewModel.InputLoop();
        }
    }
}
