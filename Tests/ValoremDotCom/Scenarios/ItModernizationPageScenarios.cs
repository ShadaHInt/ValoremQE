using CommonTestLibrary.AzureDevOps.Attributes;
using CommonTestLibrary.Planner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using ValoremDotCom.Rules;

namespace ValoremDotCom.Scenarios;

[ExcludeFromCodeCoverage]
[TestClass]
public class ItModernizationPageScenarios : ValoremDotComScenarios
{
    [Description("The 'IT Modernization' page is accessible.")]
    [TestCategory(Accessibility)]
    [TestCategory(Nightly)]
    [Regression(222633)]
    [TestMethod]
    public void TheItModernizationPageIsAccessible()
    {
        Pages.ItModernization page = Given<Pages.ItModernization>(
            ValoremDotComRules.ItModernizationPage,
            ValoremDotComRules.DefaultBrowserType,
            new[] { ValoremDotComRules.CookiesAccepted });
        ThenThePageIsAccessible(page);
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
