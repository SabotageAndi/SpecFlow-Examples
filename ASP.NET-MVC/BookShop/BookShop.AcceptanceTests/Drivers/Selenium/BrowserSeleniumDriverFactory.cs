﻿using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using TechTalk.SpecRun;
using ChromeDriver = TestProject.OpenSDK.Drivers.Web.ChromeDriver;
using EdgeDriver = TestProject.OpenSDK.Drivers.Web.EdgeDriver;
using FirefoxDriver = TestProject.OpenSDK.Drivers.Web.FirefoxDriver;
using InternetExplorerDriver = TestProject.OpenSDK.Drivers.Web.InternetExplorerDriver;

namespace BookShop.AcceptanceTests.Drivers.Selenium
{
    public class BrowserSeleniumDriverFactory
    {
        private readonly TestRunContext _testRunContext;
        private readonly WebServerDriver _webServerDriver;

        public BrowserSeleniumDriverFactory(TestRunContext testRunContext, WebServerDriver webServerDriver)
        {
            _testRunContext = testRunContext;
            _webServerDriver = webServerDriver;
        }

        public IWebDriver GetForBrowser(string browserId)
        {
            string lowerBrowserId = browserId.ToUpper();
            switch (lowerBrowserId)
            {
                case "IE": return GetInternetExplorerDriver();
                case "CHROME": return GetChromeDriver(false);
                case "CHROME-HEADLESS": return GetChromeDriver(true);
                case "FIREFOX": return GetFirefoxDriver();
                case "EDGE": return GetEdgeDriver();
                case string browser: throw new NotSupportedException($"{browser} is not a supported browser");
                default: throw new NotSupportedException("not supported browser: <null>");
            }
        }

        private IWebDriver GetEdgeDriver()
        {
            return new EdgeDriver()
            {
                Url = _webServerDriver.Hostname
            };
        }

        private IWebDriver GetFirefoxDriver()
        {
            var firefoxDriverService = FirefoxDriverService.CreateDefaultService(_testRunContext.TestDirectory);
            return new FirefoxDriver()
            {
                Url = _webServerDriver.Hostname,
            };
        }

        private IWebDriver GetChromeDriver(bool isHeadless)
        {
            var chromeDriverService = ChromeDriverService.CreateDefaultService(_testRunContext.TestDirectory);

            var chromeOptions = new ChromeOptions();

            if (isHeadless)
            {
                chromeOptions.AddArgument("headless");
            }

            var chromeDriver = new ChromeDriver(chromeOptions: chromeOptions)
            {
                Url = _webServerDriver.Hostname
            };

            return chromeDriver;
        }

        private IWebDriver GetInternetExplorerDriver()
        {
            var internetExplorerOptions = new InternetExplorerOptions
            {
                IgnoreZoomLevel = true,


            };
            return new InternetExplorerDriver(internetExplorerOptions: internetExplorerOptions)
            {
                Url = _webServerDriver.Hostname,


            };
        }
    }
}