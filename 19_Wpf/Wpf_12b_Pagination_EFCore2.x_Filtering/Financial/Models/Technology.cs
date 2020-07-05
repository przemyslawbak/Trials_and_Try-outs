using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Financial.Models
{
    public class Technology
    {
        public int TechnologyID { get; set; }
        [Required(ErrorMessage = "Please fill it up.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please fill it up.")]
        public string PictureLink { get; set; }
        public ICollection<TechnologyProject> TechnologiesProjects { get; set; }
        public ICollection<LiteratureTechnology> LiteraturesTechnologies { get; set; }
    }
}
