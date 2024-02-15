using CommonTestLibrary.SeleniumUtilities.BrowserExtensions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ValoremDotCom.Pages;

[ExcludeFromCodeCoverage]
public class DataAndAi : FreeOfferPage
{
    protected override void WaitForPageLoad() => WebDriver.WaitUntilPageIsReady();
}
