using System.Collections.Generic;

namespace HtmlToPdf.Models
{
    public class Technology
    {
        public int TechnologyID { get; set; }
        public string Name { get; set; }
        public string PictureLink { get; set; }
        public ICollection<TechnologyProject> TechnologiesProjects { get; set; }
    }
}
