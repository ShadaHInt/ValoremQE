using CommonTestLibrary.AzureDevOps.Attributes;
using CommonTestLibrary.Frameworks.WebUi;
using CommonTestLibrary.Logging;
using CommonTestLibrary.Planner;
using CommonTestLibrary.SeleniumUtilities.BrowserExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ValoremDotCom.Rules;

namespace ValoremDotCom.Scenarios;

/// <summary>
/// Demonstrates how to quickly and easily create tests using
/// the WebUi framework. This provides base functionality around
/// logging, cleanup, initialization, and more.
/// </summary>
[ExcludeFromCodeCoverage]
[TestClass]
public class CareersPageTests : ValoremDotComScenarios
{
    [Description("Verifies that job listings are presented to the user.")]
    [TestCategory(MustPass)]
    [TestMethod]
    public void JobListingsArePresent()
    {
        Pages.Careers careers = Given<Pages.Careers>(
            "careers",
            ValoremDotComRules.DefaultBrowserType,
            new[] { ValoremDotComRules.CookiesAccepted });
        When(
            "the job listings are viewed.",
            careers.ViewJobListings);
        Then(
            "at least one listing is available.",
            (state) =>
            {
                List<Pages.Careers.JobListing> listings = GetObject<IEnumerable<Pages.Careers.JobListing>>(Pages.Careers.SaveCareerListingsPredicateName).ToList();

                Log($"Found {listings.Count} listings:");

                foreach (Pages.Careers.JobListing entry in listings)
                {
                    OpenQA.Selenium.IWebElement textElement = WebUiRules.Browser.FindElementsByText(entry.Title).FirstOrDefault();

                    // Try to get the listing into view for the screen shot
                    if (textElement != null) { WebUiRules.Browser.ScrollIntoView(textElement); }

                    using LoggerIndent _ = new();

                    Log(entry.ToString());
                }

                return listings != null && listings.Any();
            });
    }

    [Description("The user is shown a careers page that is accessible.")]
    [TestMethod]
    [TestCategory(Nightly)]
    [TestCategory(Accessibility)]
    [Regression(220508)]
    [Regression(222630)]
    public void TheCareersPageIsAccessible()
    {
        Pages.Careers careers = Given<Pages.Careers>(
            "careers",
            ValoremDotComRules.DefaultBrowserType,
            new[] { ValoremDotComRules.CookiesAccepted });
        ThenThePageIsAccessible(careers);
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