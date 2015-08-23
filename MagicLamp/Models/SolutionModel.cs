using System;
using System.Collections.Generic;

namespace MagicLamp.Models
{
    public class SolutionModel
    {
        public SolutionModel()
        {
            Folders = new List<FolderModel>();
            Projects = new List<ProjectModel>();
        }

        public string Name { get; set; }
        public ICollection<FolderModel> Folders { get; set; }
        public ICollection<ProjectModel> Projects { get; set; }

        public string Description { get; set; }

        public ICollection<string> Tags { get; set; }

        

        public static SolutionModel FromFile(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}