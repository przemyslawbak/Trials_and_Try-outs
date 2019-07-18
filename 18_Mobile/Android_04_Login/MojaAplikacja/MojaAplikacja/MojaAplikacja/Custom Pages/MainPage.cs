using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace MojaAplikacja.Custom_Pages
{
    public class MainPage : ContentPage
    {
        Entry userNameEntry;
        Entry passwordEntry;
        Button loginButton;
        StackLayout stackLayout;
        public MainPage()
        {
            userNameEntry = new Entry
            {
                Placeholder = "login"
            };
            passwordEntry = new Entry
            {
                Placeholder = "hasło",
                IsPassword = true
            };
            loginButton = new Button
            {
                Text = "Zaloguj"
            };
            loginButton.Clicked += LoginButton_Clicked;
            this.Padding = new Thickness(20);
            stackLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
                Spacing = 10,
                Children = {
userNameEntry,
passwordEntry,
loginButton
}
            };
            this.Content = stackLayout;
        }
        void LoginButton_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine(string.Format("Login: {0} - Hasło: {1}",
            userNameEntry.Text, passwordEntry.Text));
        }
    }
}
