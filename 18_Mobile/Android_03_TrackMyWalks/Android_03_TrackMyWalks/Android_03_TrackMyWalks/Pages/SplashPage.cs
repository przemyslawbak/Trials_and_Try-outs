using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Android_03_TrackMyWalks.Pages
{
	public class SplashPage : ContentPage
	{
        public SplashPage()
        {
            AbsoluteLayout splashLayout = new AbsoluteLayout
            {
                HeightRequest = 600
            };
            var image = new Image()
            {
                Source = ImageSource.FromFile("icon.png"),
                Aspect = Aspect.AspectFill,
            };
            AbsoluteLayout.SetLayoutFlags(image, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(image, new Rectangle(0f, 0f,
            1f, 1f));
            splashLayout.Children.Add(image);
            Content = new StackLayout()
            {
                Children = { splashLayout }
            };
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // kilkusekundowe opóźnienie wyłączenia ekranu startowego
            await Task.Delay(3000);
            // utworzenie egzemplarza NavigationPage i przypisanie go do MainPage
            var navPage = new NavigationPage(new WalksPage()
            {
                Title = "Track My Walks"
            });
            Application.Current.MainPage = navPage;
        }

    }
}