using CommonTestLibrary.Frameworks.WebUi.PageObjectModel;
using CommonTestLibrary.Utilities;
using CommonTestLibrary.Utilities.Exceptions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ValoremDotCom.Pages;

/// <summary>
/// Shows how you can model a HeaderMenu that is on every page
/// and then easily include it in all other page models.
/// 
/// Shows how to use the new MenuItems interfaces.
/// </summary>
[ExcludeFromCodeCoverage]
public class HeaderMenu : PageObject
{
    //Local constants
    public const string Modernization = "IT MODERNIZATION";
    public const string Data = "DATA & AI";
    public const string Workplace = "DIGITAL WORKPLACE";
    public const string Product = "PRODUCT TRANSFORMATION";
    public const string Security = "SECURITY";

    /// <summary>
    /// The menu items that are in the header menu.
    /// </summary>
    public enum TopLevelMenuItems
    {
        None,
        Home,
        Solutions,
        Careers,
        Work,
        About,
        Connect
    }

    /// <summary>
    /// The menu items in the header.
    /// </summary>
    public Dictionary<TopLevelMenuItems, IMenuItem> Menus { get; private set; }

    /// <summary>
    /// Expand one of the drop down menus.
    /// </summary>
    /// <param name="menuToExpand">The menu to expand.</param>
    /// <returns>The dropdown menu that was expanded.</returns>
    public DropDownMenu ExpandMenu(
        TopLevelMenuItems menuToExpand
    )
    {
        MustBe.NotEqual(menuToExpand, ExpandedMenu, $"Selection {menuToExpand} is already expanded.");

        switch (menuToExpand)
        {
            case TopLevelMenuItems.Solutions:
            case TopLevelMenuItems.About:
                Menus[menuToExpand].Click();
                ExpandedMenu = menuToExpand;

                return Menus[menuToExpand] as DropDownMenu;
            default:
                throw new ArgumentException($"Selection {menuToExpand} is not an expandable item.");
        }
    }

    /// <summary>
    /// Click a menu item in the header.
    /// These are items that **are not** drop down menus.
    /// </summary>
    /// <param name="item">The item to click.</param>
    public void ClickMenuItem(
        TopLevelMenuItems item
    )
    {
        switch (item)
        {
            case TopLevelMenuItems.Solutions:
            case TopLevelMenuItems.About:
                throw new TestIssueException($"Selection {item} is an expandable item. Please use `ExpandMenu` instead.");

            case TopLevelMenuItems.None:
                throw new TestIssueException("`None` is not  valid click selection.");

            default:
                Menus[item].Click();
                break;
        }
    }

    /// <summary>
    /// Initialize the header menu with the current WebUI browser.
    /// </summary>
    public HeaderMenu()
        : base(CommonTestLibrary.Frameworks.WebUi.WebUiRules.Browser)
    {
        InitializeMenu();
    }

    /// <summary>
    /// Create a new header menu model.
    /// </summary>
    /// <param name="browser">The browser controlling the menu.</param>
    public HeaderMenu(
        IWebDriver browser
    ) : base(browser)
    {
        InitializeMenu();
    }

    protected void InitializeMenu()
    {
        Menus = new()
        {
            {
                TopLevelMenuItems.Home,
                new MenuItem(
                    WebDriver,
                    "HOME",
                    "#companyLogoCt"
                )
            },
            {
                TopLevelMenuItems.Solutions,
                new DropDownMenu(
                    WebDriver,
                    "SOLUTIONS",
                    "#capabilitiesLink > a",
                    new Dictionary<string, string>()
                    {
                        {Modernization, "#capabilitiesLink > ul > li:nth-child(1) > a"},
                        {Data, "#capabilitiesLink > ul > li:nth-child(2) > a"},
                        {Workplace, "#capabilitiesLink > ul > li:nth-child(3) > a"},
                        {Product, "#capabilitiesLink > ul > li:nth-child(4) > a"},
                        {Security, "#capabilitiesLink > ul > li:nth-child(5) > a"},
                    })
            },
            {
                TopLevelMenuItems.Careers,
                new MenuItem(
                    WebDriver,
                    "CAREERS",
                    "#mainNavigationMenu > ul > li:nth-child(2) > a"
                )
            },
            {
                TopLevelMenuItems.Work,
                new MenuItem(
                    WebDriver,
                    "WORK",
                    "#mainNavigationMenu > ul > li:nth-child(3) > a"
                )
            },
            {
                TopLevelMenuItems.About,
                new DropDownMenu(
                    WebDriver,
                    "ABOUT",
                    "#aboutLink > a",
                    new Dictionary<string, string>()
                    {
                        {"OUR COMPANY", "#aboutLink > ul > li:nth-child(1) > a"},
                        {"BLOG",  "#aboutLink > ul > li:nth-child(2) > a"},
                        {"EVENTS",  "#aboutLink > ul > li:nth-child(3) > a"}
                    })
            },
            {
                TopLevelMenuItems.Connect,
                new MenuItem(
                    WebDriver,
                    "CONNECT",
                    "#mainNavigationMenu > ul > li:nth-child(5) > a"
                )
            }
        };
    }

    private TopLevelMenuItems ExpandedMenu = TopLevelMenuItems.None;
}