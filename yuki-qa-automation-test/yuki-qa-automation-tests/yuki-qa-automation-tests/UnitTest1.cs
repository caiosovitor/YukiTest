using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace yuki_qa_automation_tests
{
    public class Tests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
     
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--ignore-certificate-errors");
            chromeOptions.AddArgument("--allow-insecure-localhost");
            chromeOptions.AddArgument("--disable-infobars"); // Disables the information bar
            chromeOptions.AddArgument("--disable-popup-blocking"); // Disables popup blockade
            chromeOptions.AddArgument("--disable-default-apps"); // Disable Standard Applications

            // Adds preferences to avoid default browser definition popups
            chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
            chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
            chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.notifications", 2);
            chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.automatic_downloads", 1);
            chromeOptions.AddUserProfilePreference("browser.default_browser_infobar", false);


            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20); // Page Charging Time
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

    [Test]

        public void NavigateToPageAndCheckTheTittle(string pageName, string pageTittle)
    {
        // Navigating to the url of application
        driver.Navigate().GoToUrl("https://localhost:5001/");
        
        //Clicking above the desired page (the page desired will be informed in the parameter pageName)
        driver.FindElement(By.XPath($"=//href[text()=Home/'{pageName}']")).Click();
        
        //Waiting until the title of the page is visible, which means that the user was redirected to the page
         wait.Until(d => d.Title.Contains(pageTittle));

    }

    private decimal GetSumOAllInvoices()
    {
        // Bringing the total value of the invoices
        string sumText = driver.FindElement(By.XPath("//table/tbody/tr/td[text()='Sum of invoices']/following-sibling::td")).Text;
        return ParseAmount(sumText);
    }

    private decimal GetAmountSpecificInvoice(string invoiceNumber)
    {
        // Bringing the specific value of the invoice according invoice ID passed in the parameter invoiceNumber
        string amountText = driver.FindElement(By.XPath($"//td[text()='{invoiceNumber}']/following-sibling::td")).Text;
        return ParseAmount(amountText);
    }
    
    public void TestNavigationAndInvoiceSummary()
    {
        // Navigating to the Invoices page passing by parameter the Invoices name that NaviateToPage will use to click
        NavigateToPageAndCheckTheTittle("Invoices", "Invoices");

        // Verifying the sum of the invoices
        decimal expectedValue = 963.97m;
        decimal actualValue = GetSumOAllInvoices();
        Assert.AreEqual(expectedValue, actualValue, "The sum of the invoices is incorrect."); //The message "The sum of the ivoices is incorrect will be show in case the value found is incorrect"

        // Bringing the 'I634' invoices value and verify the value of this invoice'
        decimal expectedAmountValue = 423.99m;
        decimal actualAmountValue = GetAmountSpecificInvoice("I634");
        Assert.AreEqual(expectedAmountValue, actualAmountValue, "The value of the invoice 'i634' is incorrect.");//The message "The value of the invoice ´i634´ is incorrect will be show in case the value found is incorrect"

    }
    public void TestNavigationToPrivacyAndHomePages()
    {
        // Navigating to Privacy page and waiting until Tittle is showed
        NavigateToPageAndCheckTheTittle("Privacy", "Privacy Policy");
        // Navigating to Home page and waiting until Tittle is showed
        NavigateToPageAndCheckTheTittle("Home", "Welcome");

    }

    private decimal ParseAmount(string amountValueText)
    {
        //Removing EUR and converting to the decimal
        return decimal.Parse(amountValueText.Replace(" EUR", ""));
    }
}

}
