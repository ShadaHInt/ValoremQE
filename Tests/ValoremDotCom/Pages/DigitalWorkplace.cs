using CommonTestLibrary.SeleniumUtilities.BrowserExtensions;
using CommonTestLibrary.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ValoremDotCom.Pages;

/// <summary>
/// Models the DigitalWorkplace page.
/// </summary>
[ExcludeFromCodeCoverage]
public class DigitalWorkplace : FreeOfferPage
{
    /// <summary>
    /// Create a new model of the Digital Workplace page.
    /// Uses the current driver in WebUiRules.
    /// Exists so it can be used for the generic Given<TPageObject>
    /// </summary>
    public DigitalWorkplace() : base(CommonTestLibrary.Frameworks.WebUi.WebUiRules.Browser) =>
        InitializePage();

    /// <summary>
    /// Create a new model of the Digital Workplace page.
    /// </summary>
    /// <param name="browser">The browser controlling this page.</param>
    public DigitalWorkplace(
        IWebDriver browser
    ) : base(browser) =>
        InitializePage();

    /// <summary>
    /// Open the video from the page into its own modal.
    /// </summary>
    public void OpenVideo()
    {
        MustBe.False(_isVideoOpen, "Video is already open.");

        WebDriver.ClickAt(WatchVideoButton);

        _isVideoOpen = true;
    }

    /// <summary>
    /// Wait for the video to finish playing.
    /// </summary>
    public void WaitForVideoToFinish()
    {
        new WebDriverWait(WebDriver, TimeSpan.FromMinutes(2)).Until(d => IsVideoFinishedPlaying());
    }

    /// <summary>
    /// Is the video playing?
    /// </summary>
    /// <returns>True if the video is not playing.</returns>
    public bool IsVideoFinishedPlaying()
    {
        MustBe.True(_isVideoOpen, "Video modal is NOT open.");

        return WebDriver.TextIsFound("More from Valorem Reply");
    }

    /// <summary>
    /// Is the video watch button enabled?
    /// </summary>
    /// <returns>True if the button is present and enabled.</returns>
    public bool IsWatchVideoButtonEnabled() => WatchVideoButton?.Enabled ?? false;

    /// <summary>
    /// Close the video player.
    /// </summary>
    public void CloseVideoWindow()
    {
        MustBe.True(_isVideoOpen, "Video modal is NOT open.");

        CloseVideoWindowButton.Click();
        _isVideoOpen = false;
    }

    protected override void WaitForPageLoad() => WaitForPageLoad("Digital Workplace", TimeSpan.FromSeconds(15));

    private IWebElement WatchVideoButton => WebDriver.FindElementByIdCssOrXpath("#bodyElm > main > div.container-fluid.no-padding.bg-white.bg-margin > div.capability-item-container.solutions-body-class-Digital-Workplace > div.solutions-info-row > div > div > a.home-button.watch-video");
    private IWebElement CloseVideoWindowButton => WebDriver.FindElementByIdCssOrXpath("#videoModal > div > div > button");

    private bool _isVideoOpen = false;
}