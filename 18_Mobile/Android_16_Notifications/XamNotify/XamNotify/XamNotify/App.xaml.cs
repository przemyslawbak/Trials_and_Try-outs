using Plugin.LocalNotifications.Abstractions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XamNotify
{
    public partial class App : Application
    {
        public App()
        {
            //pokazuje na pasku
            ILocalNotifications localNotifications = DependencyService.Get<ILocalNotifications>();
            Button showNotificationButton = new Button();
            showNotificationButton.Text = "Pokaż lokalne powiadomienia";
            showNotificationButton.Clicked +=
            (sender, e) => localNotifications.Show("Test", "Lokalne powiadomienie", 1);
            Button cancelNotificationButton = new Button();
            cancelNotificationButton.Text = "Przerwij lokalne powiadomienia";
            cancelNotificationButton.Clicked += (sender, e) => localNotifications.Cancel(1);
            MainPage = new ContentPage
            {
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        showNotificationButton,
                        cancelNotificationButton
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
