
namespace Sample_MAUI
{
    //https://www.youtube.com/watch?v=B-5e0PJtSDs
    
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
    }
}
