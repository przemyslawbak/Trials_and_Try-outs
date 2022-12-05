using System;
using System.Collections.Generic;
using System.Text;
using Testy_16_Console_Menu.ViewModels;

namespace Testy_16_Console_Menu.Views
{
    /*installed libraries:
     * Microsoft.EntityFrameworkCore.SqlServer
     * System.Data.SqlClient
     * Microsoft.NETCore.App (by default)
     */
    class ProgramView
    {
        private readonly MenuViewModel viewModel = new MenuViewModel();
        public ProgramView()
        {
            viewModel.PrintMenu(false); //print the initial menu
            viewModel.InputLoop(); //executes loop method for taking the input
        }
    }
}
