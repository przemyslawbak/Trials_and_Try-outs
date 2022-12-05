using Microsoft.Extensions.DependencyInjection;
using System;

namespace BasicConfig.Services
{
    public class ServicesProvider : IServicesProvider
    {
        private readonly IServiceProvider _provider;

        public ServicesProvider(IServiceProvider provider)
        {
            _provider = provider;
        }

        public ISomeSerice Get(string clientName)
        {
            ISomeSerice client = null;

            if (clientName == "4programmers")
            {
                client = _provider.GetService<ISomeSerice>();
            }

            return client;
        }
    }
}
