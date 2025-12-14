using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace ComicsShopTests
{
    [TestFixture]
    public class FeedbackAndValidationTests : SeleniumTestsBase
    {
        [Test]
        public void Validation_RequiredFields_ShouldHighlightOrShowMessage()
        {
            LoginAsAdmin();
            driver.Navigate().GoToUrl($"{BaseUrl}/Publishers/Create");

            var saveBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btn-save-pub")));
            ScrollAndClick(saveBtn);

            var errorSpan = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("span[data-valmsg-for='Name']")));
            Assert.That(errorSpan.Text, Is.Not.Empty);
        }

        [Test]
        public void DeleteAction_ShouldTriggerBrowserAlert()
        {
            LoginAsAdmin();
            driver.Navigate().GoToUrl($"{BaseUrl}/Publishers");

            var deleteBtns = driver.FindElements(By.CssSelector("a.btn-danger"));
            if (deleteBtns.Count > 0)
            {
                ScrollAndClick(deleteBtns[0]);
                IAlert alert = wait.Until(ExpectedConditions.AlertIsPresent());
                Assert.That(alert.Text, Does.Contain("Delete"));
                alert.Dismiss();
            }
        }

        [Test]
        public void CreateAction_ShouldShowSuccessFlashMessage()
        {
            LoginAsAdmin();
            driver.Navigate().GoToUrl($"{BaseUrl}/Publishers/Create");

            string uniquePubName = GenerateUniqueName("UI_Flash_Pub");

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("pub-name"))).SendKeys(uniquePubName);
            driver.FindElement(By.Id("pub-country")).SendKeys("USA");
            driver.FindElement(By.Id("pub-year")).SendKeys("2025");

            var saveBtn = driver.FindElement(By.Id("btn-save-pub"));
            ScrollAndClick(saveBtn);

            var successAlert = wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("alert-success")));

            Assert.That(successAlert.Text, Does.Contain("Publisher created successfully"));
        }
    }
}