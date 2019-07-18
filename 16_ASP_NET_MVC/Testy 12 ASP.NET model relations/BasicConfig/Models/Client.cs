using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicConfig.Models
{
    public class Client
    {
        public int ClientID { get; set; }
        public string Name { get; set; }
        public int ProductID { get; set; }
        public ICollection<ClientsProducts> CPs { get; set; }
    }
}
