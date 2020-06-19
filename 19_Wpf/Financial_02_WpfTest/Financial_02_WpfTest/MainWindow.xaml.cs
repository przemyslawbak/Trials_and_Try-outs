using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Financial_02_WpfTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() { InitializeComponent(); }
        private void txBox_TextChanged(object sender, TextChangedEventArgs e) { txBlock.Text = txBox.Text; }
        private void btnChangeColor_Click(object sender, RoutedEventArgs e)
        {
            if (txBlock.Foreground == Brushes.Black) txBlock.Foreground = Brushes.Red; else txBlock.Foreground = Brushes.Black;
        }
        private void btnChangeSize_Click(object sender, RoutedEventArgs e) { if (txBlock.FontSize == 11) txBlock.FontSize = 24; else txBlock.FontSize = 11; }
    }
}
