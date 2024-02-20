using Services;

namespace Sample
{
    internal interface IApp
    {
        Task Start();
    }
    internal class App : IApp
    {
        private readonly IServiceClass _service;

        public App(IServiceClass service)
        {
            _service = service;
        }

        public async Task Start()
        {
            var dupa = _service.GetDupa();
        }
    }
}
