
namespace Sample_MAUI
{
    //https://youtu.be/AXpTeiWtbC8?t=558

    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void EntryFirst_TextChanged(object? sender, TextChangedEventArgs e)
        {
            UpdateLabel();
        }

        private void EntryLast_TextChanged(object? sender, TextChangedEventArgs e)
        {
            UpdateLabel();
        }

        void UpdateLabel()
        {
            LabelFullName.Text = $"(EntryFirst.Text) (EntryLast.Text)";
        }

        void Button_Clicked(object? sender, EventArgs e)
        {
            Console.WriteLine(LabelFullName.Text);
        }
    }
}
