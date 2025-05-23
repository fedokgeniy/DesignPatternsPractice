using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace SeleniumTests.Utils
{
    public class WebDriverSingleton
    {
        private static IWebDriver instance;
        private static readonly object lockObj = new();

        private WebDriverSingleton() { }

        public static IWebDriver Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        var options = new ChromeOptions();
                        options.BinaryLocation = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
                        instance = new ChromeDriver(options);
                        instance.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                        instance.Manage().Window.Maximize();
                    }
                    return instance;
                }
            }
        }

        public static void Quit()
        {
            if (instance != null)
            {
                instance.Quit();
                instance = null;
            }
        }
    }
}
