using System.Collections.Generic;

namespace HtmlToPdf.Models
{
    public class Literature
    {
        public int LiteratureID { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Url { get; set; }
        public ICollection<LiteratureTechnology> LiteraturesTechnologies { get; set; }
    }
}
