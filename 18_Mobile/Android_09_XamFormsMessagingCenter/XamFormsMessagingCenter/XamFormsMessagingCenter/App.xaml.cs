using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XamFormsMessagingCenter
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            new CrashManager();
            MainPage = new MainPage();
        }
        public enum MessagingKey
        {
            HandledException = 0
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
