using CommonTestLibrary.Frameworks.WebUi.PageObjectModel;
using CommonTestLibrary.SeleniumUtilities.BrowserExtensions;
using CommonTestLibrary.SeleniumUtilities.WebElementExtensions;
using OpenQA.Selenium;
using System.Diagnostics.CodeAnalysis;

namespace ValoremDotCom.Pages;

[ExcludeFromCodeCoverage]
public class CookiesConsentModal : PageObject
{
    public IWebElement CookieAlertDiv => WebDriver.FindElementByIdCssOrXpath(CookieAlertDivSelector);
    public IWebElement AcceptButton => WebDriver.FindElementByIdCssOrXpath(AcceptCookiesSelector);
    public IWebElement DenyButton => WebDriver.FindElementByIdCssOrXpath(DenyCookiesSelector);

    /// <summary>
    /// Create a new model of the cookies-consent modal.
    /// </summary>
    /// <param name="browser">The browser that will be controlling the modal.</param>
    public CookiesConsentModal(
        IWebDriver browser
    ) : base(browser)
    {
    }

    /// <summary>
    /// Is the alert modal being displayed.
    /// </summary>
    /// <returns>True if the alert modal is being displayed.</returns>
    public bool IsAlertShown()
    {
        IWebElement div = CookieAlertDiv;

        if (div == null)
        {
            return false;
        }

        return div.SafeDisplayed();
    }

    /// <summary>
    /// Click the button that accepts cookies.
    /// </summary>
    public void Accept() => AcceptButton.Click();

    /// <summary>
    /// Click the button that denies cookies
    /// </summary>
    public void Deny() => DenyButton.Click();

    private const string CookieAlertDivSelector = "#cookieAlert";
    private const string AcceptCookiesSelector = "#cookieAgree";
    private const string DenyCookiesSelector = "#cookieDisagree";
}