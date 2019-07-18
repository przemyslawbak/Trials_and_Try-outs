using System;
using System.Collections.Generic;
using System.Text;

namespace T_SQL_03_QueryToObject.Models
{
    public class Branch
    {
        public int BRANCH_ID { get; set; }
        public string ADDRESS { get; set; }
        public string CITY { get; set; }
        public string NAME { get; set; }
        public string STATE { get; set; }
        public string ZIP_CODE { get; set; }

    }
}
