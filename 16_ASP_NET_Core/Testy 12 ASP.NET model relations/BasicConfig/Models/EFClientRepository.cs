using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicConfig.Models
{
    public class EFClientRepository : IClientRepository
    {
        private ApplicationDbContext context;
        public EFClientRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<Client> Clients => context.Clients;
    }
}
