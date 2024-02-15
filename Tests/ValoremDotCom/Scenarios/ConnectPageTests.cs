using CommonTestLibrary.AzureDevOps.Attributes;
using CommonTestLibrary.Frameworks.WebUi;
using CommonTestLibrary.Planner;
using CommonTestLibrary.Planner.Predicates;
using CommonTestLibrary.SeleniumUtilities.BrowserExtensions;
using CommonTestLibrary.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using ValoremDotCom.Rules;

namespace ValoremDotCom.Scenarios;

[ExcludeFromCodeCoverage]
[TestClass]
public class ValoremConnectPageScenarios : ValoremDotComScenarios
{
    [Description("The user may visit the page.")]
    [TestMethod]
    [TestCategory(MustPass)]
    [Related(217924)]
    public void UserMayVisitTheConnectPage()
    {
        Given(
            "the user has accepted cookies and is on the contact page.",
            // Demonstrate how to combine sets of predicates functionally
            WebUiRules.DefaultBrowserOpened
                .And(ValoremDotComRules.CookiesAccepted) // List of predicates
                .And(ValoremDotComRules.UrlConnect)); // Single predicate
        Then(
            "the user is presented text that says 'Contact Us'",
            () => WebUiRules.Browser.TextIsFound("Contact Us"));
    }

    [Description("The user is shown a Connect Page that is accessible.")]
    [TestCategory(Accessibility)]
    [TestCategory(Nightly)]
    [Regression(220502)]
    [TestMethod]
    public void TheConnectPageIsAccessible()
    {
        Pages.Connect page = Given<Pages.Connect>(
            ValoremDotComRules.ConnectPage,
            ValoremDotComRules.DefaultBrowserType,
            new[] { ValoremDotComRules.CookiesAccepted });
        ThenThePageIsAccessible(page);
    }

    [Description("The user is shown a request form that is accessible.")]
    [TestCategory(Accessibility)]
    [TestCategory(Nightly)]
    [TestCategory(Weekly)]
    [Regression(220502)]
    [TestMethod]
    public void TheContactRequestFormIsAccessible()
    {
        Pages.Connect connectPage = Given<Pages.Connect>(
            ValoremDotComRules.ConnectPage,
            ValoremDotComRules.DefaultBrowserType,
            new[] { ValoremDotComRules.CookiesAccepted });
        ThenThePageIsAccessible(connectPage);
    }

    [Description("Validate that the user may submit a request to be contacted.")]
    [TestMethod]
    [TestCategory(MustPass)]
    public void UserMaySubmitContactRequest()
    {
        const string FormCompletedAt = "form_completion_time";

        // It is best to wait to create the PageObject until you need it.
        // Note that we are doing our interactions with the page IN THE SCENARIO
        // since using rules would cause the internal state of the PageObject to be
        // lost when the rule exits scope.
        Pages.Connect connectPage = Given<Pages.Connect>(
            ValoremDotComRules.ConnectPage,
            ValoremDotComRules.DefaultBrowserType,
            new[] { ValoremDotComRules.CookiesAccepted });
        When(
            "the user fills out the 'Start a Project' form.",
            () => connectPage.FillContactRequest(
                "Business Inquiries",
                "Valorem",
                "Professional Services",
                EasyToCleanupFirstName,
                EasyToCleanupLastName,
                "(816) 398-8949",
                "valorem@reply.com",
                "Quality Engineer",
                "TEST TEST TEST TEST"),
            adds: new CommonTestLibrary.Planner.State.ActionState(
                new Predicate[]
                {
                    new(ValoremDotComRules.ContactRequestFormFilled)
                }));
        When(
            "the user submits the form",
            () =>
            {
                connectPage.SubmitContactRequestForm();
                MustBe.True(WebUiRules.Browser.WaitForPage("/Thank_You---Contact-Us.html"), "The page did not navigate after the submission button was clicked.");
            },
            removes: new CommonTestLibrary.Planner.State.ActionState(ValoremDotComRules.ContactRequestFormFilled.And(ValoremDotComRules.ContactDialogOpen).And(WebUiRules.AnyPage)),
            adds: new CommonTestLibrary.Planner.State.ActionState(
                new Predicate[]
                {
                    new(ValoremDotComRules.ContactRequestFormSubmitted),
                    new(ValoremDotComRules.UrlContactFormSubmitted),
                    // The "Adds" are processed BEFORE the action is performed
                    // Since this step only clicks the submit button
                    // and then waits for the response page
                    // we can use the time stored
                    // in FormCompletedAt as a way to
                    // approximate the time the Submit
                    // button was clicked.
                    new(FormCompletedAt, DateTime.UtcNow)
                }));
        Then(
            "the submission is accepted within 10 seconds.",
            (state) =>
            {
                // This finds the predicate that stored the
                // time of when the submit button was clicked.
                // Objects are stored separately from the arguments
                // withing a predicate.
                DateTime submissionTime = GetObject<DateTime>(FormCompletedAt);
                TimeSpan timeToProcess = DateTime.UtcNow - submissionTime;

                return timeToProcess.TotalSeconds < 10.0f;
            });
        Then(
           "text thanking the user is presented",
           (state) => WebUiRules.Browser.TextIsFound("Thank You"));
    }

    /// <summary>
    /// Every test class must have a ClassInitialize.
    /// You can not simply tag a function in a base class. It will not work.
    /// A default implementation is provided.
    /// 
    /// Note that the context is only the STARTING context.
    /// The context will be updated by the test running between
    /// every test.
    /// </summary>
    /// <param name="context">The logging and artifact context that the tests START with.</param>
    [ClassInitialize]
    public static void ClassInitialize(
        TestContext context
    )
    {
        Initialize(context);

        TaskEstimation.Load("Scenarios\\execution_times.csv");
    }

    /// <summary>
    /// Every test class must have a ClassCleanup.
    /// You can not simply tag a function in a base class. It will not work.
    /// A default implementation is provided.
    /// </summary>
    [ClassCleanup]
    public static void ClassCleanup() => Cleanup();

    // Data that is easy for the Dev & Ops to find
    // and clean up on a regular basis.
    private const string EasyToCleanupFirstName = "Kochi";
    private const string EasyToCleanupLastName = "Kochi";
}