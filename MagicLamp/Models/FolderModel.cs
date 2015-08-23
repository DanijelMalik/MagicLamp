using System.Collections.Generic;

namespace MagicLamp.Models
{
    public class FolderModel
    {
        public FolderModel()
        {
            Projects = new List<ProjectModel>();
        }

        public ICollection<ProjectModel> Projects { get; set; }
        public string Name { get; set; }
    }
}