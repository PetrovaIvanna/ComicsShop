using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ComicsShopTests
{
    [TestFixture]
    public class ElementsStateTests : SeleniumTestsBase
    {
        [Test]
        public void CreateForm_Inputs_ShouldAcceptTextAndToggleCheckboxes()
        {
            LoginAsAdmin();
            driver.Navigate().GoToUrl($"{BaseUrl}/ComicBooks/Create");

            var titleInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("input-title")));
            titleInput.SendKeys("UI Test Comic");
            Assert.That(titleInput.GetAttribute("value"), Is.EqualTo("UI Test Comic"));

            var rareCheckbox = driver.FindElement(By.Id("check-rare"));
            bool initialState = rareCheckbox.Selected;
            rareCheckbox.Click();

            Assert.That(rareCheckbox.Selected, Is.Not.EqualTo(initialState));

            var radioUsed = driver.FindElement(By.Id("radio-used"));
            radioUsed.Click();
            Assert.That(radioUsed.Selected, Is.True);
        }

        [Test]
        public void EditForm_DiscountToggle_ShouldShowHideInputs()
        {
            LoginAsAdmin();
            driver.Navigate().GoToUrl($"{BaseUrl}/ComicBooks");

            var editBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a.btn-warning")));
            editBtn.Click();

            var discountCheckbox = wait.Until(ExpectedConditions.ElementExists(By.Id("chk-enable-discount")));

            if (!discountCheckbox.Selected)
            {
                discountCheckbox.Click();

                var container = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("discount-container")));
                Assert.That(container.Displayed, Is.True);
            }
            else
            {
                discountCheckbox.Click();
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("discount-container")));
            }
        }

        [Test]
        public void Dropdown_ShouldExpandAndAllowSelection()
        {
            LoginAsAdmin();
            driver.Navigate().GoToUrl($"{BaseUrl}/ComicBooks/Create");

            var selectElem = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("select-publisher")));
            var select = new SelectElement(selectElem);

            if (select.Options.Count > 1)
            {
                select.SelectByIndex(1);
                Assert.That(select.SelectedOption.GetAttribute("value"), Is.Not.Empty);
            }
        }
    }
}