using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Sample_MAUI
{
    public partial class DetailViewModel : ObservableObject, IQueryAttributable
    {
        [ObservableProperty]
        string text;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Text = query["FullName"] as string;
        }

        [RelayCommand]
        async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
