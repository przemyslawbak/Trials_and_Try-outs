using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AsyncWpf
{
    /// <summary>
    ///
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public string Text { get; set; }

        private async void ButtonClick(object sender, RoutedEventArgs e) //not blocking UI
        {
            try
            {
                decimal result = await CalculateMeaningOfLife();
                Text = result.ToString();
            }
            catch (Exception error)
            {
                Text = error.Message;
            }
        }

        private async Task<decimal> CalculateMeaningOfLife()
        {
            await Task.Delay(10000);

            return 48;
        }
    }
}
