using MvvmDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf_.Commands;

namespace Wpf_.ViewModels
{
    public class AddTextDialogViewModel : ViewModelBase, IModalDialogViewModel
    {
        private string text;
        private bool? dialogResult;

        public AddTextDialogViewModel()
        {
            OkCommand = new DelegateCommand(Ok);
        }

        public string Text
        {
            get => text;
            set
            {
                text = value;
                OnPropertyChanged();
            }
        }

        public ICommand OkCommand { get; }

        public bool? DialogResult
        {
            get => dialogResult;
            private set
            {
                dialogResult = value;
                OnPropertyChanged();
            }
        }

        private void Ok(object obj)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                DialogResult = true;
            }
        }
    }
}
