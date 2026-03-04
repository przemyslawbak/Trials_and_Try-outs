using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Sample_MAUI
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FullName))]
        string firstName;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FullName))]
        string lastName;

        public string FullName => $"{FirstName} {LastName}";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        bool isBusy;

        public bool IsNotBusy => !IsBusy;

        [RelayCommand]
        void Tap()
        {
            IsBusy = true;

            Console.WriteLine(FullName);

            IsBusy = false;
        }

        [RelayCommand]
        async Task Go()
        {
            await Shell.Current.GoToAsync(nameof(DetailPage), true, 
                new Dictionary<string, object>()
                {
                    {"FullName", FullName },
                });
        }
    }
}
