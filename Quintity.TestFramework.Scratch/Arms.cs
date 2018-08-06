using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using System.Runtime.Serialization;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;

// Requires reference to WebDriver.Support.dll
using OpenQA.Selenium.Support.UI;

using QTF = Quintity.TestFramework.Core;

namespace Quintity.TestFramework.Scratch.TestSuites
{
    [DataContract]
    public class Intake
    {
        [DataMember]
        public int SakIntake;

        public Intake()
        { }
    }

    [QTF.TestClass()]
    public class Arms : ScratchBase
    {
        [QTF.TestMethod]
        public QTF.TestVerdict CreateIntake()
        {
            try
            {
                Setup();

                var intakeSerializer = new DataContractSerializer(typeof(Intake));

                var url = new Uri("http://sgtst50app01.cognosante.hosted:8080/api/Intakes/Post");
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/xml";

                var intake = new Intake 
                {
                    SakIntake = 7,
                };

                using (var requestStream = request.GetRequestStream())
                {
                    intakeSerializer.WriteObject(requestStream, intake);
                }

                var response = (HttpWebResponse)request.GetResponse();
                var statusCode = response.StatusCode;

                TestVerdict = statusCode == HttpStatusCode.OK ? QTF.TestVerdict.Pass : QTF.TestVerdict.Error;

                TestMessage = response.StatusDescription;
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

        [QTF.TestMethod]
        public QTF.TestVerdict GetIntakes(
            [QTF.TestParameter("Content Format", "Specifies the return from the ARMS API", "XML")]
            string format)
        {
            //XDocument xmlResult = null;

            try
            {
                Setup();

                var client = new WebClient();
                string contentType = string.Format("application/{0}", format);
                client.Headers.Add(HttpRequestHeader.ContentType, contentType); // you also can use application/json content type 
                string response = client.DownloadString("http://sgtst50app01.cognosante.hosted:8080/api/Intakes/Get");
                //string response = client.DownloadString("http://sgtst50app01.cognosante.hosted:8080/api/Cases/Get");
                //string response = client.DownloadString("http://sgtst50app01.cognosante.hosted:8080/api/Cases/Get/101");

                if (format.ToUpper().Equals("XML"))
                {
                    var xdoc = XDocument.Parse(response);
                    response = xdoc.ToString();
                }
                //TestMessage = xmlResult.ToString();
                TestMessage = response;
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

        [QTF.TestMethod]
        public QTF.TestVerdict GetUserByPK(string pk)
        {
            XDocument xmlResult = null;

            try
            {
                Setup();

                string address = string.Format("http://sgtst50app01.cognosante.hosted:8080/api/Users/Get?id={0}", pk);

                var client = new WebClient();
                client.Headers.Add(HttpRequestHeader.ContentType, "application/xml"); // you also can use application/json content type 
                string response = client.DownloadString(address);
                xmlResult = XDocument.Parse(response);

                TestMessage = xmlResult.ToString();
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


        [QTF.TestMethod]
        public QTF.TestVerdict GetUsers()
        {
            XDocument xmlResult = null;

            try
            {
                Setup();

                var client = new WebClient();
                client.Headers.Add(HttpRequestHeader.ContentType, "application/xml"); // you also can use application/json content type 

                xmlResult = XDocument.Parse(client.DownloadString("http://sgtst50app01.cognosante.hosted:8080/api/Users/Get"));

                TestMessage = xmlResult.ToString();
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

        [QTF.TestMethod]
        public QTF.TestVerdict GetRoleCodes()
        {
            XDocument xmlResult = null;

            try
            {
                Setup();

                //var client = new WebClient {Credentials = new NetworkCredential("jmothershead", "Invalid!")}; 
                var client = new WebClient();
                client.Headers.Add(HttpRequestHeader.ContentType, "application/xml"); // you also can use application/json content type 

                xmlResult = XDocument.Parse(client.DownloadString("http://sgtst50app01.cognosante.hosted:8080/api/RoleCodes/Get"));

                TestMessage = xmlResult.ToString();
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
         [QTF.TestMethod]
         public QTF.TestVerdict GetProviders()
        {
            XDocument xmlResult = null;

            try
            {
                Setup();

                //var client = new WebClient {Credentials = new NetworkCredential("jmothershead", "Invalid!")}; 
                var client = new WebClient();
                client.Headers.Add(HttpRequestHeader.ContentType, "application/xml"); // you also can use application/json content type 

                xmlResult = XDocument.Parse(client.DownloadString("http://sgtst50app01.cognosante.hosted:8080/api/Providers/Get"));

                TestMessage = xmlResult.ToString();
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

         [QTF.TestMethod]
         public QTF.TestVerdict GetAllOpenCases()
         {
             XDocument xmlResult = null;

             try
             {
                 Setup();

                 //var client = new WebClient {Credentials = new NetworkCredential("jmothershead", "Invalid!")}; 
                 var client = new WebClient();
                 client.Headers.Add(HttpRequestHeader.ContentType, "application/xml"); // you also can use application/json content type 

                 xmlResult = XDocument.Parse(client.DownloadString("http://sgtst50app01.cognosante.hosted:8080/api/Cases/GetAllOpenCases"));

                 TestMessage = xmlResult.ToString();
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

        [QTF.TestMethod]
        public QTF.TestVerdict ArmsTest()
        {
            try
            {
                Setup();

                // Create a new instance of the Firefox driver.

                // Notice that the remainder of the code relies on the interface, 
                // not the implementation.

                // Further note that other drivers (InternetExplorerDriver,
                // ChromeDriver, etc.) will require further configuration 
                // before this example will work. See the wiki pages for the
                // individual drivers at http://code.google.com/p/selenium/wiki
                // for further information.
                IWebDriver driver = new FirefoxDriver();
                //IWebDriver driver = new InternetExplorerDriver();

                //Notice navigation is slightly different than the Java version
                //This is because 'get' is a keyword in C#
                driver.Navigate().GoToUrl("http://sguat50app01.cognosante.hosted/Arms/login.aspx");

                Thread.Sleep(10000);

                // Find the text input element by its name
                IWebElement userName = driver.FindElement(By.Id("txtUserName"));
                //userName.Clear();
                //userName.SendKeys("jmothershead");
                IWebElement password = driver.FindElement(By.Id("j_password"));
                password.Clear();
                IWebElement loginBtn = driver.FindElement(By.Id("LoginButton"));
                
                // Enter something to search for
                userName.SendKeys("jmothershead");
                password.SendKeys("");
                loginBtn.Submit();

                Thread.Sleep(10000);
                // Now submit the form. WebDriver will find the form for us from the element
                //userName.Submit();

                // Google's search is rendered dynamically with JavaScript.
                // Wait for the page to load, timeout after 10 seconds
                //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                //wait.Until((d) => { return d.Title.ToLower().StartsWith("cheese"); });

                //// Should see: "Cheese - Google Search"
                //TestMessage = "Page title is: " + driver.Title;
                ////System.Console.WriteLine("Page title is: " + driver.Title);

                //Close the browser
                //driver.Quit();


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