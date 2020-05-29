using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AsyncWpf
{
    /// <summary>
    /// Listing 6-4. Background Thread Compute, with Continuation on UI Thread
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public string Text { get; set; }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            SynchronizationContext ctx = SynchronizationContext.Current;

            Task.Factory
                .StartNew<decimal>(() => CalculateMeaningOfLifeAsync())
                .ContinueWith(t => Text = t.Result.ToString(),
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private decimal CalculateMeaningOfLifeAsync()
        {
            for (int i = 0; i < 1000000; i++)
            {

            }

            return 48;
        }
    }
}
