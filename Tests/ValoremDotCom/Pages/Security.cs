using System;
using System.Diagnostics.CodeAnalysis;

namespace ValoremDotCom.Pages;

[ExcludeFromCodeCoverage]
public class Security : FreeOfferPage
{
    protected override void WaitForPageLoad() => WaitForPageLoad("Security", TimeSpan.FromSeconds(15));
}
