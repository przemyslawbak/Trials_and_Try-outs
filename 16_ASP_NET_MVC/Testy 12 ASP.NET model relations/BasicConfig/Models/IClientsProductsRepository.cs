using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicConfig.Models
{
    public interface IClientsProductsRepository
    {
        IQueryable<ClientsProducts> Cepes { get; }
    }
}
