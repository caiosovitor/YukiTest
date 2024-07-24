using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

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
            chromeOptions.AddArgument("--disable-infobars");
            chromeOptions.AddArgument("--disable-popup-blocking");
            chromeOptions.AddArgument("--disable-default-apps");

            chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
            chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
            chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.notifications", 2);
            chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.automatic_downloads", 1);
            chromeOptions.AddUserProfilePreference("browser.default_browser_infobar", false);

            driver = new ChromeDriver(chromeOptions);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20); // Timeout for page load
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        private void NavigateToPageAndCheckTitle(string pageName, string expectedTitle)
        {
            // Navigate to the base URL
            driver.Navigate().GoToUrl("https://localhost:5001/");
            
            // Click on the link with the given pageName
            var link = driver.FindElement(By.LinkText(pageName));
            link.Click();
            
            // Wait until the title of the page contains the expectedTitle
            wait.Until(d => d.Title.Contains(expectedTitle));
        }

        private decimal GetSumOfAllInvoices()
        {
            // Retrieve the total sum of invoices
            string sumText = driver.FindElement(By.XPath("//table/tbody/tr/td[text()='Sum of invoices']/following-sibling::td")).Text;
            return ParseAmount(sumText);
        }

        private decimal GetAmountOfSpecificInvoice(string invoiceNumber)
        {
            // Retrieve the amount of a specific invoice
            string amountText = driver.FindElement(By.XPath($"//td[text()='{invoiceNumber}']/following-sibling::td")).Text;
            return ParseAmount(amountText);
        }

        private decimal ParseAmount(string amountText)
        {
            // Remove ' EUR' and convert to decimal
            return decimal.Parse(amountText.Replace(" EUR", "").Trim());
        }

        [Test]
        public void TestNavigationAndInvoiceSummary()
        {
            // Navigate to the Invoices page and verify the title
            NavigateToPageAndCheckTitle("Invoices", "Invoices");

            // Verify the sum of the invoices
            decimal expectedSum = 963.97m;
            decimal actualSum = GetSumOfAllInvoices();
            Assert.AreEqual(expectedSum, actualSum, "The sum of the invoices is incorrect.");

            // Verify the amount of a specific invoice
            decimal expectedAmount = 423.99m;
            decimal actualAmount = GetAmountOfSpecificInvoice("I634");
            Assert.AreEqual(expectedAmount, actualAmount, "The value of the invoice 'I634' is incorrect.");
        }

        [Test]
        public void TestNavigationToPrivacyAndHomePages()
        {
            // Navigate to Privacy page and verify the title
            NavigateToPageAndCheckTitle("Privacy", "Privacy Policy");

            // Navigate to Home page and verify the title
            NavigateToPageAndCheckTitle("Home", "Welcome");
        }
    }
}
