using OpenQA.Selenium;

namespace SeleniumTests.Pages
{
    public class AboutPage : BasePage
    {
        public AboutPage(IWebDriver driver) : base(driver) { }

        public string GetTitle() => Driver.Title;
        public string GetHeading() => Driver.FindElement(By.TagName("h1")).Text;
    }
}
