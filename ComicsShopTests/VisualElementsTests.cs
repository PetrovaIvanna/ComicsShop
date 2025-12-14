using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ComicsShopTests
{
    [TestFixture]
    public class VisualElementsTests : SeleniumTestsBase
    {

        [Test]
        public void ComicCard_ShouldDisplay_RareBadge_IfApplicable()
        {
            LoginAsAdmin();
            driver.Navigate().GoToUrl($"{BaseUrl}/ComicBooks/Create");

            string uniqueTitle = GenerateUniqueName("VisualTestRare");

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("input-title"))).SendKeys(uniqueTitle);
            driver.FindElement(By.Id("input-issue")).SendKeys("1");
            driver.FindElement(By.Id("input-price")).SendKeys("10");
            driver.FindElement(By.Id("input-date")).SendKeys("01012025");

            var qty = driver.FindElement(By.Name("QuantityInStock"));
            qty.Clear();
            qty.SendKeys("5");

            var radioNew = driver.FindElement(By.Id("radio-new"));
            ScrollAndClick(radioNew);

            var checkRare = driver.FindElement(By.Id("check-rare"));
            if (!checkRare.Selected) ScrollAndClick(checkRare);

            new SelectElement(driver.FindElement(By.Id("select-publisher"))).SelectByIndex(1);

            var submitBtn = driver.FindElement(By.Id("btn-submit"));
            ScrollAndClick(submitBtn);

            wait.Until(d => d.Url.Contains("/ComicBooks") && !d.Url.Contains("/Create"));

            var searchInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Name("searchString")));
            searchInput.SendKeys(uniqueTitle);

            var searchBtn = driver.FindElement(By.XPath("//input[@name='searchString']/following-sibling::button[@type='submit']"));
            ScrollAndClick(searchBtn);

            wait.Until(d => d.Url.Contains("searchString="));

            var row = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//tr[contains(., '{uniqueTitle}')]")));
            var rareBadge = row.FindElement(By.CssSelector(".badge.bg-warning"));

            Assert.That(rareBadge.Text, Is.EqualTo("Rare"));
        }

        [Test]
        public void SoldOut_Button_ShouldHave_DisabledStyle()
        {
            LoginAsAdmin();
            driver.Navigate().GoToUrl($"{BaseUrl}/ComicBooks/Create");

            string uniqueTitle = GenerateUniqueName("VisualTestSoldOut");

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("input-title"))).SendKeys(uniqueTitle);
            driver.FindElement(By.Id("input-issue")).SendKeys("1");
            driver.FindElement(By.Id("input-price")).SendKeys("10");
            driver.FindElement(By.Id("input-date")).SendKeys("01012025");

            var qty = driver.FindElement(By.Name("QuantityInStock"));
            qty.Clear();
            qty.SendKeys("0");

            var radioNew = driver.FindElement(By.Id("radio-new"));
            ScrollAndClick(radioNew);

            new SelectElement(driver.FindElement(By.Id("select-publisher"))).SelectByIndex(1);

            var submitBtn = driver.FindElement(By.Id("btn-submit"));
            ScrollAndClick(submitBtn);

            wait.Until(d => d.Url.Contains("/ComicBooks") && !d.Url.Contains("/Create"));

            driver.Manage().Cookies.DeleteAllCookies();
            driver.Navigate().GoToUrl($"{BaseUrl}/ComicBooks");

            wait.Until(ExpectedConditions.ElementIsVisible(By.LinkText("Login")));

            var searchInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Name("searchString")));
            searchInput.SendKeys(uniqueTitle);

            var searchBtn = driver.FindElement(By.XPath("//input[@name='searchString']/following-sibling::button[@type='submit']"));
            ScrollAndClick(searchBtn);

            wait.Until(d => d.Url.Contains("searchString="));

            var btn = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[contains(text(), 'Sold Out')]")));

            Assert.That(btn.GetAttribute("class"), Does.Contain("btn-secondary"));
            Assert.That(btn.Enabled, Is.False);
        }
    }
}