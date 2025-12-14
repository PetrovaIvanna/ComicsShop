using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Interactions;
using System;

namespace ComicsShopTests
{
    public class SeleniumTestsBase
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;
        protected const string BaseUrl = "http://localhost:5152";

        [SetUp]
        public void Setup()
        {
            var options = new EdgeOptions();
            driver = new EdgeDriver(options);
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [TearDown]
        public void Teardown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }

        protected void LoginAsAdmin()
        {
            Login("admin@gmail.com", "Admin123!");
        }

        protected void LoginAsUser()
        {
            Login("testuser@gmail.com", "User123!");
        }

        private void Login(string email, string password)
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/Identity/Account/Login");
            if (driver.Url == $"{BaseUrl}/") return;

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Input_Email")));

            driver.FindElement(By.Id("Input_Email")).Clear();
            driver.FindElement(By.Id("Input_Email")).SendKeys(email);
            
            driver.FindElement(By.Id("Input_Password")).Clear();
            driver.FindElement(By.Id("Input_Password")).SendKeys(password);

            var loginBtn = driver.FindElement(By.CssSelector("button[type='submit']"));
            ScrollAndClick(loginBtn);

            wait.Until(d => d.Url == $"{BaseUrl}/" || !d.Url.Contains("/Login"));
        }

        protected string GenerateUniqueName(string baseName)
        {
            return $"{baseName}_{DateTime.Now.Ticks}";
        }

        protected void ScrollAndClick(IWebElement element)
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(element));

            Actions actions = new Actions(driver);

            actions.MoveToElement(element)
                   .Click()
                   .Perform();
        }
    }
}