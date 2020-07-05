using System.Collections.Generic;

namespace Financial.Models
{
    public class PickedUp
    {
        public int ProjectID { get; set; }
        public bool Checked { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public string Techs { get; set; }
    }
}
