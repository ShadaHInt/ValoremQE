using CommonTestLibrary.Frameworks.WebUi;
using CommonTestLibrary.Frameworks.WebUi.PageObjectModel;
using CommonTestLibrary.Planner.Rules;
using CommonTestLibrary.Planner.State;
using CommonTestLibrary.SeleniumUtilities;
using CommonTestLibrary.SeleniumUtilities.BrowserExtensions;
using CommonTestLibrary.Utilities;
using System.Diagnostics.CodeAnalysis;
using ValoremDotCom.Pages;

namespace ValoremDotCom.Rules;

/// <summary>
/// Example set of rules.
/// Extends the basic WebUiRules that automatically provides
/// browser management, logging, and other things.
/// 
/// This makes creating a new set of rules very quick and easy.
/// </summary>
[ExcludeFromCodeCoverage]
public class ValoremDotComRules : WebUiRules
{
    /// <summary>
    /// Predicate name to store when the home page
    /// was loaded.
    /// </summary>
    public string HomePageLoadedPredicateName = "home_loaded";

    /// <summary>
    /// The default browser to user for scenarios.
    /// </summary>
    public const Browser DefaultBrowserType = CommonTestLibrary.SeleniumUtilities.Browser.Edge;

    /// <summary>
    /// The parameter name of the Home page. Normally used by the URL predicate.
    /// </summary>
    public const string HomePage = "home";

    /// <summary>
    /// The page name of where the user may fill out a contact request form.
    /// </summary>
    public const string ConnectPage = "connect";

    /// <summary>
    /// The page about our data & AI offerings
    /// </summary>
    public const string DataAndAiPage = "data_and_ai";

    /// <summary>
    /// The page about our IT Modernization offerings
    /// </summary>
    public const string ItModernizationPage = "it_modernization";

    /// <summary>
    /// The page about our Product Transformation services.
    /// </summary>
    public const string ProductTransformationPage = "product_transformation";

    /// <summary>
    /// The page about our security offerings
    /// </summary>
    public const string SecurityPage = "security";

    /// <summary>
    /// The page name of where the user is sent after completing the  contact request form.
    /// </summary>
    public const string ConnectFormSubmittedPage = "connect_form_submitted";

    /// <summary>
    /// The name of the URL for Digital Workplace
    /// </summary>
    public const string DigitalWorkplacePage = "digital_workplace";

    /// <summary>
    /// Request that we are on the app's landing page.
    /// </summary>
    public const string UrlHome = "url(home)";

    /// <summary>
    /// The url representation of the contact request form page.
    /// </summary>
    public static readonly string UrlConnect = $"url({ConnectPage})";

    /// <summary>
    /// The url representation of the contact form submitted landing page.
    /// </summary>
    public static readonly string UrlContactFormSubmitted = $"url({ConnectFormSubmittedPage})";

    /// <summary>
    /// The cookies dialog has been accepted
    /// </summary>
    public const string CookiesAccepted = "cookies(accepted)";

    /// <summary>
    /// Cookies have been denied
    /// </summary>
    public const string CookiesDenied = "cookies(denied)";

    /// <summary>
    /// The "Start a Project" contact request form DIALOG is open
    /// </summary>
    public const string ContactDialogOpen = "open(connect_dialog)";

    /// <summary>
    /// Is the video player open?
    /// </summary>
    public const string VideoPlayerOpened = "open(video)";

    /// <summary>
    /// The "Start a Project" contact request form DIALOG has been filled
    /// </summary>
    public const string ContactRequestFormFilled = "filled(connect_dialog)";

    /// <summary>
    /// The "Start a Project" contact request form DIALOG has been submitted
    /// 
    /// This predicate has the "@" since the submission is only good
    /// for the current scenario.
    /// </summary>
    public const string ContactRequestFormSubmitted = "@submitted(contact)";

    /// <remark>
    /// This rule will set the URL directly.
    /// The rule will normally be triggered if there is no URL or no other way to get home.
    /// </remark>
    [PlannerRule(
        name: "navigate(home)",
        requires: new[] { AnyBrowserOpened },
        removes: new[] { "url(*)" },
        adds: new[] { UrlHome })]
    public ActionState NavigateHome(
        ActionState _,
        PlannerAction action
    )
    {
        MustBe.NotNull(Browser, nameof(Browser), "We must have a controlling browser.");

        Browser.WaitUntilPageIsReady();
        Browser.Navigate().GoToUrl("http://www.valoremreply.com");
        Browser.WaitUntilPageIsReady();

        return new();
    }

    /// <remark>
    /// This rule will fire if we are already on a page in the app.
    /// </remark>
    [UrlChangeRule(
        new string[] { ConnectFormSubmittedPage },
        HomePage)]
    public ActionState ClickHome(
        ActionState _,
        PlannerAction action
    )
    {
        ValoremPage page = new(Browser);

        page.HeaderMenu.ClickMenuItem(HeaderMenu.TopLevelMenuItems.Home);

        return new();
    }

    /// <remark>
    /// Rule to accept cookies.
    /// </remark>
    [PlannerRule(
        name: "accept(cookies)",
        requires: new[] { AnyBrowserOpened, UrlHome, "^cookies(accepted)" },
        adds: new[] { CookiesAccepted })]
    public ActionState AcceptCookies(
        ActionState _,
        PlannerAction action
    )
    {
        ValoremPage page = new(Browser);

        page.CookiesConsent.Accept();

        return new();
    }

    /// <remark>
    /// This rule will use the header menu from ANY page to get us to the 'IT Modernization' page.
    /// Note that this rule CAN NOT be triggered from the digital_workplace or connect_form_submitted pages.
    /// Several preconditions are added automatically by this rule.
    /// </remark>
    [UrlChangeRule(
        urlsNotToStartOn: new[] { ConnectFormSubmittedPage },
        endingUrl: ItModernizationPage)]
    public ActionState NavigateToItModernization(
        ActionState _,
        PlannerAction action
    ) =>
        NavigateToSolution("IT MODERNIZATION");

    /// <remark>
    /// This rule will use the header menu from ANY page to get us to the 'Data & AI' page.
    /// Note that this rule CAN NOT be triggered from the digital_workplace or connect_form_submitted pages.
    /// Several preconditions are added automatically by this rule.
    /// </remark>
    [UrlChangeRule(
        urlsNotToStartOn: new[] { ConnectFormSubmittedPage },
        endingUrl: DataAndAiPage)]
    public ActionState NavigateToDataAndAi(
        ActionState _,
        PlannerAction action
    ) =>
        NavigateToSolution("DATA & AI");

    /// <remark>
    /// This rule will use the header menu from ANY page to get us to the 'Digital Workplace' page.
    /// Note that this rule CAN NOT be triggered from the digital_workplace or connect_form_submitted pages.
    /// Several preconditions are added automatically by this rule.
    /// </remark>
    [UrlChangeRule(
        urlsNotToStartOn: new[] { ConnectFormSubmittedPage },
        endingUrl: DigitalWorkplacePage)]
    public ActionState NavigateToDigitalWorkplace(
        ActionState _,
        PlannerAction action
    ) =>
        NavigateToSolution("DIGITAL WORKPLACE");

    /// <remark>
    /// This rule will use the header menu from ANY page to get us to the 'Product Transformation' page.
    /// Note that this rule CAN NOT be triggered from the digital_workplace or connect_form_submitted pages.
    /// Several preconditions are added automatically by this rule.
    /// </remark>
    [UrlChangeRule(
        urlsNotToStartOn: new[] { ConnectFormSubmittedPage },
        endingUrl: ProductTransformationPage)]
    public ActionState NavigateToProductTransformation(
        ActionState _,
        PlannerAction action
    ) =>
        NavigateToSolution("PRODUCT TRANSFORMATION");

    /// <remark>
    /// This rule will use the header menu from ANY page to get us to the Security page.
    /// Note that this rule CAN NOT be triggered from the digital_workplace or connect_form_submitted pages.
    /// Several preconditions are added automatically by this rule.
    /// </remark>
    [UrlChangeRule(
        urlsNotToStartOn: new[] { ConnectFormSubmittedPage },
        endingUrl: SecurityPage)]
    public ActionState NavigateToSecurity(
        ActionState _,
        PlannerAction action
    ) =>
        NavigateToSolution("SECURITY");


    /// <summary>
    /// Rule to get to the "Careers" page.
    /// This rule can not be triggered from the digital_workplace, connect_form_submitted, or careers pages.
    /// </summary>
    [UrlChangeRule(
        urlsNotToStartOn: new[] { DigitalWorkplacePage, ConnectFormSubmittedPage },
        endingUrl: "careers")]
    public ActionState ClickCareers(
        ActionState _,
        PlannerAction action
    )
    {
        ValoremPage page = new(Browser);

        page.HeaderMenu.ClickMenuItem(HeaderMenu.TopLevelMenuItems.Careers);

        return new();
    }

    /// <summary>
    /// Rule to navigate to the "Work" page using the header menu.
    /// This rule can not be triggered from connect_form_submitted page.
    /// </summary>
    [UrlChangeRule(
        urlsNotToStartOn: new[] { ConnectFormSubmittedPage },
        endingUrl: "work")]
    public ActionState ClickWork(
        ActionState _,
        PlannerAction action
    )
    {
        ValoremPage page = new(Browser);

        page.HeaderMenu.ClickMenuItem(HeaderMenu.TopLevelMenuItems.Work);

        return new();
    }

    /// <summary>
    /// Rule that achieves the "connect" page.
    /// For this rule to fire, the user must already be on the "home" page.
    /// </summary>
    [UrlChangeRule(
        startingUrl: HomePage,
        endingUrl: ConnectPage)]
    public ActionState ClickConnect(
        ActionState _,
        PlannerAction action
    )
    {
        ValoremPage page = new(Browser);

        page.HeaderMenu.ClickMenuItem(HeaderMenu.TopLevelMenuItems.Connect);

        return new();
    }

    private ActionState NavigateToSolution(
        [NotNull] in string linkText
    )
    {
        MustBe.ValidString(linkText);

        ValoremPage page = new(Browser);

        DropDownMenu menu = page.HeaderMenu.ExpandMenu(HeaderMenu.TopLevelMenuItems.Solutions);

        MustBe.Equal(menu.Name, "SOLUTIONS", "Did not get the solutions menu back.");

        menu.GetMenuSelection(linkText).Click();

        return new();
    }
}