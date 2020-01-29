using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BasicConfig.Models
{
    public class Audit
    {
        // Audit Properties
        [Key]
        public int AuditId { get; set; }
        public string UserName { get; set; }
        public string IpAddress { get; set; }
        public string AreaAccessed { get; set; }
        public DateTime Timestamp { get; set; }

        // Default Constructor
        public Audit() { }
    }
}
