using Plugin.FilePicker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FilePicker
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            MyButton.Clicked += MyButton_Clicked;
        }

        private async void MyButton_Clicked(object sender, EventArgs e)
        {
            var file = await CrossFilePicker.Current.PickFile();
            if (file != null)
            {
                MyLabel.Text = file.FileName;
            }
        }
    }
}
