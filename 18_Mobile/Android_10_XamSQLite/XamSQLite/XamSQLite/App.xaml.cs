using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XamSQLite
{
    public partial class App : Application
    {
        public App()
        {
            // Główna strona aplikacji
            ISQLiteConnection connection = DependencyService.Get<ISQLiteConnection>();
            if (connection.GetConnection() != null)
            {
                Debug.WriteLine("Połączenie SQLite gotowe!");
            }
            MainPage = new ContentPage
            {
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        new Label {
                        HorizontalTextAlignment = TextAlignment.Center,
                        Text = connection.GetDataBasePath()
                        }
                    }
                }
            };
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
