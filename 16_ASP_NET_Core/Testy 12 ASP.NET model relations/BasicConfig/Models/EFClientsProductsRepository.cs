using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicConfig.Models
{
    public class EFClientsProductsRepository : IClientsProductsRepository
    {
        private ApplicationDbContext context;
        public EFClientsProductsRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<ClientsProducts> Cepes => context.CliPro;
    }
}
