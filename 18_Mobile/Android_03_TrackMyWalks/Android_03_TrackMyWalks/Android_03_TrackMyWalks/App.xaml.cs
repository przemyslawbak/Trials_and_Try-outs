using Android_03_TrackMyWalks.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Android_03_TrackMyWalks
{
    public partial class App : Application
    {
        public App()
        {
            // sprawdzenie platformy
            if (Device.OS == TargetPlatform.Android)
            {
                MainPage = new SplashPage();
            }
            else
            {
                // strona główna aplikacji
                var navPage = new NavigationPage(new WalksPage()
                {
                    Title = "Track My Walks"
                });
                MainPage = navPage;
            }
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
