using System.ComponentModel;
using System.Net;
using System.Threading;
using System.Windows;

namespace AsyncWpf
{
    /// <summary>
    /// Listing 6-7. BackgroundWorker for UI Marshaling
    /// </summary>
    public partial class MainWindow : Window
    {
        private WebClient client = new WebClient();
        public MainWindow()
        {
            InitializeComponent();
        }

        public string Text { get; set; }
        public int Value { get; set; }

        private BackgroundWorker backgroundWorker;
        private void StartProcesing(object sender, RoutedEventArgs e)
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += PerformCalculation;
            backgroundWorker.RunWorkerCompleted += CalculationDone;
            backgroundWorker.ProgressChanged += UpdateProgress;
            // Define what behavior is supported by the background worker
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            // On a background thread, fire the DoWork event
            backgroundWorker.RunWorkerAsync();
        }
        private void UpdateProgress(object sender, ProgressChangedEventArgs e)
        {
            this.Value = e.ProgressPercentage;
        }
        private void CalculationDone(object sender, RunWorkerCompletedEventArgs e)
        {
            // If the asynchronous operation completed by throwing an exception
            if (e.Error != null)
                this.Text = e.Error.Message;
            else if (e.Cancelled)
                this.Text = "CANCELLED";
            else
                this.Text = e.Result.ToString();
        }
        private void PerformCalculation(object sender, DoWorkEventArgs e)
        {
            // Dummy Loop to represent some idea of progress in
            // calculating the value
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(50); // Simulating work
                backgroundWorker.ReportProgress(i);
                // Check if cancellation has been requested
                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true; //Indicate the reason for completion is due to cancellation
                    return;
                }
            }
        }
        private void CancelProcessing(object sender, RoutedEventArgs e)
        {
            // Politely ask for the background worker to cancel
            backgroundWorker.CancelAsync();
        }
    }
}
