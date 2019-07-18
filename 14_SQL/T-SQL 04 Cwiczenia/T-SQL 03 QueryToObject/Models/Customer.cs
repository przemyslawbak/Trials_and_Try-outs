using System;
using System.Collections.Generic;
using System.Text;

namespace T_SQL_03_QueryToObject.Models
{
    public class Customer
    {
        public int CUST_ID { get; set; }
        public string ADDRESS { get; set; }
        public string CITY { get; set; }
        public string CUST_TYPE_CD { get; set; }
        public string FED_ID { get; set; }
        public string POSTAL_CODE { get; set; }
        public string STATE { get; set; }
    }
}
