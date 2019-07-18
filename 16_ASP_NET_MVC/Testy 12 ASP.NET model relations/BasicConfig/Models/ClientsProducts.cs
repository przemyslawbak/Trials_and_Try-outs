using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicConfig.Models
{
    public class ClientsProducts
    {
        public int ClientID { get; set; }
        public int ProductID { get; set; }
        public Client Client { get; set; }
        public Product Product { get; set; }
    }
}
