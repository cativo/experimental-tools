using System.Composition;

namespace ExperimentalTools.Environment
{
    [Export(typeof(IEnvironment))]
    internal class EnvironmentService : IEnvironment
    {
        public bool IsVS2015
        {
            get
            {
                var version = EnvironmentData.Instance.IDEVersion;
                return version != null && version.StartsWith("14.");
            }
        }
    }
}
