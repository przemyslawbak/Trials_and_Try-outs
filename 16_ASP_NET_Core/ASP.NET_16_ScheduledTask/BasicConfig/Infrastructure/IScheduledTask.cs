using System.Threading;
using System.Threading.Tasks;

namespace BasicConfig.Infrastructure
{
    public interface IScheduledTask
    {
        string Schedule { get; }
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}