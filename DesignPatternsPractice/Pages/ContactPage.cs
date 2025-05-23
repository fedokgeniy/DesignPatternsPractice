using OpenQA.Selenium;

namespace SeleniumTests.Pages
{
    public class ContactPage : BasePage
    {
        public ContactPage(IWebDriver driver) : base(driver) { }

        public string GetEmail() =>
            Driver.FindElement(By.XPath("//a[contains(text(), 'franciskscarynacr@gmail.com')]")).Text;

        public string GetPhoneLT() =>
            Driver.FindElement(By.XPath("//strong[contains(text(), 'LT):')]/parent::li")).Text.Trim();

        public string GetPhoneBY() =>
            Driver.FindElement(By.XPath("//strong[contains(text(), 'Phone (BY')]/parent::li")).Text.Trim();

        public bool HasSocialLink(string network) =>
            Driver.FindElement(By.XPath($"//a[contains(text(), '{network}')]")) != null;
    }
}
