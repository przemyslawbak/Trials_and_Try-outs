using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace T_SQL_03_QueryToObject.Models
{
    public class Business
    {
        public DateTime? INCORP_DATE { get; set; }
        public string NAME { get; set; }
        public string STATE_ID { get; set; }
        [ForeignKey("Customer")]
        public int CUST_ID { get; set; }
        public Customer Customer { get; set; }
    }
}
