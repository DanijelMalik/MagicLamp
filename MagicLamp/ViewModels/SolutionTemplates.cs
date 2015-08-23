using System;
using System.Collections.Generic;
using MagicLamp.Models;

namespace MagicLamp.ViewModels
{
    internal class SolutionTemplates
    {
        private static SolutionModel _sswDataOnion;
        private static SolutionModel _mvcAngular;
        private static SolutionModel _hotTowel;

        public static SolutionModel MvcAngular
        {
            get
            {
                if (_mvcAngular != null)
                {
                    return _mvcAngular;
                }

                const string language = "CSharp";

                _mvcAngular = new SolutionModel
                {
                    Name = "MVC - AngularJS",
                    Description = "MVC 5 application with AngularJS",
                    Tags = new List<string> { "ASP.NET MVC", "AngularJS", "JavaScript" },
                    Projects = new List<ProjectModel>
                    {
                        new ProjectModel
                        {
                            Id = new Guid("{653AF2EC-67D3-48E5-9246-4DEBF5015AD8}"),
                            Language = language,
                            TemplatePath = @"{0}\ProjectTemplatesCache\CSharp\Web\Version2012\1033\MvcWebApplicationProjectTemplatev4.1.cshtml\MvcWebApplicationProjectTemplate.12.cshtml.vstemplate",
                            Name = "Web",
                            IsFindTemplate = false,
                            Packages = new List<string> { "angularjs" }
                        },
                    }
                };

                return _mvcAngular;
            }
        }

        public static SolutionModel SswDataOnion
        {
            get
            {
                if (_sswDataOnion != null)
                {
                    return _sswDataOnion;
                }

                const string language = "CSharp";
                const string template = "Class Library";

                _sswDataOnion = new SolutionModel
                {
                    Name = "MVC - SSW Data Onion",
                    Description = "MVC 5 application using SSW Data Onion",
                    Tags = new List<string> { "ASP.NET MVC", "Onion Architecture", "Dependency Injection" },
                    Folders = new List<FolderModel>
                    {
                        new FolderModel
                        {
                            Name = "1. Presentation",
                            Projects = new List<ProjectModel>
                            {
                                new ProjectModel
                                {
                                    Id = new Guid("{653AF2EC-67D3-48E5-9246-4DEBF5015AD8}"),
                                    Language = language,
                                    TemplatePath = @"{0}\ProjectTemplatesCache\CSharp\Web\Version2012\1033\MvcWebApplicationProjectTemplatev4.1.cshtml\MvcWebApplicationProjectTemplate.12.cshtml.vstemplate",
                                    Name = "Presentation.Web",
                                    References = new List<Guid>
                                    {
                                        new Guid("{0BAE7E69-E4AA-4018-889A-BD19D466FD51}"),
                                        new Guid("{A4EA805C-D92C-4DEA-8F18-38415C1290C0}"),
                                    },
                                    IsFindTemplate = false,
                                },
                            }
                        },
                        new FolderModel
                        {
                            Name = "2. Business",
                            Projects = new List<ProjectModel>
                            {
                                new ProjectModel
                                {
                                    Id = new Guid("{457361D2-0861-49A5-ABE8-0D6278DAE4EF}"),
                                    Language = language,
                                    TemplatePath = template,
                                    Name = "Business",
                                    References = new List<Guid>
                                    {
                                        new Guid("{25CE2990-94EB-46C4-9F5D-C11344D446E4}"),
                                        new Guid("{A4EA805C-D92C-4DEA-8F18-38415C1290C0}"),
                                        new Guid("{0BAE7E69-E4AA-4018-889A-BD19D466FD51}"),
                                    }
                                },
                                new ProjectModel
                                {
                                    Id = new Guid("{A4EA805C-D92C-4DEA-8F18-38415C1290C0}"),
                                    Language = language,
                                    TemplatePath = template,
                                    Name = "Business.Contracts",
                                    References = new List<Guid>
                                    {
                                        new Guid("{0BAE7E69-E4AA-4018-889A-BD19D466FD51}"),
                                    }
                                },
                            }
                        },
                        new FolderModel
                        {
                            Name = "3. Data",
                            Projects = new List<ProjectModel>
                            {
                                new ProjectModel
                                {
                                    Id = new Guid("{8B569E61-72AF-44DC-87C4-63B1A99B7629}"),
                                    Language = language,
                                    TemplatePath = template,
                                    Name = "Data",
                                    References = new List<Guid>
                                    {
                                        new Guid("{25CE2990-94EB-46C4-9F5D-C11344D446E4}"),
                                        new Guid("{0BAE7E69-E4AA-4018-889A-BD19D466FD51}"),
                                    }
                                },
                                new ProjectModel
                                {
                                    Id = new Guid("{25CE2990-94EB-46C4-9F5D-C11344D446E4}"),
                                    Language = language,
                                    TemplatePath = template,
                                    Name = "Data.Contracts",
                                    References = new List<Guid>
                                    {
                                        new Guid("{0BAE7E69-E4AA-4018-889A-BD19D466FD51}"),
                                    }
                                },
                            }
                        },
                        new FolderModel
                        {
                            Name = "4. Common",
                            Projects = new List<ProjectModel>
                            {
                                new ProjectModel
                                {
                                    Id = new Guid("{0BAE7E69-E4AA-4018-889A-BD19D466FD51}"),
                                    Language = language,
                                    TemplatePath = template,
                                    Name = "Common"
                                },
                            }
                        },
                        new FolderModel
                        {
                            Name = "5. Tests",
                            Projects = new List<ProjectModel>
                            {
                                new ProjectModel
                                {
                                    Id = new Guid("{EF1D6ABB-D8EB-40AE-81A5-1929BF6B53CF}"),
                                    Language = language,
                                    TemplatePath = template,
                                    Name = "Business.Tests",
                                    References = new List<Guid>
                                    {
                                        new Guid("{25CE2990-94EB-46C4-9F5D-C11344D446E4}"),
                                        new Guid("{A4EA805C-D92C-4DEA-8F18-38415C1290C0}"),
                                        new Guid("{0BAE7E69-E4AA-4018-889A-BD19D466FD51}"),
                                        new Guid("{457361D2-0861-49A5-ABE8-0D6278DAE4EF}")
                                    }
                                },
                            }
                        },
                    },
                };

                return _sswDataOnion;
            }
        }

        public static SolutionModel HotTowel
        {
            get
            {
                if (_hotTowel != null)
                {
                    return _hotTowel;
                }

                const string language = "CSharp";

                _hotTowel = new SolutionModel
                {
                    Name = "HotTowel SPA",
                    Description = "Single Page Application using Hot Towel",
                    Tags = new List<string> { "ASP.NET", "AngularJS", "BreezeJS", "HotTowel" },
                    Projects = new List<ProjectModel>
                    {
                        new ProjectModel
                        {
                            Id = new Guid("{653AF2EC-67D3-48E5-9246-4DEBF5015AD8}"),
                            Language = language,
                            TemplatePath = @"{0}\ProjectTemplatesCache\CSharp\Web\Version2012\1033\EmptyWebApplicationProject40\EmptyWebApplicationProject40.vstemplate",
                            Name = "Web",
                            IsFindTemplate = false,
                            Packages = new List<string> { "HotTowel" }
                        },
                    }
                };

                return _hotTowel;
            }
        }
    }
}