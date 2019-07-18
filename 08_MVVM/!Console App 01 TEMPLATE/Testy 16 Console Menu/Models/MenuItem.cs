using System;
using System.Collections.Generic;
using System.Text;

namespace Testy_16_Console_Menu.Models
{
    class MenuItem
    {
        public string Header { get; set; } //property
        public ICommand Command { get; set; } //command binded to the header
    }
}
