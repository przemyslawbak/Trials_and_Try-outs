using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace T_SQL_03_QueryToObject.Models
{
    public class Account
    {
        public int ACCOUNT_ID { get; set; }
        public float? AVAIL_BALANCE { get; set; }
        public DateTime? CLOSE_DATE { get; set; }
        public DateTime? LAST_ACTIVITY_DATE { get; set; }
        public DateTime OPEN_DATE { get; set; }
        public float? PENDING_BALANCE { get; set; }
        public string STATUS { get; set; }
        [ForeignKey("Customer")]
        public int? CUST_ID { get; set; }
        public Customer Customer { get; set; }
        [ForeignKey("Branch")]
        public int OPEN_BRANCH_ID { get; set; }
        public Branch Branch { get; set; }
        [ForeignKey("Book")]
        public int OPEN_EMP_ID { get; set; }
        [ForeignKey("Employee")]
        public Employee Employee { get; set; }
        public string PRODUCT_CD { get; set; }
        public Product Product { get; set; }
    }
}
