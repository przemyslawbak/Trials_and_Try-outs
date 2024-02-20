using Microsoft.Extensions.Options;
using Models;

namespace Services
{
    public interface IServiceClass
    {
        object GetDupa();
    }

    public class ServiceClass : IServiceClass
    {
        private TransientFaultHandlingOptions _config;

        public ServiceClass(IOptions<TransientFaultHandlingOptions> config)
        {
            _config = config.Value;
        }

        public object GetDupa()
        {
            return _config.AutoRetryDelay;
        }
    }

}
