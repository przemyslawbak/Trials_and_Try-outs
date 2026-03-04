
namespace Sample_MAUI
{
    //https://www.youtube.com/watch?v=5Qga2pniN78&list=PLdo4fOcmZ0oUBAdL2NwBpDs32zwGqb9DY&index=5

    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();

            BindingContext = vm;
        }
    }
}
