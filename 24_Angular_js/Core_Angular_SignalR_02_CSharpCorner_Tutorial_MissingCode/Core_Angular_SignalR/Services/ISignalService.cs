using Core_Angular_SignalR.Models;
using System.Threading.Tasks;

namespace Core_Angular_SignalR.Services
{
    public interface ISignalService
    {
        Task<bool> SaveSignalAsync(SignalInputModel inputodel);
    }
}
