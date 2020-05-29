using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AsyncWpf
{
    /// <summary>
    /// Listing 6-5. Example of EAP Pattern Client
    /// </summary>
    public partial class MainWindow : Window
    {
        private WebClient client = new WebClient();
        public MainWindow()
        {
            InitializeComponent();
            client.DownloadStringCompleted += ProcessResult;
        }

        public string Text { get; set; }

        private void ProcessResult(object sender, DownloadStringCompletedEventArgs e)
        {
            // event handler running on the UI thread
            if (e.Error != null)
                Text = e.Error.Message;
            else
                Text = e.Result;
        }
        private void DownloadIt(object sender, RoutedEventArgs e)
        {
            // Captures Synchronization Context at this point
            client.DownloadStringAsync(new Uri("http://www.rocksolidknowledge.com/5SecondPage.aspx"));
        }
    }
}
