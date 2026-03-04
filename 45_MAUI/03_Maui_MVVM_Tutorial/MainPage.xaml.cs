
namespace Sample_MAUI
{
    //https://youtu.be/AXpTeiWtbC8?t=558

    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = new MainViewModel();
        }
    }
}
