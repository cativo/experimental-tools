using System.ComponentModel;

namespace ExperimentalTools.Vsix.Features.Options
{
    internal class PostVS2015Options : VS2015Options
    {
        [Browsable(false)]
        public new bool RenameTypeToMatchFileNameCodeFix { get; set; }

        [Browsable(false)]
        public new bool RenameFileToMatchTypeNameCodeFix { get; set; }
    }
}
