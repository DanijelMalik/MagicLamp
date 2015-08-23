using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using EnvDTE;
using EnvDTE100;
using EnvDTE80;
using MagicLamp.ViewModels;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.Win32;
using NuGet.VisualStudio;
using VSLangProj;

namespace MagicLamp
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidMagicLampPkg)]
    [ProvideAutoLoad("ADFC4E64-0397-11D1-9F4E-00A0C911004F")]
    public sealed class MagicLampPackage : Package
    {
        private DTE _dte;
        private IComponentModel _componentModel;

        private DTE DTE
        {
            get { return _dte ?? (_dte = GetService(typeof(DTE)) as DTE); }
        }

        private IComponentModel ComponentModel
        {
            get { return _componentModel ?? (_componentModel = GetService(typeof(SComponentModel)) as IComponentModel); }
        }

        protected override void Initialize()
        {
            base.Initialize();

            var mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;

            if (mcs != null)
            {
                var advancedModelMakerCommandID = new CommandID(GuidList.guidSolutionToolbarCmdSet, (int)PkgCmdIDList.cmdidTestCmd);
                mcs.AddCommand(new MenuCommand(CreateSolution, advancedModelMakerCommandID));
            }
        }

        private void CreateSolution(object sender, EventArgs e)
        {
            var appDialog = new AppDialog { Title = "Create a new Solution" };
            var result = WindowHelper.ShowModal(appDialog);
           
            if (result == DialogResult.OK)
            {
                CreateSolution(appDialog.ViewModel);
            }
        }

        internal void CreateSolution(MainViewModel mainViewModel)
        {
            var model = mainViewModel.SelectedTemplate;
            var company = mainViewModel.Company;
            var solutionName = mainViewModel.SolutionName;

            if (model == null)
            {
                return;
            }

            var registryPath = String.Format(Environment.Is64BitOperatingSystem ? @"Software\Wow6432Node\Microsoft\VisualStudio\{0}" : @"Software\Microsoft\VisualStudio\{0}", DTE.Version);
            var registryKey = Registry.LocalMachine.OpenSubKey(registryPath);

            if (registryKey == null)
            {
                return;
            }

            var installDir = registryKey.GetValue("InstallDir");
            var solution = DTE.Solution as Solution4;

            if (solution == null)
            {
                return;
            }

            string solutionFileName;

            if (solution.IsOpen)
            {
                solution.Close();
            }

            var properties = DTE.Properties["Environment", "ProjectsAndSolution"];
            var setting = properties.Item("ProjectsLocation");

            if (setting != null && setting.Value != null)
            {
                var targetDir = Path.Combine(setting.Value.ToString(), solutionName);

                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }

                solutionFileName = Path.Combine(targetDir, String.Format("{0}.sln", solutionName));
                solution.Create(targetDir, solutionName);
                solution.SaveAs(solutionName);
            }
            else
            {
                return;
            }

            var projects = new Dictionary<Guid, Tuple<Project, string, ICollection<Guid>, ICollection<string>>>();
            DTE.Events.SolutionEvents.ProjectAdded += project =>
            {
                var item = projects.SingleOrDefault(x => x.Value.Item2 == project.Name);

                if (projects.ContainsKey(item.Key))
                {
                    projects[item.Key] = new Tuple<Project, string, ICollection<Guid>, ICollection<string>>(project, item.Value.Item2, item.Value.Item3, item.Value.Item4);
                }
            };

            // Create Folders & Projects
            foreach (var folder in model.Folders)
            {
                var solutionFolder = (SolutionFolder)solution.AddSolutionFolder(folder.Name).Object;

                foreach (var project in folder.Projects)
                {
                    var fullProjectName = String.Format("{0}.{1}.{2}", company, solutionName, project.Name);
                    var projectFolder = Path.Combine(Path.GetDirectoryName(DTE.Solution.FullName), fullProjectName);

                    if (Directory.Exists(projectFolder))
                    {
                        continue;
                    }

                    projects.Add(project.Id, new Tuple<Project, string, ICollection<Guid>, ICollection<string>>(null, fullProjectName, project.References, project.Packages));

                    var csTemplate = project.IsFindTemplate ? solution.GetProjectTemplate(project.TemplatePath, project.Language) : String.Format(project.TemplatePath, installDir);
                    solutionFolder.AddFromTemplate(csTemplate, projectFolder, fullProjectName);
                }
            }

            // Create Projects - without folders
            foreach (var project in model.Projects)
            {
                var fullProjectName = String.Format("{0}.{1}.{2}", company, solutionName, project.Name);
                var projectFolder = Path.Combine(Path.GetDirectoryName(DTE.Solution.FullName), fullProjectName);

                if (Directory.Exists(projectFolder))
                {
                    continue;
                }

                projects.Add(project.Id, new Tuple<Project, string, ICollection<Guid>, ICollection<string>>(null, fullProjectName, project.References, project.Packages));

                var csTemplate = project.IsFindTemplate ? solution.GetProjectTemplate(project.TemplatePath, project.Language) : String.Format(project.TemplatePath, installDir);
                solution.AddFromTemplate(csTemplate, projectFolder, fullProjectName);
            }

            // Reference the Projects
            foreach (var project in projects)
            {
                var vsProject = project.Value.Item1;
                var solutionProject = vsProject.Object as VSProject;

                InstallNugetPackages(vsProject, project.Value.Item4);

                if (solutionProject != null)
                {
                    var references = projects.Where(x => project.Value.Item3.Contains(x.Key)).Select(x => x.Value.Item1);

                    foreach (var reference in references)
                    {
                        solutionProject.References.AddProject(reference);
                    }
                }

                vsProject.Save();
            }

            solution.SaveAs(solutionFileName);
        }

        private void InstallNugetPackages(Project project, ICollection<string> packages)
        {
            if (packages == null || !packages.Any())
            {
                return;
            }

            var nuget = ComponentModel.GetService<IVsPackageInstaller>();
            DTE.StatusBar.Progress(true, "Magic Lamp - Installing pacakages", 0, 100);
            DTE.StatusBar.Animate(true, vsStatusAnimation.vsStatusAnimationGeneral);
            var count = packages.Count;
            var installed = 0;

            System.Threading.Tasks.Task.Run(() =>
            {
                foreach (var package in packages)
                {
                    DTE.StatusBar.Progress(true, string.Format("Magic Lamp - Installing NuGet package: {0}", package), installed, count);
                    nuget.InstallPackage(string.Empty, project, package, (Version)null, false);
                    installed++;
                }
            });

            DTE.StatusBar.Animate(false, vsStatusAnimation.vsStatusAnimationGeneral);
        }
    }
}