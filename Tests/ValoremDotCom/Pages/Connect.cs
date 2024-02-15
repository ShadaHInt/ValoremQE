using CommonTestLibrary.SeleniumUtilities.BrowserExtensions;
using CommonTestLibrary.SeleniumUtilities.WebElementExtensions;
using CommonTestLibrary.Utilities;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ValoremDotCom.Pages;

/// <summary>
/// Model of the main landing page.
/// </summary>
[ExcludeFromCodeCoverage]
public class Connect : ValoremPage
{
    /// <summary>
    /// Build a new instance of the Connect PageObject using
    /// the driver that is currently in WebUiRules
    /// Exists so it can be used for the generic Given<TPageObject>
    /// </summary>
    /// <returns>A new Connect PageObject</returns>
    public Connect() : base() => WaitForPageLoad();

    /// <summary>
    /// Build a new Page Object Model
    /// </summary>
    /// <param name="browser">The browser controlling the connect page.</param>
    public Connect(IWebDriver browser) : base(browser) => WaitForPageLoad();


    /// <summary>
    /// Demonstration for a workflow.
    /// 
    /// Please note that this ONLY fills in the form.
    /// A separate method exists to submit the form.
    /// </summary>
    /// <param name="inquiryType">Why is the customer contacting Valorem?</param>
    /// <param name="companyName">The name of the company that will be filled in.</param>
    /// <param name="industry">The industry the contacting company operates in.</param>
    /// <param name="contactFirstName">The first name of the contacting company's employee.</param>
    /// <param name="contactLastName">The last name of the contacting company's employee.</param>
    /// <param name="contactPhone">The phone number Valorem will call.</param>
    /// <param name="contactEmail">The email address Valorem will contact.</param>
    /// <param name="contactJobTitle">The job title of the employee.</param>
    /// <param name="otherDetails">Any other text we wish to give Valorem</param>
    public void FillContactRequest(
        [NotNull] in string inquiryType,
        [NotNull] in string companyName,
        [NotNull] in string industry,
        [NotNull] in string contactFirstName,
        [NotNull] in string contactLastName,
        [NotNull] in string contactPhone,
        [NotNull] in string contactEmail,
        [NotNull] in string contactJobTitle,
        [NotNull] in string otherDetails
    )
    {
        MustBe.NotNull(inquiryType, "Inquiry may not be null");
        MustBe.NotNull(companyName, "Company name may not be null.");
        MustBe.NotNull(industry, "Industry name may not be null.");
        MustBe.NotNull(contactFirstName, "First name may not be null.");
        MustBe.NotNull(contactLastName, "Last name may not be null.");
        MustBe.NotNull(contactPhone, "Phone number may not be null.");
        MustBe.NotNull(contactEmail, "Email name may not be null.");
        MustBe.NotNull(contactJobTitle, "Job title may not be null.");
        MustBe.NotNull(otherDetails, "Details may not be null.");

        // Gate that we already have the form open.
        MustBe.Equal(_pageState, PageStates.ContactDialog, "The contact dialog is not open yet.");

        InquirySelectBox.ChangeSelectElement(inquiryType);
        CompanyNameTextBox.SendText(companyName);
        IndustrySelectBox.ChangeSelectElement(industry);
        ContactFirstNameTextBox.SendText(contactFirstName);
        ContactLastNameTextBox.SendText(contactLastName);
        EmailTextBox.SendText(contactEmail);
        PhoneTextBox.SendText(contactPhone);
        JobTitleTextBox.SendText(contactJobTitle);
        OtherDetailsTextBox.SendText(otherDetails);
    }

    /// <summary>
    /// Only attempts to submit the form.
    /// 
    /// This does not check that the form has been filled out.
    /// </summary>
    public void SubmitContactRequestForm()
    {
        MustBe.Equal(_pageState, PageStates.ContactDialog, "The contact dialog is not open yet.");

        SendRequestButton.Click();

        WebDriver.WaitUntilPageIsReady();

        _pageState = PageStates.RequestSubmitted;
    }

    /// <summary>
    /// Gate on the company name text box. When this is visible and available
    /// then the page is ready.
    ///
    /// We want to be sure that control is not returned to the scenario
    /// until the page is ready.
    ///
    /// This makes gating and waiting simpler in the rest of the code.
    /// </summary>
    private void WaitForPageLoad() => WebDriver.WaitForElement(By.CssSelector("#Company"), TimeSpan.FromSeconds(10));

    private IWebElement FilterEnabledAndDisplayed(
        [NotNull] in By search
    )
    {
        IEnumerable<IWebElement> foundElements = WebDriver.FindElements(search);
        List<IWebElement> filteredElements = foundElements.Where(e => e.Enabled).ToList();

        filteredElements = filteredElements.Where(e => e.Displayed).ToList();
        filteredElements = filteredElements.OrderBy(e => e.GetInnerHtml().Length).ToList();

        return filteredElements.FirstOrDefault();
    }

    // Region for all of the elements. These are searches
    // so the element is found only when it is needed.
    // This allows for dynamic elements.

    // Dialog
    private IWebElement CompanyNameTextBox => FilterEnabledAndDisplayed(By.Id("Company"));

    // For some reason all of these IDs have duplicates in the DOM. The one that we want is always the last of the two.
    private IWebElement InquirySelectBox => WebDriver.FindElementByIdCssOrXpath("#new_contactusreason");
    private IWebElement ContactFirstNameTextBox => FilterEnabledAndDisplayed(By.Id("FirstName"));
    private IWebElement ContactLastNameTextBox => FilterEnabledAndDisplayed(By.Id("LastName"));
    private IWebElement PhoneTextBox => FilterEnabledAndDisplayed(By.Id("Phone"));
    private IWebElement EmailTextBox => FilterEnabledAndDisplayed(By.Id("Email"));
    private IWebElement JobTitleTextBox => FilterEnabledAndDisplayed(By.Id("Title"));
    private IWebElement IndustrySelectBox => FilterEnabledAndDisplayed(By.Id("Industry"));
    private IWebElement OtherDetailsTextBox => FilterEnabledAndDisplayed(By.CssSelector("#new_projectneed"));
    private IWebElement SendRequestButton => GetByText("Submit");

    private PageStates _pageState = PageStates.ContactDialog;

    /// <summary>
    /// What state the page is in.
    /// </summary>
    private enum PageStates
    {
        /// <summary>
        /// When the form modal is open
        /// </summary>
        ContactDialog,
        /// <summary>
        /// When the form has been submitted
        /// </summary>
        RequestSubmitted
    }
}