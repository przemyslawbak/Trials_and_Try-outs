namespace Financial.ViewModels
{
    public class InputViewModel : ViewModelBase
    {
        public InputViewModel()
        {

        }
        private string _input;
        public string InputText
        {
            get => _input;
            set
            {
                _input = value;
                OnPropertyChanged();
                InputChanged();
            }
        }

        private void InputChanged()
        {
            //
        }
    }
}
