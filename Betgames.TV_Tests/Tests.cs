using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Frameworks;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace Betgames.TV_Tests
{
    [TestClass]
    public class Tests
    {
        private WebDriver WebDriver { get; set; }
        private string DriverPath { get; set; } = @"C:\Users\villi\Downloads\edgedriver_win64";
        private string BaseUrl { get; set; } = "https://demo.betgames.tv";
        private string LobbyUrl { get; set; } = "https://demo-webiframe.betgames.tv/auth?apiUrl=https%3A%2F%2Fbetgames9.betgames.tv&partnerUrl=https%3A%2F%2Fdemo.betgames.tv%2F&partnerCode=testpartner&token=bgtv-demo-token-3123&locale=en&timezone=0&defaultPage=lobby&newIntegration=1&scriptIntegration=1";

        [TestInitialize]
        public void EdgeDriverInitialize()
        {
            WebDriver = GetEdgeDriver();
            WebDriver.Manage().Window.Maximize();
        }

        [TestMethod]
        public void ContactUsTest()
        {
            WebDriver.Navigate().GoToUrl(BaseUrl);

            //Enter message
            var message = WebDriver.FindElement(By.Id("message"));
            message.Clear();
            message.SendKeys("Hello, I like your website.");

            //Enter email
            var email = WebDriver.FindElement(By.Id("email"));
            email.Clear();
            email.SendKeys("myemail@gmail.com");

            //Click send
            var sendButton = WebDriver.FindElement(By.CssSelector("button.send.btn.btn-primary.pull-left"));
            sendButton.Click();
            Thread.Sleep(3000);

            //Verify success message
            var sentMessage = WebDriver.FindElement(By.Id("send"));
            Assert.IsTrue(sentMessage.Displayed);
            Assert.AreEqual("Your message is sent.", sentMessage.Text);
        }

        [TestMethod]
        public void SkywardTest()
        {
            WebDriver.Navigate().GoToUrl(LobbyUrl);
            WebDriverWait wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(30));

            //Open game video
            var gameVideo = wait.Until(drv => drv.FindElement(By.CssSelector("a[href='/game/27']")));
            gameVideo.Click();

            //Unmute video
            var soundButton = wait.Until(drv => drv.FindElement(By.CssSelector("button[data-qa='button-toggle-sound']")));
            //soundButton = WebDriver.FindElement(By.CssSelector("button[data-qa='button-toggle-sound']"));
            soundButton.Click();
            Thread.Sleep(2000);

            //Turn off music
            var settings = WebDriver.FindElement(By.Id("top-navigation-settings"));
            settings.Click();
            var music = WebDriver.FindElement(By.CssSelector("div[data-qa='area-toggle-background-music-list-item']"));
            _ = music.Selected;
            music.Click();

            //Turn off animation and close settings
            var animation = WebDriver.FindElement(By.CssSelector("div[data-qa='area-toggle-animation-list-item']"));
            _ = animation.Selected;
            animation.Click();
            var close = WebDriver.FindElement(By.CssSelector("button[data-qa='button-modal-close']"));
            close.Click();

            //Place auto bet
            var autoPlay = WebDriver.FindElement(By.CssSelector("button[class='b_aG7bzo5aM2mIR7O3BA BaAj1j8NoIHk4C0kYMLs QdM0zIFg42K1J36rsnw1']"));
            autoPlay.Click();
            var tenRounds = WebDriver.FindElement(By.CssSelector("button[class='b_aG7bzo5aM2mIR7O3BA Z8ppCFe3k22L3jP8FRHW QdM0zIFg42K1J36rsnw1']"));
            tenRounds.Click();
            var start = WebDriver.FindElement(By.CssSelector("button[class='gOv3x_JAVQG3N7KoLalc QdM0zIFg42K1J36rsnw1']"));
            start.Click();
            Thread.Sleep(2000);
            var stop = wait.Until(drv => drv.FindElement(By.CssSelector("button[class='b_aG7bzo5aM2mIR7O3BA BaAj1j8NoIHk4C0kYMLs QdM0zIFg42K1J36rsnw1']")));
            stop.Click();
            Thread.Sleep(3000);
        }

        [TestCleanup]
        public void EdgeDriverCleanup()
        {
            WebDriver.Quit();
        }

        private WebDriver GetEdgeDriver()
        {
            var options = new EdgeOptions();

            return new EdgeDriver(DriverPath, options, TimeSpan.FromSeconds(300));
        }
    }
}
