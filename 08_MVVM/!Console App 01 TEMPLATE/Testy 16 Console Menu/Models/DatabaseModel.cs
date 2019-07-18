using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Testy_16_Console_Menu.Models
{
    public class DatabaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //to give automaticly new ID
        public int ID { get; set; }
        public string Name { get; set; }
        public double Salary { get; set; }
        public DateTime StartWork { get; set; }
    }
}
