using System;
using System.Collections.Generic;

namespace MagicLamp.Models
{
    public class ProjectModel
    {
        public ProjectModel()
        {
            References = new List<Guid>();
            IsFindTemplate = true;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string TemplatePath { get; set; }
        public string Language { get; set; }
        public ICollection<Guid> References { get; set; }
        public bool IsFindTemplate { get; set; }
        public ICollection<string> Packages { get; set; }
    }
}