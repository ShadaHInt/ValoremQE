using CommonTestLibrary.AzureDevOps.Attributes;
using CommonTestLibrary.Frameworks.WebUi;
using CommonTestLibrary.Frameworks.WebUi.PageObjectModel;
using CommonTestLibrary.Planner;
using CommonTestLibrary.SeleniumUtilities;
using CommonTestLibrary.SeleniumUtilities.BrowserExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Diagnostics.CodeAnalysis;
using ValoremDotCom.Pages;
using ValoremDotCom.Rules;

namespace ValoremDotCom.Scenarios;

/// <summary>
/// Demonstrates how to quickly and easily create tests using
/// the WebUi framework. This provides base functionality around
/// logging, cleanup, initialization, and more.
/// </summary>
[ExcludeFromCodeCoverage]
[TestClass]
public class ValoremLandingPageScenarios : ValoremDotComScenarios
{
    [Description("The user is presented with a landing page when they load ValoremReply.com")]
    [DataTestMethod]
    [DataRow(Browser.Chrome)]
    [DataRow(Browser.Edge)]
    [TestCategory(MustPass)]
    public void UserMayVisitLandingPage(
        Browser browserType
    )
    {
        // Demonstrates a simple scenario that with multiple browser types.
        // Note how the browser predicate is formed.
        Given(
            $"the user has an open instance of {browserType}.",
            $"browser({browserType})");
        When(
            "the user navigates to Valorem.com",
            Scenario.Rules.NavigateHome);
        Then(
            "the url has `valoremreply.com` in it.",
            () => WebUiRules.Browser.UrlContains("valoremreply.com"));
        Then(
            "the text `Valorem Reply` is found on the page.",
            () => WebUiRules.Browser.TextIsFound("Valorem Reply"));
        Then(
            $"the name of the test in the context (found='{TestContext.TestName}') is 'UserMayVisitLandingPage'",
            () => string.Equals("UserMayVisitLandingPage", TestContext.TestName, System.StringComparison.InvariantCultureIgnoreCase));
    }

    [Description("The user is presented with a cookies consent dialog when they load the home page.")]
    [TestCategory(Weekly)]
    [TestMethod]
    public void UserIsPresentedWithCookiesConsent()
    {
        // Demonstrates a 'Then' lambda that uses a PageObject.
        Given(
            "previous browser instances are closed.",
            WebUiRules.BrowserClosed);
        Pages.Landing landingPage = Given<Pages.Landing>(
            ValoremDotComRules.HomePage,
            ValoremDotComRules.DefaultBrowserType);
        Then(
            "the cookies consent modal is presented",
            () => landingPage.CookiesConsent.IsAlertShown());
        Then(
            $"the name of the test in the context (found='{TestContext.TestName}') is 'UserIsPresentedWithCookiesConsent'",
            () => string.Equals("UserIsPresentedWithCookiesConsent", TestContext.TestName, System.StringComparison.InvariantCultureIgnoreCase));
    }

    [Description("The user is presented with a page that is accessible.")]
    [TestCategory(Accessibility)]
    [TestCategory(Nightly)]
    [Regression(220509)]
    [TestMethod]
    public void TheValoremHomePageIsAccessible()
    {
        Pages.Landing landingPage = Given<Pages.Landing>(
            ValoremDotComRules.HomePage,
            ValoremDotComRules.DefaultBrowserType);
        Then(
            "the Valorem.com home page passes an accessability assessment.",
            () => landingPage.IsAccessible(TestContext));
    }

    [Description("The user may visit pages from the landing page.")]
    [TestCategory(MustPass)]
    [DataTestMethod]
    [DataRow(Pages.HeaderMenu.TopLevelMenuItems.Connect, "Contact Us")]
    [DataRow(Pages.HeaderMenu.TopLevelMenuItems.Work, "OUR WORK")]
    [DataRow(Pages.HeaderMenu.TopLevelMenuItems.Careers, "Love your Career.")]
    [Related(217924)]
    public void UserMayVisit(
        Pages.HeaderMenu.TopLevelMenuItems page,
        string expectedText
    )
    {
        // Demonstrates how a 'When' clause can be built using data rows
        // Also shows how to access the Browser within a scenario.
        Pages.Landing landingPage = Given<Pages.Landing>(
            ValoremDotComRules.HomePage,
            ValoremDotComRules.DefaultBrowserType);
        When(
            $"the user clicks `{page}`",
            () => landingPage.HeaderMenu.ClickMenuItem(page),
            adds: new CommonTestLibrary.Planner.State.ActionState($"url({page})"),
            removes: new CommonTestLibrary.Planner.State.ActionState("url(*)"));
        Then(
            $"the text `{expectedText}` is found.",
            () => WebUiRules.Browser.TextIsFound(expectedText));
    }

    [Description("The user is shown dropdown menu items.")]
    [TestMethod]
    [TestCategory(MustPass)]
    [Related(217950)]
    public void TheUserIsShownSolutionsDropdownItems()
    {
        // Demonstrates how BDD calls can be included inside other
        // control structures.
        //
        // Also exercises some code for coverage
        string[] menuItems = new[]
        {
            HeaderMenu.Modernization,
            HeaderMenu.Data,
            HeaderMenu.Workplace,
            HeaderMenu.Product,
            HeaderMenu.Security
        };

        Pages.Landing page = Given<Pages.Landing>(
            ValoremDotComRules.HomePage,
            ValoremDotComRules.DefaultBrowserType);
        DropDownMenu menu = page.HeaderMenu.Menus[Pages.HeaderMenu.TopLevelMenuItems.Solutions] as DropDownMenu;

        When(
            $"the user expands the `{menu.Name}` menu. ({menu})",
            () => menu.Click());

        foreach (string entry in menuItems)
        {
            Then(
                $"the item '{entry}' is shown and enabled.",
                () =>
                {
                    // Variable scoping in Lambdas can be weird. For instance, "menu" is not visible from the outer scope
                    DropDownMenu menu = page.HeaderMenu.Menus[Pages.HeaderMenu.TopLevelMenuItems.Solutions] as DropDownMenu;
                    IWebElement menuLink = menu.GetMenuSelection(entry);

                    return menuLink != null && menuLink.Enabled;
                });
        }

        Then(
            $"the name of the test in the context (found='{TestContext.TestName}') is 'TheUserIsShownSolutionsDropdownItems'",
            () => string.Equals("TheUserIsShownSolutionsDropdownItems", TestContext.TestName, System.StringComparison.InvariantCultureIgnoreCase));
    }

    [Description("The user is shown top level link in the header menu.")]
    [TestMethod]
    [TestCategory(MustPass)]
    public void TheUserIsShownHeaderMenuLinks()
    {
        Pages.HeaderMenu.TopLevelMenuItems[] menuItems = new Pages.HeaderMenu.TopLevelMenuItems[] { Pages.HeaderMenu.TopLevelMenuItems.Home, Pages.HeaderMenu.TopLevelMenuItems.Careers, Pages.HeaderMenu.TopLevelMenuItems.Work, Pages.HeaderMenu.TopLevelMenuItems.Connect };

        // Also exercises some code for coverage

        Pages.Landing page = Given<Pages.Landing>(
            ValoremDotComRules.HomePage,
            ValoremDotComRules.DefaultBrowserType);

        foreach (Pages.HeaderMenu.TopLevelMenuItems entry in menuItems)
        {
            MenuItem menuItem = page.HeaderMenu.Menus[entry] as MenuItem;

            Then(
                $"the item '{entry}' is shown and enabled. ({menuItem?.ToString() ?? "null"})",
                () =>
                {
                    // Variable scoping in Lambdas can be weird. For instance, "menu" is not visible from the outer scope
                    IWebElement link = menuItem.Element;

                    return menuItem != null && link.Enabled;
                });
        }
    }

    [TestInitialize]
    public void BeforeTest()
    {
        Scenario.UpdateContext(TestContext);

        base.TestInitialize();
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
}