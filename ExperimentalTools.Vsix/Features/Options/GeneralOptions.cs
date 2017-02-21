using Microsoft.VisualStudio.Shell;

namespace ExperimentalTools.Vsix.Features.Options
{
    internal class GeneralOptions : DialogPage
    {
        private readonly IOptionsSet options;

        public GeneralOptions()
        {
            var environment = ServiceLocator.GetService<IEnvironment>();
            options = environment.IsVS2015 ? new VS2015Options() : new PostVS2015Options();
        }

        public override object AutomationObject => options;

        protected override void OnApply(PageApplyEventArgs e)
        {
            if (e.ApplyBehavior == ApplyKind.Apply)
            {
                UpdateFeatureStates();
            }

            base.OnApply(e);
        }

        public void UpdateFeatureStates()
        {
            options.UpdateFeatureStates();
        }
    }
}
