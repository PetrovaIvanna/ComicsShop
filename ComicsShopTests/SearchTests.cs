using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ComicsShopTests
{
    [TestFixture]
    public class SearchTests : SeleniumTestsBase
    {
        [Test]
        public void Search_ExistingComic_ShouldFindResult_RegardlessOfCase()
        {
            driver.Manage().Cookies.DeleteAllCookies();
            driver.Navigate().GoToUrl($"{BaseUrl}/ComicBooks");

            var searchInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Name("searchString")));
            searchInput.Clear();
            searchInput.SendKeys("batman");

            var searchBtn = driver.FindElement(By.XPath("//input[@name='searchString']/following-sibling::button[@type='submit']"));
            ScrollAndClick(searchBtn);

            wait.Until(d => d.Url.Contains("searchString="));

            bool isFound = driver.PageSource.Contains("Batman");
            Assert.That(isFound, Is.True);
        }

        [Test]
        public void Search_Comics_WhenNotFound_ShouldShowNoResultsMessage()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/ComicBooks");

            var searchInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Name("searchString")));
            searchInput.SendKeys("AbsolutleyNonExistentComic12345");

            var searchBtn = driver.FindElement(By.XPath("//input[@name='searchString']/following-sibling::button[@type='submit']"));
            ScrollAndClick(searchBtn);

            wait.Until(d => d.Url.Contains("searchString="));

            bool messageFound = driver.PageSource.Contains("No comics found matching your criteria");
            Assert.That(messageFound, Is.True);
        }

        [Test]
        public void Search_ExistingPublisher_ById_ShouldFindResult()
        {
            LoginAsAdmin();
            driver.Navigate().GoToUrl($"{BaseUrl}/Publishers");

            var searchInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Name("searchId")));
            searchInput.Clear();
            searchInput.SendKeys("1");

            var searchBtn = driver.FindElement(By.XPath("//input[@name='searchId']/following-sibling::button[@type='submit']"));
            ScrollAndClick(searchBtn);

            wait.Until(d => d.Url.Contains("searchId=1"));

            Assert.That(driver.PageSource, Does.Contain("Marvel Comics"));
            Assert.That(driver.PageSource, Does.Not.Contain("DC Comics"));
        }

        [Test]
        public void Search_Publishers_WhenNotFound_ShouldShowMessage()
        {
            LoginAsAdmin();
            driver.Navigate().GoToUrl($"{BaseUrl}/Publishers");

            var searchInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Name("searchId")));
            searchInput.Clear();
            searchInput.SendKeys("9999");

            var searchBtn = driver.FindElement(By.XPath("//input[@name='searchId']/following-sibling::button[@type='submit']"));
            ScrollAndClick(searchBtn);

            wait.Until(d => d.Url.Contains("searchId=9999"));

            var errorAlert = wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("alert-danger")));

            Assert.That(errorAlert.Text, Does.Contain("not found"));
            Assert.That(errorAlert.Text, Does.Contain("9999"));
        }
    }
}
