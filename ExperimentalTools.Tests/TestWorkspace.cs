﻿using Microsoft.CodeAnalysis;

namespace ExperimentalTools.Tests
{
    internal class TestWorkspace : Workspace
    {
        public TestWorkspace() 
            : base(Microsoft.CodeAnalysis.Host.Mef.MefHostServices.DefaultHost, "Test")
        {
        }

        public void Open(SolutionInfo solutionInfo)
        {
            OnSolutionAdded(solutionInfo);
        }
    }
}
