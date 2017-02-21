using System;

namespace ExperimentalTools.Environment
{
    internal class EnvironmentData
    {
        private static Lazy<EnvironmentData> instance = new Lazy<EnvironmentData>(true);
        public static EnvironmentData Instance => instance.Value;

        public string IDEVersion { get; set; }
    }
}
