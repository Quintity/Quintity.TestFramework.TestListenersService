using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.PhantomJS;

// Requires reference to WebDriver.Support.dll
using OpenQA.Selenium.Support.UI;

using QTF = Quintity.TestFramework.Core;

namespace Quintity.TestFramework.Scratch.TestSuites
{
     [QTF.TestClass()]
    public class Selenium : ScratchBase
    {
        [QTF.TestMethod]
        public QTF.TestVerdict SeleniumTest(int i)
        {
            try
            {
                Setup();

                //QTF.TestCheck.IsTrue
                ///QTF.TestWarning.


                // Create a new instance of the Firefox driver.

                // Notice that the remainder of the code relies on the interface, 
                // not the implementation.

                // Further note that other drivers (InternetExplorerDriver,
                // ChromeDriver, etc.) will require further configuration 
                // before this example will work. See the wiki pages for the
                // individual drivers at http://code.google.com/p/selenium/wiki
                // for further information.
                //IWebDriver driver = new FirefoxDriver();
                //IWebDriver driver = new InternetExplorerDriver();
                IWebDriver driver = new PhantomJSDriver();

                //Notice navigation is slightly different than the Java version
                //This is because 'get' is a keyword in C#
                using (var metric = new QTF.TestMetric("12345", true, "Navigate to Google."))
                {
                    driver.Navigate().GoToUrl("http://www.google.com/");
                }

                // Find the text input element by its name
                IWebElement query = driver.FindElement(By.Name("q"));

                // Enter something to search for
                query.SendKeys("Cheese");

                // Now submit the form. WebDriver will find the form for us from the element
                using (var metric = new QTF.TestMetric("67890", true, "Query for cheese."))
                {
                    query.Submit();

                    // Google's search is rendered dynamically with JavaScript.
                    // Wait for the page to load, timeout after 10 seconds
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    wait.Until((d) => { return d.Title.ToLower().StartsWith("cheese"); });
                }

                Thread.Sleep(10000);

                // Should see: "Cheese - Google Search"
                TestMessage = "Page title is: " + driver.Title;
                //System.Console.WriteLine("Page title is: " + driver.Title);

                //Close the browser
                driver.Quit();


                TestVerdict = QTF.TestVerdict.Pass;
            }
            catch (Exception e)
            {
                TestMessage += e.ToString();
                TestVerdict = QTF.TestVerdict.Error;
            }
            finally
            {
                Teardown();
            }

            return TestVerdict;
        }
    }
}
