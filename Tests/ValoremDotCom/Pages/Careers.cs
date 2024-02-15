using CommonTestLibrary.Planner.Predicates;
using CommonTestLibrary.Planner.Rules;
using CommonTestLibrary.Planner.State;
using CommonTestLibrary.SeleniumUtilities.BrowserExtensions;
using CommonTestLibrary.SeleniumUtilities.WebElementExtensions;
using CommonTestLibrary.Utilities;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ValoremDotCom.Pages;

[ExcludeFromCodeCoverage]
public class Careers : ValoremPage
{
    /// <summary>
    /// Name of the predicate that career listings are stored in.
    /// </summary>
    public const string SaveCareerListingsPredicateName = "career_listings";

    /// <summary>
    /// Stores the extracted data from a job listing on the careers page.
    /// </summary>
    /// <param name="Title">The title of the job.</param>
    /// <param name="Office">The office where the work will be done.</param>
    /// <param name="Link">A link to the job description.</param>
    public record JobListing(string Title, string Office, string Link);

    /// <summary>
    /// Create a new model of the careers page.
    /// </summary>
    /// <param name="browser">The browser controlling the page.</param>
    public Careers(
        IWebDriver browser
    ) : base(browser)
    {
        ValidatePage();
    }

    /// <summary>
    /// Create a new model of the careers page.
    /// </summary>
    public Careers() : base()
    {
        ValidatePage();
    }

    /// <summary>
    /// Get the active listings on the career page.
    /// </summary>
    /// <returns>A set of job listings.</returns>
    public IEnumerable<JobListing> GetOpenPositions()
    {
        // We don't really care if this has already been clicked
        OpenPositionsButton?.SafeClick();

        IEnumerable<IWebElement> rawRows = WebDriver.FindElements(By.XPath(JobListingSelector));

        IEnumerable<JobListing> listings = rawRows.Select
        (
            element =>
            {
                string rawElement = element.GetInnerHtml();
                string link = rawElement.Split(new string[] { "<a href=", "</a>" }, System.StringSplitOptions.RemoveEmptyEntries | System.StringSplitOptions.TrimEntries)[1].Split('"', System.StringSplitOptions.RemoveEmptyEntries | System.StringSplitOptions.TrimEntries)[0];
                string title = rawElement.Split("</a>", System.StringSplitOptions.RemoveEmptyEntries | System.StringSplitOptions.TrimEntries)[0].Split(">", System.StringSplitOptions.RemoveEmptyEntries | System.StringSplitOptions.TrimEntries).Last();
                string office = rawElement.Split(new string[] { "<h6>", "</h6>" }, System.StringSplitOptions.RemoveEmptyEntries | System.StringSplitOptions.TrimEntries)[1];

                return new JobListing(title, office, link);
            }
        );

        return listings;
    }

    /// <summary>
    /// Rule that shows how to use temporary predicates
    /// to store data collected by a rule.
    /// 
    /// This data is ALWAYS cleaned up and removed
    /// from the storage once a scenario is finished.
    /// </summary>
    /// <returns>State with a temporary predicate that has the job listings.</returns>
    [PlannerRule(
        name: "view(job_listings)",
        requires: new[] { "url(careers)", "^listings(collected)" },
        adds: new string[] { "listings(collected)" })]
    public ActionState ViewJobListings(
        ActionState _,
        PlannerAction action
    )
    {
        Predicate listings = new(SaveCareerListingsPredicateName, GetOpenPositions());

        return new ActionState(new[] { listings });
    }

    private void ValidatePage() =>
        MustBe.True(WebDriver.UrlContains("/careers"), $"Not on the correct page. Found to be on `{WebDriver.Url}`");

    private IWebElement OpenPositionsButton => WebDriver.FindElementByIdCssOrXpath(OpenPositionsSelector);

    private const string OpenPositionsSelector = "btnSeeOpenPostionsBar";
    private const string JobListingSelector = "//li[@career-loc]";
}