using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AsyncWpf
{
    /// <summary>
    /// Listing 6-2. Asynchronous Work Initated from the UI Thread
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
            Task.Factory.StartNew(() =>
            {
                decimal result = CalculateMeaningOfLifeAsync();
                Text = result.ToString();
            });
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
