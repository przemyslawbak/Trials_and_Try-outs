using System;
using System.Collections.Generic;
using System.Text;

namespace Commerce.Domain
{
    public class ExchangeRate
    {
        public Guid Id { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
    }
}
