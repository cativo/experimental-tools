using ExperimentalTools.Environment;
using ExperimentalTools.Options;
using ExperimentalTools.Roslyn.Features.AccessModifier;
using ExperimentalTools.Roslyn.Features.AccessModifier.Recipes;
using ExperimentalTools.Tests.Infrastructure.Refactoring;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Xunit.Abstractions;

namespace ExperimentalTools.Tests.Features.AccessModifier
{
    public class AccessModifierTest : RefactoringTest
    {
        public AccessModifierTest(ITestOutputHelper output)
            : base(output)
        {
        }

        protected override CodeRefactoringProvider Provider =>
            new ChangeAccessModifierRefactoring(new ITypeRecipe[]
            {
                                            new TopLevelTypeRecipe(),
                                            new NestedInClassRecipe(),
                                            new NestedInStructRecipe()
            }, new OptionsService(new EnvironmentService()));
    }
}
