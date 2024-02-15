using CommonTestLibrary.AzureDevOps.Attributes;
using CommonTestLibrary.Planner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using ValoremDotCom.Rules;

namespace ValoremDotCom.Scenarios;

[ExcludeFromCodeCoverage]
[TestClass]
public class DigitalWorkplacePageScenarios : ValoremDotComScenarios
{
    [Description("The 'Digital Workplace' page is accessible.")]
    [TestCategory(Accessibility)]
    [TestCategory(Nightly)]
    [Regression(222632)]
    [TestMethod]
    public void TheDigitalWorkplacePageIsAccessible()
    {
        Pages.DigitalWorkplace page = Given<Pages.DigitalWorkplace>(
            ValoremDotComRules.DigitalWorkplacePage,
            ValoremDotComRules.DefaultBrowserType,
            new[] { ValoremDotComRules.CookiesAccepted });
        ThenThePageIsAccessible(page);
    }

    [Description("The user may watch the embedded video.")]
    [TestCategory(Nightly)]
    [TestMethod]
    public void UserMayOpenAndCloseTheDigitalWorkplaceVideo()
    {
        Pages.DigitalWorkplace page = Given<Pages.DigitalWorkplace>(
            ValoremDotComRules.DigitalWorkplacePage,
            ValoremDotComRules.DefaultBrowserType,
            new[] { ValoremDotComRules.CookiesAccepted });
        When(
            "the user clicks the video button.",
            page.OpenVideo,
            adds: new CommonTestLibrary.Planner.State.ActionState(ValoremDotComRules.VideoPlayerOpened));
        When(
            "the user closes the video.",
            () => page.CloseVideoWindow(),
            removes: new CommonTestLibrary.Planner.State.ActionState(ValoremDotComRules.VideoPlayerOpened));
        Then(
            "the `Watch` button is enabled again.",
            () => page.IsWatchVideoButtonEnabled());
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
