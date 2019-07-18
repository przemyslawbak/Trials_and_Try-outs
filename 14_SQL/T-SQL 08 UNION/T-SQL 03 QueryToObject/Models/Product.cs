using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace T_SQL_03_QueryToObject.Models
{
    public class Product
    {
        public string PRODUCT_CD { get; set; }
        public DateTime? DATE_OFFERED { get; set; }
        public DateTime? DATE_RETIRED { get; set; }
        public string NAME { get; set; }
        [ForeignKey("Product")]
        public string PRODUCT_TYPE_CD { get; set; }
        public Product Products { get; set; }
    }
}
