using System;
using System.Threading.Tasks;
using Core_Angular_SignalR.Models;
using Core_Angular_SignalR.Presistance;

namespace Core_Angular_SignalR.Services
{
    public class SignalService : ISignalService
    {
        private readonly MainDbContext _mainDbContext;

        public SignalService(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<bool> SaveSignalAsync(SignalInputModel inputodel)
        {
            try
            {
                SignalDataModel signalModel = new SignalDataModel();
                signalModel.CustomerName = inputodel.CustomerName;
                signalModel.Description = inputodel.Description;
                signalModel.AccessCode = inputodel.AccessCode;
                signalModel.Area = inputodel.Area;
                signalModel.Zone = inputodel.Zone;
                signalModel.SignalTime = DateTime.Now;

                _mainDbContext.Signals.Add(signalModel);

                return await _mainDbContext.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
