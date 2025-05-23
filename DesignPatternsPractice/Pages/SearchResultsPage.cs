using OpenQA.Selenium;

namespace SeleniumTests.Pages
{
    public class SearchResultsPage : BasePage
    {
        public SearchResultsPage(IWebDriver driver) : base(driver) { }

        public string ResultCountText =>
            Driver.FindElement(By.ClassName("search-filter__result-count")).Text;
    }
}
