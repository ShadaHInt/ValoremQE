using CommonTestLibrary.AzureDevOps.Attributes;
using CommonTestLibrary.Planner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using ValoremDotCom.Rules;

namespace ValoremDotCom.Scenarios;

[ExcludeFromCodeCoverage]
[TestClass]
public class DataAndAiPageScenarios : ValoremDotComScenarios
{
    [Description("The 'Data & AI' page is accessible.")]
    [TestCategory(Accessibility)]
    [TestCategory(Nightly)]
    [Regression(222631)]
    [TestMethod]
    public void TheDataAndAiPageIsAccessible()
    {
        Pages.DataAndAi page = Given<Pages.DataAndAi>(
            ValoremDotComRules.DataAndAiPage,
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
