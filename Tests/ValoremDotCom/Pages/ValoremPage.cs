using CommonTestLibrary.Frameworks.WebUi.PageObjectModel;
using OpenQA.Selenium;
using System.Diagnostics.CodeAnalysis;

namespace ValoremDotCom.Pages;

/// <summary>
/// Common code to create models for things that are on ALL pages
/// for the Valorem marketing site.
/// 
/// By including this in all of the other page models, then
/// you automatically gain the header and cookies models.
/// </summary>
[ExcludeFromCodeCoverage]
public class ValoremPage : PageObject
{
    /// <summary>
    /// Sub-page object that abstracts and models the Cookie Consent modal dialog.
    /// </summary>
    public CookiesConsentModal CookiesConsent { get; init; }

    /// <summary>
    /// The model of the header menu that is available on all of the marketing pages.
    /// </summary>
    public HeaderMenu HeaderMenu { get; init; }

    /// <summary>
    /// Create a new model of page using the PageObject pattern.
    /// Uses the current driver in WebUiRules.
    /// Exists so it can be used for the generic Given<TPageObject>
    /// </summary>
    public ValoremPage()
    {
        CookiesConsent = new(CommonTestLibrary.Frameworks.WebUi.WebUiRules.Browser);
        HeaderMenu = new(CommonTestLibrary.Frameworks.WebUi.WebUiRules.Browser);
    }

    /// <summary>
    /// Create a new model a generic page on the Valorem site.
    /// </summary>
    /// <param name="browser">The browser controlling the page.</param>
    public ValoremPage(
        IWebDriver browser
    ) : base(browser)
    {
        CookiesConsent = new(browser);
        HeaderMenu = new(browser);
    }
}