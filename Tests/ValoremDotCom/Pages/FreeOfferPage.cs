using CommonTestLibrary.SeleniumUtilities.BrowserExtensions;
using OpenQA.Selenium;
using System.Diagnostics.CodeAnalysis;

namespace ValoremDotCom.Pages;

[ExcludeFromCodeCoverage]
public abstract class FreeOfferPage : ValoremPage
{
    /// <summary>
    /// Create a new model of a page with the free offer popup.
    /// Uses the most recent browser available.
    /// </summary>
    /// <returns></returns>
    public FreeOfferPage() : base(CommonTestLibrary.Frameworks.WebUi.WebUiRules.Browser) => InitializePage();

    /// <summary>
    /// Create a new model of a page with the free offer popup.
    /// </summary>
    /// <param name="browser">The browser controlling this page.</param>
    public FreeOfferPage(IWebDriver browser) : base(browser) => InitializePage();

    protected abstract void WaitForPageLoad();

    protected void InitializePage()
    {
        WaitForPageLoad();
        // The first dismissal only moves it to the corner.
        // We need to close it TWICE to truly get it out of the way.
        DismissOfferPopup(1);
        DismissOfferPopup(2);
    }

    /// <summary>
    /// If a free offer pop-up is presented, dismiss it.
    /// </summary>
    private void DismissOfferPopup(
        in int popupNumber
    )
    {
        IWebElement closeOffer = GetFreeOfferCloseButton(popupNumber);

        if (closeOffer != null)
        {
            try
            {
                WebDriver.ClickAt(closeOffer);
                WebDriver.WaitUntilPageIsReady();
            }
            catch
            {
                // Failing to click on the close icon is not
                // a critical failure
            }
        }
    }

    private IWebElement GetFreeOfferCloseButton(
        in int popupNumber
    ) =>
        WebDriver.FindElementByIdCssOrXpath($"//*[@class='close-{popupNumber} popup-close']");
}
