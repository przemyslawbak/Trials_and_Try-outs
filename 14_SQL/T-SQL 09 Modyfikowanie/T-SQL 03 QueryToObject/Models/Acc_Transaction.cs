using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace T_SQL_03_QueryToObject.Models
{
    public class Acc_Transaction
    {
        public int TXN_ID { get; set; }
        public float AMOUNT { get; set; }
        public DateTime FUNDS_AVAIL_DATE { get; set; }
        public DateTime TXN_DATE { get; set; }
        public string TXN_TYPE_CD { get; set; }
        [ForeignKey("Account")]
        public int? ACCOUNT_ID { get; set; }
        public Account Account { get; set; }
        [ForeignKey("Branch")]
        public int? EXECUTION_BRANCH_ID { get; set; }
        public Branch Branch { get; set; }
        [ForeignKey("Employee")]
        public int? TELLER_EMP_ID { get; set; }
        public Employee Employee { get; set; }
    }
}
