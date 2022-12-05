using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamFormsMVVM.ViewModels;
using XamFormsMVVM.Views;
using XLabs.Forms.Mvvm;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XamFormsMVVM
{
    public partial class App : Application
    {
        public App()
        {
            // Główna strona aplikacji
            RegisterViews();
            //wyjątek
            MainPage = new NavigationPage((ContentPage)ViewFactory.CreatePage<ContactListViewModel, ContactListPage>());
        }
        private void RegisterViews()
        {
            ViewFactory.Register<ContactListPage, ContactListViewModel>();
            ViewFactory.Register<ContactDetailsPage, ContactDetailsViewModel>();
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
