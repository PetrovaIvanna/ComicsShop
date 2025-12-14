using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace ComicsShopTests
{
    [TestFixture]
    public class ShoppingFlowTests : SeleniumTestsBase
    {
        [Test]
        public void Full_Shopping_Process_ShouldShowSuccessMessage()
        {
            driver.Navigate().GoToUrl(BaseUrl);

            var shopBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Shop Now")));
            ScrollAndClick(shopBtn);

            var addToCartBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Add to Cart")));
            ScrollAndClick(addToCartBtn);

            wait.Until(ExpectedConditions.UrlContains("ShoppingCart"));

            var checkoutBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Check out now!")));
            ScrollAndClick(checkoutBtn);

            if (driver.Url.Contains("Login"))
            {
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Input_Email"))).SendKeys("testuser@gmail.com");
                driver.FindElement(By.Id("Input_Password")).SendKeys("User123!");

                var loginBtn = driver.FindElement(By.CssSelector("button[type='submit']"));
                ScrollAndClick(loginBtn);
            }

            wait.Until(ExpectedConditions.UrlContains("Checkout"));

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("FirstName"))).SendKeys("Ivanna");
            driver.FindElement(By.Id("LastName")).SendKeys("Petrova");
            driver.FindElement(By.Id("AddressLine1")).SendKeys("Street 1");
            driver.FindElement(By.Id("PhoneNumber")).SendKeys("1234567890");

            var completeBtn = driver.FindElement(By.XPath("//button[text()='Complete Order']"));
            ScrollAndClick(completeBtn);

            var successMsg = wait.Until(ExpectedConditions.ElementIsVisible(By.TagName("h1")));
            Assert.That(successMsg.Text, Is.EqualTo("Thanks for your order. You'll soon get your comics!"));
        }
    }
}