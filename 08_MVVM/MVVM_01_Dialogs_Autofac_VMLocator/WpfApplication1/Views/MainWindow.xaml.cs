using Autofac;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApplication1.Startup;
using WpfApplication1.ViewModels;

namespace WpfApplication1.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var bootStrapper = new BootStrapper();
            var container = bootStrapper.BootStrap();
            MainWindowViewModel vm = container.Resolve<MainWindowViewModel>();
            InitializeComponent();
            DataContext = vm;



            this.df.Owner = this;
            (this.DataContext as ViewModels.MainWindowViewModel).DialogFacade = this.df;
        }
    }
}
