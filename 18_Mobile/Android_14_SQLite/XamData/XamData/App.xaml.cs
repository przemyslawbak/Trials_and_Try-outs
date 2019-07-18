using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamData.Data;
using XamData.View;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XamData
{
    public partial class App : Application
    {
        public App()
        {
            // The root page of your application
            MainPage = new NavigationPage(new ContactEntry());
        }

        static ContactDatabase database;

        public static ContactDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new ContactDatabase();
                }
                return database;
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
