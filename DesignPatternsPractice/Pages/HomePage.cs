using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;

namespace SeleniumTests.Pages
{
    public class HomePage : BasePage
    {
        public HomePage(IWebDriver driver) : base(driver) { }

        public void GoTo() => Driver.Navigate().GoToUrl("https://en.ehu.lt/");

        public void ClickAboutLink() =>
            Driver.FindElement(By.LinkText("About")).Click();

        public void OpenSearch() =>
            new Actions(Driver).MoveToElement(Driver.FindElement(By.ClassName("header-search"))).Perform();

        public void TypeSearchQuery(string query)
        {
            IWebElement searchBox = Driver.FindElement(By.Name("s"));
            searchBox.SendKeys(query + Keys.Enter);
        }

        public void ChangeLanguageToLT()
        {
            new Actions(Driver).MoveToElement(Driver.FindElement(By.ClassName("language-switcher"))).Perform();
            Driver.FindElement(By.XPath("//a[@href='https://lt.ehu.lt/']")).Click();
        }
    }
}
