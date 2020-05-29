using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Listing 6-3. Using SynchronizationContext to Marshal Back onto the UI Thread
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

            Task.Factory.StartNew(() =>
            {
                decimal result = CalculateMeaningOfLifeAsync();

                // request that the synchronization context object
                // runs Text = result.ToString() on the UI thread asynchronsly
                ctx.Post(state => Text = result.ToString(), null);
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
