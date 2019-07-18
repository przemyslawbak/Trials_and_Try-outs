using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testy_04_Bare_Metal_MVVM.Models
{
    public class MenuItem
    {
        public string Header { get; set; } //własność
        public ICommand Command { get; set; } //komenda powiązana z header
    }

    public interface ICommand
    {
        void Execute(); //wykonanie
    }
}
