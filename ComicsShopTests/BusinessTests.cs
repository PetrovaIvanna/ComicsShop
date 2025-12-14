using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ComicsShopTests
{
    [TestFixture]
    public class BusinessTests : SeleniumTestsBase
    {
        [Test]
        public void CreatePublisher_FromBannedCountry_ShouldShowSanctionsError()
        {
            LoginAsAdmin();
            driver.Navigate().GoToUrl($"{BaseUrl}/Publishers/Create");

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("pub-name"))).SendKeys("Banned Publisher");
            driver.FindElement(By.Id("pub-year")).SendKeys("2020");

            var countryInput = driver.FindElement(By.Id("pub-country"));
            countryInput.SendKeys("Russia");

            var saveBtn = driver.FindElement(By.Id("btn-save-pub"));
            ScrollAndClick(saveBtn);

            Assert.That(driver.Url, Does.Contain("/Create"));

            var errorSpan = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("span[data-valmsg-for='Country']")));
            Assert.That(errorSpan.Text, Does.Contain("banned").Or.Contain("sanctions"));
        }

        [Test]
        public void Cart_AddSameItemTwice_ShouldIncreaseQuantity()
        {
            driver.Manage().Cookies.DeleteAllCookies();
            driver.Navigate().GoToUrl($"{BaseUrl}/ComicBooks");

            var addToCartBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Add to Cart")));
            ScrollAndClick(addToCartBtn);

            wait.Until(d => d.Url.Contains("ShoppingCart"));
            driver.Navigate().Back();

            wait.Until(d => d.Url.Contains("ComicBooks"));
            var addToCartBtn2 = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Add to Cart")));
            ScrollAndClick(addToCartBtn2);

            wait.Until(d => d.Url.Contains("ShoppingCart"));

            var amountCell = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("tbody tr td:first-child")));
            Assert.That(amountCell.Text, Is.EqualTo("2"));
        }

        [Test]
        public void Cart_RemoveItem_ShouldDeleteItemFromList()
        {
            driver.Manage().Cookies.DeleteAllCookies();
            driver.Navigate().GoToUrl($"{BaseUrl}/ComicBooks");

            var addBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Add to Cart")));
            ScrollAndClick(addBtn);

            wait.Until(d => d.Url.Contains("ShoppingCart"));

            var rowsBefore = driver.FindElements(By.CssSelector("tbody tr"));
            Assert.That(rowsBefore.Count, Is.GreaterThan(0));

            var removeBtn = driver.FindElement(By.LinkText("Remove"));
            ScrollAndClick(removeBtn);

            wait.Until(ExpectedConditions.StalenessOf(removeBtn));

            var rowsAfter = driver.FindElements(By.CssSelector("tbody tr"));
            Assert.That(rowsAfter.Count, Is.EqualTo(0));
        }

        [Test]
        public void Edit_Publisher_ShouldUpdateNameInList()
        {
            LoginAsAdmin();
            driver.Navigate().GoToUrl($"{BaseUrl}/Publishers/Create");

            string oldName = GenerateUniqueName("OldNamePub");
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("pub-name"))).SendKeys(oldName);
            driver.FindElement(By.Id("pub-country")).SendKeys("USA");
            driver.FindElement(By.Id("pub-year")).SendKeys("2000");
            ScrollAndClick(driver.FindElement(By.Id("btn-save-pub")));

            wait.Until(d => d.Url.Contains("/Publishers") && !d.Url.Contains("Create"));

            var editBtn = driver.FindElement(By.XPath($"//tr[contains(., '{oldName}')]//a[contains(@class, 'btn-warning')]"));
            ScrollAndClick(editBtn);

            string newName = GenerateUniqueName("NewNamePub");
            var nameInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("edit-pub-name")));
            nameInput.Clear();
            nameInput.SendKeys(newName);

            ScrollAndClick(driver.FindElement(By.Id("btn-save-changes")));

            wait.Until(d => d.Url.Contains("/Publishers") && !d.Url.Contains("Edit"));

            Assert.That(driver.PageSource, Does.Contain(newName));
            Assert.That(driver.PageSource, Does.Not.Contain(oldName));
        }
    }
}