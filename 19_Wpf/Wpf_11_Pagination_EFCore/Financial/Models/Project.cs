using System;
using System.Collections.Generic;

namespace Financial.Models
{
    public class Project
    {
        public int ProjectID { get; set; }
        public bool Checked { get; set; }
        public string Name { get; set; }
        public string Comments { get; set; }
        public string GitHubUrl { get; set; }
        public string WebUrl { get; set; }
        public string WorkLogUrl { get; set; }
        public string YouTubeUrl { get; set; }
        public string BackColor { get; set; }
        public string PictureUrl { get; set; }
        public DateTime CompletionDate { get; set; }
        public ICollection<TechnologyProject> TechnologiesProjects { get; set; }
    }
}
