using Microsoft.Extensions.DependencyInjection;

namespace Sample.ViewModels
{
    public class ViewModelLocator
    {
        public MainViewModel MainViewModel
        => App.ServiceProvider.GetRequiredService<MainViewModel>();
    }
}
