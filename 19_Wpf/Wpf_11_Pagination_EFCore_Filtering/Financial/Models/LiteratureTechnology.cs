namespace Financial.Models
{
    public class LiteratureTechnology
    {
        public int LiteratureID { get; set; }
        public int TechnologyID { get; set; }
        public Literature Literature { get; set; }
        public Technology Technology { get; set; }
    }
}
