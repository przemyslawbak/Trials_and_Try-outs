using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace T_SQL_03_QueryToObject.Models
{
    public class Employee
    {
        public int EMP_ID { get; set; }
        public DateTime? END_DATE { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public DateTime START_DATE { get; set; }
        public string TITLE { get; set; }
        [ForeignKey("Branch")]
        public int? ASSIGNED_BRANCH_ID { get; set; }
        public Branch Branch { get; set; }
        [ForeignKey("Department")]
        public int? DEPT_ID { get; set; }
        public Department Department { get; set; }
        [ForeignKey("Employee")]
        public int? SUPERIOR_EMP_ID { get; set; }
        public Employee Employees { get; set; }
    }
}
