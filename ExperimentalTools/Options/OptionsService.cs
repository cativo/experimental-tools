using System.Composition;

namespace ExperimentalTools.Options
{
    [Export(typeof(IOptions))]
    internal class OptionsService : IOptions
    {
        private readonly IEnvironment environment;

        [ImportingConstructor]
        public OptionsService(IEnvironment environment)
        {
            this.environment = environment;
        }

        public bool IsFeatureEnabled(string identifier)
        {
            if ((identifier == FeatureIdentifiers.RenameFileToMatchTypeNameCodeFix ||
                identifier == FeatureIdentifiers.RenameTypeToMatchFileNameCodeFix) &&
                !environment.IsVS2015)
            {
                return false;
            }

            var features = OptionsBucket.Instance.Features;
            return features.ContainsKey(identifier) ? features[identifier] : false;
        }
    }
}
