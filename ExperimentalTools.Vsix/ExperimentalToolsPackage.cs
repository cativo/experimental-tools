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

namespace ExperimentalTools.Vsix
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", Vsix.Version, IconResourceID = 400)] // Info on this package for Help/About
    [Guid(Vsix.Id)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideOptionPage(typeof(GeneralOptions), "Experimental Tools", "General", 0, 0, true)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionOpening_string)]
    public sealed class ExperimentalToolsPackage : Package
    {
        protected override void Initialize()
        {
            var componentModel = (IComponentModel)GetService(typeof(SComponentModel));
            var workspace = componentModel.GetService<VisualStudioWorkspace>();
            workspace.WorkspaceChanged += WorkspaceChanged;

            var generalOptions = (GeneralOptions)GetDialogPage(typeof(GeneralOptions));
            generalOptions.UpdateFeatureStates();

            base.Initialize();
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
                        WorkspaceCache.Instance.AddOrUpdate(description);
                    }
                    break;
                case Microsoft.CodeAnalysis.WorkspaceChangeKind.ProjectRemoved:
                    WorkspaceCache.Instance.Remove(e.ProjectId);
                    break;
                default:
                    break;
            }
        }
    }
}