using Microsoft.Extensions.Configuration;
using Sample.Services;

namespace Sample.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ISampleService _sampleService;
        private readonly IConfiguration _config;

        public MainViewModel(ISampleService sampleService, IConfiguration config)
        {
            _sampleService = sampleService;
            _config = config;

            var urlBase = config.GetSection("UrlBase").Value;
        }
    }
}
