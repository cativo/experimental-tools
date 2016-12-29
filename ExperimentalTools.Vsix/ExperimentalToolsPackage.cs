﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using ExperimentalTools.Vsix.Options;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.LanguageServices;
using ExperimentalTools.Workspace;
using System.Linq;
using ExperimentalTools.Models;
using System.IO;
using ExperimentalTools.Vsix.Commands;
using EnvDTE;

namespace ExperimentalTools.Vsix
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", Vsix.Version, IconResourceID = 400)] // Info on this package for Help/About
    [Guid(Vsix.Id)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideOptionPage(typeof(GeneralOptions), "Experimental Tools", "General", 0, 0, true)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionOpening_string)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class ExperimentalToolsPackage : Package
    {
        internal static string IDEVersion { get; set; }

        protected override void Initialize()
        {
            base.Initialize();

            var componentModel = (IComponentModel)GetService(typeof(SComponentModel));
            var workspace = componentModel.GetService<VisualStudioWorkspace>();
            workspace.WorkspaceChanged += WorkspaceChanged;

            var dte = (DTE)GetService(typeof(DTE));
            IDEVersion = dte.Version;

            var generalOptions = (GeneralOptions)GetDialogPage(typeof(GeneralOptions));
            generalOptions.UpdateFeatureStates();

            LocateInSolutionExplorerCommand.Initialize(this);
        }        

        private void WorkspaceChanged(object sender, Microsoft.CodeAnalysis.WorkspaceChangeEventArgs e)
        {
            switch (e.Kind)
            {
                case Microsoft.CodeAnalysis.WorkspaceChangeKind.SolutionRemoved:
                    WorkspaceCache.Instance.Clear();
                    break;
                case Microsoft.CodeAnalysis.WorkspaceChangeKind.ProjectAdded:
                case Microsoft.CodeAnalysis.WorkspaceChangeKind.ProjectChanged:
                    var project = e.NewSolution.Projects.FirstOrDefault(p => p.Id == e.ProjectId);
                    if (project != null)
                    {
                        var description = new ProjectDescription
                        {
                            Id = project.Id,
                            Path = !string.IsNullOrWhiteSpace(project.FilePath) ? Path.GetDirectoryName(project.FilePath) : null,
                            AssemblyName = project.AssemblyName
                        };
                        WorkspaceCache.Instance.AddOrUpdateProject(description);
                    }
                    break;
                case Microsoft.CodeAnalysis.WorkspaceChangeKind.ProjectRemoved:
                    WorkspaceCache.Instance.RemoveProject(e.ProjectId);
                    break;
                default:
                    break;
            }
        }
    }
}
