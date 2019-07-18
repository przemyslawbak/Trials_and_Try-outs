using System;
using System.Collections.Generic;
using System.Text;

namespace Testy_16_Console_Menu.Models
{
    public interface ICommand //simplified ICommand interface that just exposes the Execute method from the WPF version
    {
        void Execute();
    }
}
