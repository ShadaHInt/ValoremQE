using CommonTestLibrary.SeleniumUtilities.BrowserExtensions;
using OpenQA.Selenium;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ValoremDotCom.Pages;

/// <summary>
/// Model of the main landing page.
/// </summary>
[ExcludeFromCodeCoverage]
public class Landing : ValoremPage
{
    /// <summary>
    /// Create a new model of page using the PageObject pattern.
    /// Uses the current driver in WebUiRules.
    /// Exists so it can be used for the generic Given<TPageObject>
    /// </summary>
    public Landing() : base() => WaitForPageLoad();

    /// <summary>
    /// Create a new page model.
    /// </summary>
    /// <param name="browser">The browser controlling the page.</param>
    /// <remarks>
    /// This makes sure the page is ready before control is passed
    /// back to the test scenario.
    /// </remarks>
    public Landing(IWebDriver browser) : base(browser) => WaitForPageLoad();

    private void WaitForPageLoad()
    {
        WebDriver.WaitForElement(StartProjectSelector, TimeSpan.FromSeconds(10));
        WaitForPageLoad("Valorem", TimeSpan.FromSeconds(5));
    }

    // Private & protected elements properties are placed towards the bottom.
    //
    // We look for the element WHEN it is needed for a number of reasons:
    // 1 - The page load is already "gated" so any delay is minimal
    // 2 - Elements may be dynamic
    // 3 - Minimizes the time in the constructor
    private IWebElement StartProjectButton => GetByName(StartProjectSelector);
    private IWebElement SeeOurWorkButton => GetByCss(SeeOurWorkSelector);
    private IWebElement LearnMoreLink => WebDriver.FindElementByIdCssOrXpath(LearnMoreSelector);
    private IWebElement CareersLink => WebDriver.FindElementByIdCssOrXpath(CareersSelector);

    private const string StartProjectSelector = "#startAProjectButton";
    private const string SeeOurWorkSelector = "#bodyElm > div.container-fluid.no-padding.bg-white.bg-margin > div:nth-child(2) > div:nth-child(4) > div.home-clients > a";
    private const string LearnMoreSelector = "#bodyElm > div.container-fluid.no-padding.bg-white.bg-margin > div:nth-child(2) > div.container.home-introduction > div.header > a";
    private const string CareersSelector = "#bodyElm > div.container-fluid.no-padding.bg-white.bg-margin > div:nth-child(2) > div.home-careers > div > div.content > div > a";
}