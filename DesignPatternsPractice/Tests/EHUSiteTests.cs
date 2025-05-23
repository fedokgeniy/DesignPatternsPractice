using System;
using System.IO;
using NLog;
using OpenQA.Selenium;
using SeleniumTests.Pages;
using SeleniumTests.Utils;
using Reqnroll;
using NUnit.Framework;
using System.Linq;

namespace DesignPatternsPractice.Tests
{
    [Binding]
    public class EHUSiteSteps
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private IWebDriver driver;
        private DateTime _startTime;
        private IDisposable _testNameScope;
        private IDisposable _testRunIdScope;
        private ContactPage contactPage;
        private HomePage homePage;
        private string _currentScenario;

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            Directory.CreateDirectory(Path.Combine(TestContext.CurrentContext.TestDirectory, "logs"));
            Directory.CreateDirectory(Path.Combine(TestContext.CurrentContext.TestDirectory, "logs/archives"));
            Directory.CreateDirectory(Path.Combine(TestContext.CurrentContext.TestDirectory, "Screenshots"));
            Logger.Info("Test session started");
        }

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            _startTime = DateTime.Now;
            _currentScenario = scenarioContext.ScenarioInfo.Title;
            string runId = Guid.NewGuid().ToString("N");

            _testNameScope = Logger.PushScopeProperty("TestName", _currentScenario);
            _testRunIdScope = Logger.PushScopeProperty("RunId", runId);

            Logger.Info($"Starting scenario {_currentScenario}");
            Logger.Debug($"Start time: {_startTime}");
            Logger.Debug($"Test ID: {runId}");
            Logger.Debug($"Test directory: {TestContext.CurrentContext.TestDirectory}");

            try
            {
                driver = WebDriverSingleton.Instance;
                Logger.Debug("WebDriver initialized successfully");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error initializing WebDriver");
                throw;
            }
        }

        [Given(@"the user is on the contacts page")]
        public void GivenUserOnContactPage()
        {
            Logger.Debug("Navigating to the contacts page");
            driver.Navigate().GoToUrl("https://en.ehu.lt/contact/");
            contactPage = new ContactPage(driver);
        }

        [When(@"the user views the contact information")]
        public void WhenUserViewsContactInfo()
        {
            // No action required
        }

        [Then(@"the email ""(.*)"" is displayed")]
        public void ThenEmailIsDisplayed(string expectedEmail)
        {
            Logger.Info("Checking displayed email address");
            string actualEmail = contactPage.GetEmail();
            Logger.Debug($"Actual email: {actualEmail}");
            Assert.That(actualEmail, Is.EqualTo(expectedEmail));
        }

        [Then(@"the LT phone ""(.*)"" is displayed")]
        public void ThenPhoneLTIsDisplayed(string expectedPhone)
        {
            Logger.Info("Checking LT phone number");
            string actualPhoneLT = contactPage.GetPhoneLT();
            Logger.Debug($"Actual LT phone: {actualPhoneLT}");
            Assert.That(actualPhoneLT, Is.EqualTo(expectedPhone));
        }

        [Then(@"the BY phone ""(.*)"" is displayed")]
        public void ThenPhoneBYIsDisplayed(string expectedPhone)
        {
            Logger.Info("Checking BY phone number");
            string actualPhoneBY = contactPage.GetPhoneBY();
            Logger.Debug($"Actual BY phone: {actualPhoneBY}");
            Assert.That(actualPhoneBY, Is.EqualTo(expectedPhone));
        }

        [Then(@"the following social links are present: (.*)")]
        public void ThenSocialLinksArePresent(string socials)
        {
            Logger.Info("Checking presence of social links");
            var socialList = socials.Split(',').Select(s => s.Trim());
            foreach (var social in socialList)
            {
                Logger.Debug($"Checking for link to {social}");
                bool hasLink = contactPage.HasSocialLink(social);
                if (!hasLink)
                    Logger.Warn($"Expected link to {social} not found, but test continues");
                Assert.That(hasLink, $"Missing link to {social}");
            }
        }

        [Given(@"the user is on the home page")]
        public void GivenUserOnHomePage()
        {
            Logger.Debug("Navigating to the home page");
            driver.Navigate().GoToUrl("https://en.ehu.lt/");
            homePage = new HomePage(driver);
        }

        [When(@"the user clicks the About link in the navigation")]
        public void WhenUserClicksAbout()
        {
            Logger.Info("Checking navigation to About page");
            Logger.Debug("Clicking the About navigation link");
            homePage.ClickAboutLink();
        }

        [Then(@"the URL contains ""(.*)""")]
        public void ThenUrlContains(string expected)
        {
            string currentUrl = driver.Url;
            Logger.Debug($"Current URL after navigation: {currentUrl}");
            if (!currentUrl.Contains(expected))
                Logger.Warn($"URL does not contain the expected path {expected}, redirect may not be working correctly");
            Assert.That(currentUrl, Does.Contain(expected), $"URL should contain {expected} after navigation");
        }

        [AfterScenario]
        public void AfterScenario(ScenarioContext scenarioContext)
        {
            TimeSpan executionTime = DateTime.Now - _startTime;
            var testOutcome = scenarioContext.TestError == null
                ? "Passed"
                : "Failed";

            if (testOutcome != "Passed")
            {
                try
                {
                    var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                    var fileName = $"{_currentScenario}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                    var filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Screenshots", fileName);

                    screenshot.SaveAsFile("ScreenshotImageFormat.Png");
                    Logger.Error($"Test failed. Screenshot saved: {filePath}");
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Failed to capture screenshot on error");
                }
            }

            if (testOutcome == "Passed")
                Logger.Info($"Test completed successfully. Execution time: {executionTime.TotalSeconds:F2} sec");
            else
                Logger.Error($"Test failed. Execution time: {executionTime.TotalSeconds:F2} sec");

            try
            {
                WebDriverSingleton.Quit();
                Logger.Debug("WebDriver closed successfully");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error closing WebDriver");
            }
            finally
            {
                _testNameScope?.Dispose();
                _testRunIdScope?.Dispose();
            }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            Logger.Info("Test session finished");
            LogManager.Flush();
        }
    }
}