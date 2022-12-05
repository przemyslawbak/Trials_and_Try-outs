using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static XamFormsMessagingCenter.App;

namespace XamFormsMessagingCenter
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                throw new NotSupportedException("To trzeba poprawić!");
            }
            catch (NotSupportedException ex)
            {
                MessagingCenter.Send<object, Exception>(this,
                MessagingKey.HandledException.ToString(), ex);
            }
        }
    }
}
