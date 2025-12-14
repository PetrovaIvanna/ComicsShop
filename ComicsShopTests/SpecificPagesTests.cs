using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace ComicsShopTests
{
    [TestFixture]
    public class SpecificPagesTests : SeleniumTestsBase
    {
        [Test]
        public void UserHome_HeroSection_ShouldDisplayPromotionalTextAndIcons()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/Identity/Account/Logout");
            try { driver.FindElement(By.XPath("//button[text()='Logout']")).Click(); } catch { }

            driver.Navigate().GoToUrl(BaseUrl);

            var heroTitle = wait.Until(ExpectedConditions.ElementIsVisible(By.TagName("h1")));
            Assert.That(heroTitle.Text, Does.Contain("Welcome to Comics Shop"));

            var truckIcon = driver.FindElement(By.CssSelector(".bi-truck"));
            Assert.That(truckIcon.Displayed, Is.True);
        }

        [Test]
        public void AdminHome_DashboardCards_ShouldBeVisible()
        {
            LoginAsAdmin();
            driver.Navigate().GoToUrl(BaseUrl);

            var primaryCard = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".card.bg-primary")));
            var successCard = driver.FindElement(By.CssSelector(".card.bg-success"));

            Assert.That(primaryCard.Displayed, Is.True);
            Assert.That(successCard.Displayed, Is.True);
        }

        [Test]
        public void ShoppingCart_TableStructure_ShouldHaveCorrectHeaders()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/ShoppingCart");

            wait.Until(ExpectedConditions.ElementExists(By.TagName("table")));
            var headers = driver.FindElements(By.CssSelector("thead th"));

            Assert.That(headers[0].Text, Is.EqualTo("Amount"));
            Assert.That(headers[1].Text, Is.EqualTo("Comic"));
        }

        [Test]
        public void LoginPage_UI_ShouldContainEmailAndPassInputs()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/Identity/Account/Login");

            var emailInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Input_Email")));
            var passInput = driver.FindElement(By.Id("Input_Password"));

            Assert.That(emailInput.Displayed, Is.True);
            Assert.That(passInput.Displayed, Is.True);
        }
    }
}