using CommonTestLibrary.Frameworks.WebUi;
using ValoremDotCom.Rules;

namespace ValoremDotCom.Scenarios;

/// <summary>
/// Class to hold common definitions and functions
/// for any scenarios exercising pages on
/// Valorem.com
/// </summary>
public abstract class ValoremDotComScenarios : WebUiTests<ValoremDotComRules>
{
    protected const string Accessibility = "Accessibility";
    protected const string MustPass = "MustPass";
    protected const string Nightly = "Nightly";
    protected const string Weekly = "Weekly";
}
