using System;
using System.Diagnostics.CodeAnalysis;

namespace ValoremDotCom.Pages;

[ExcludeFromCodeCoverage]
public class ItModernization : FreeOfferPage
{
    protected override void WaitForPageLoad() => WaitForPageLoad("IT Modernization", TimeSpan.FromSeconds(15));
}
