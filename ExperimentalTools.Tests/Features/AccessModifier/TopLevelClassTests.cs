﻿using ExperimentalTools.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ExperimentalTools.Tests.Features.AccessModifier
{
    public class TopLevelClassTests
    {
        [Theory, MemberData("TestData")]
        public async Task Test(string test, string actionTitle, string input, string expectedResult)
        {
            await TestRunner.RunAsync(input, async (acceptor, context) =>
            {
                Assert.True(acceptor.HasAction(actionTitle));

                var result = await acceptor.GetResultAsync(actionTitle, context);
                Assert.Equal(expectedResult.HomogenizeLineEndings(), result.HomogenizeLineEndings());
            });
        }
                
        public static IEnumerable<object[]> TestData =>
            new[]
            {
                new object[]
                {
                    "Implicit to InternalExplicit",
                    Resources.ToInternalExplicit,
                    @"
namespace HelloWorld
{
    @::@class Test
    {
    }
}",
                    @"
namespace HelloWorld
{
    internal class Test
    {
    }
}"
                },
                new object[]
                {
                    "Implicit to Public",
                    Resources.ToPublic,
                    @"
namespace HelloWorld
{
    @::@class Test
    {
    }
}",
                    @"
namespace HelloWorld
{
    public class Test
    {
    }
}"
                },
                new object[]
                {
                    "Public to InternalExplicit",
                    Resources.ToInternalExplicit,
                    @"
namespace HelloWorld
{
    @::@public class Test
    {
    }
}",
                    @"
namespace HelloWorld
{
    internal class Test
    {
    }
}"
                },
                new object[]
                {
                    "Internal to Public",
                    Resources.ToPublic,
                    @"
namespace HelloWorld
{
    internal @::@class Test
    {
    }
}",
                    @"
namespace HelloWorld
{
    public class Test
    {
    }
}"
                },
                new object[]
                {
                    "Multiple modifiers to InternalExplicit",
                    Resources.ToInternalExplicit,
                    @"
namespace HelloWorld
{
    private static @::@public class Test
    {
    }
}",
                    @"
namespace HelloWorld
{
    internal static class Test
    {
    }
}"
                },
                new object[]
                {
                    "Multiple modifiers to Public",
                    Resources.ToPublic,
                    @"
namespace HelloWorld
{
    private static @::@public class Test
    {
    }
}",
                    @"
namespace HelloWorld
{
    public static class Test
    {
    }
}"
                },
                new object[]
                {
                    "Public to InternalImplicit",
                    Resources.ToInternalImplicit,
                    @"
namespace HelloWorld
{
    @::@public class Test
    {
    }
}",
                    @"
namespace HelloWorld
{
    class Test
    {
    }
}"
                },
                new object[]
                {
                    "Internal to InternalImplicit",
                    Resources.ToInternalImplicit,
                    @"
namespace HelloWorld
{
    @::@internal class Test
    {
    }
}",
                    @"
namespace HelloWorld
{
    class Test
    {
    }
}"
                },
                new object[]
                {
                    "Multiple modifiers to InternalImplicit",
                    Resources.ToInternalImplicit,
                    @"
namespace HelloWorld
{
    internal protected static class Test@::@
    {
    }
}",
                    @"
namespace HelloWorld
{
    static class Test
    {
    }
}"
                }
            };
    }
}
