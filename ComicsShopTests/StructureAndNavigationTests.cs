using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace ComicsShopTests
{
    [TestFixture]
    public class StructureAndNavigationTests : SeleniumTestsBase
    {
        [Test]
        public void Header_NavigationLinks_ShouldBeVisibleAndClickable()
        {
            driver.Navigate().GoToUrl(BaseUrl);

            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("navbar")));

            var comicsLink = driver.FindElement(By.LinkText("Comics"));
            comicsLink.Click();

            wait.Until(ExpectedConditions.UrlContains("ComicBooks"));

            var heading = wait.Until(ExpectedConditions.ElementIsVisible(By.TagName("h2")));
            Assert.That(heading.Text, Does.Contain("Comics Catalogue"));
        }

        [TestCase("")]
        [TestCase("ComicBooks")]
        [TestCase("Identity/Account/Login")]
        public void Footer_ShouldBeVisible_On_VariousPages(string relativeUrl)
        {
            var url = string.IsNullOrEmpty(relativeUrl) ? BaseUrl : $"{BaseUrl}/{relativeUrl}";

            driver.Navigate().GoToUrl(url);

            var footer = wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("footer")));

            Assert.That(footer.Text, Does.Contain("2025 - ComicsShop"));
        }

        [Test]
        public void Guest_ShouldSee_ShoppingCartLink()
        {
            driver.Manage().Cookies.DeleteAllCookies();
            driver.Navigate().GoToUrl(BaseUrl);

            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("navbar")));

            var cartLinks = driver.FindElements(By.CssSelector("a[href*='ShoppingCart']"));

            Assert.That(cartLinks.Count, Is.GreaterThan(0));
            Assert.That(cartLinks[0].Displayed, Is.True);
        }

        [Test]
        public void Admin_ShouldNotSee_ShoppingCartLink()
        {
            LoginAsAdmin();
            driver.Navigate().GoToUrl(BaseUrl);

            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("navbar")));

            var cartLinks = driver.FindElements(By.CssSelector("a[href*='ShoppingCart']"));

            Assert.That(cartLinks.Count, Is.EqualTo(0));
        }
    }
}