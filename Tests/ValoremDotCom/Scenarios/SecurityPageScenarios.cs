using CommonTestLibrary.Planner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using ValoremDotCom.Rules;

namespace ValoremDotCom.Scenarios;

[ExcludeFromCodeCoverage]
[TestClass]
public class SecurityPageScenarios : ValoremDotComScenarios
{
    [Description("The 'Security' page is accessible.")]
    [TestCategory(Accessibility)]
    [TestCategory(Nightly)]
    [TestMethod]
    public void TheSecurityPageIsAccessible()
    {
        Pages.Security page = Given<Pages.Security>(
            ValoremDotComRules.SecurityPage,
            ValoremDotComRules.DefaultBrowserType,
            new[] { ValoremDotComRules.CookiesAccepted });
        Then(
            $"the {page.PageName} page passes an accessibility scan.",
            () => page.IsAccessible(TestContext));
    }

    [ClassInitialize]
    public static void ClassInitialize(
        TestContext context
    )
    {
        Initialize(context);

        TaskEstimation.Load("Scenarios\\execution_times.csv");
    }

    [ClassCleanup]
    public static void ClassCleanup() => Cleanup();
}
