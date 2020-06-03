using System;
using System.Collections.Generic;
using System.Text;

namespace Commerce.Domain
{
    public class DiscountedProduct
    {
        public DiscountedProduct(string name, decimal unitPrice)
        {
            if (name == null) throw new ArgumentNullException("name");

            this.Name = name;
            this.UnitPrice = unitPrice;
        }

        public string Name { get; }
        public decimal UnitPrice { get; }
    }
}
