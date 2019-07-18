using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace T_SQL_03_QueryToObject.Models
{
    public class Individual
    {
        public DateTime? BIRTH_DATE { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        [ForeignKey("Customer")]
        public int CUST_ID { get; set; }
        public Customer Customer { get; set; }
    }
}
