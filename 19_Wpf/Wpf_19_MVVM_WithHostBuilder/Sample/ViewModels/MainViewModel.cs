using Sample.Services;

namespace Sample.ViewModels
{
    //source: https://marcominerva.wordpress.com/2020/01/07/using-the-mvvm-pattern-in-wpf-applications-running-on-net-core/
    public class MainViewModel : ViewModelBase
    {
        private readonly ISampleService sampleService;

        public MainViewModel(ISampleService sampleService)
        {
            this.sampleService = sampleService;

            //ExecuteCommand = new RelayCommand(async () => await ExecuteAsync());
        }

        /*private string input;
        public string Input
        {
            get => input;
            set => Set(ref input, value);
        }*/
        //private readonly AppSettings settings;

        //public RelayCommand ExecuteCommand { get; }

        /*private Task ExecuteAsync()
        {
            Debug.WriteLine($"Current value: {input}");
            return Task.CompletedTask;
        }*/
    }
}
