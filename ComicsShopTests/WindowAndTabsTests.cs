using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System.Linq;

namespace ComicsShopTests
{
    [TestFixture]
    public class WindowAndTabsTests : SeleniumTestsBase
    {
        [Test]
        public void GradingGuide_Link_ShouldOpenNewTab()
        {
            LoginAsAdmin();
            driver.Navigate().GoToUrl(BaseUrl);
            string originalTab = driver.CurrentWindowHandle;

            var guideBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btn-open-guide")));
            ScrollAndClick(guideBtn);

            wait.Until(d => d.WindowHandles.Count > 1);

            var newTab = driver.WindowHandles.First(handle => handle != originalTab);
            driver.SwitchTo().Window(newTab);

            var guideTitle = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("guide-title")));
            Assert.That(guideTitle.Displayed, Is.True);

            Assert.That(driver.Url, Does.Contain("GradingGuide"));

            var closeBtn = driver.FindElement(By.Id("btn-close-guide"));
            ScrollAndClick(closeBtn);
            driver.SwitchTo().Window(originalTab);

            Assert.That(driver.Title, Does.Contain("Comics Shop"));
        }
    }
}