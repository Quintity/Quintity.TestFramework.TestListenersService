using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Net;

using QTF = Quintity.TestFramework.Core;

namespace Quintity.TestFramework.Scratch.TestSuites
{
    [QTF.TestClass()]
    public class TargetProcess : ScratchBase
    {
        [QTF.TestMethod]
        public QTF.TestVerdict TargetProcessTest()
        {
            try
            {
                Setup();

                //var client = new WebClient { Credentials = new NetworkCredential("AZ\\jmothershead", "Anathem!!") };
                var client = new WebClient()
                {
                    Credentials = new NetworkCredential("jim.mothershead@cognosante.com", "Invalid!")
                };

                client.Headers.Add(HttpRequestHeader.ContentType, "application/xml"); 

                //var xmlResult = XDocument.Parse(client.DownloadString("https://quintity.tpondemand.com/api/v1/Context/?ids=222"));

                //var xmlResult = XDocument.Parse(client.DownloadString("https://quintity.tpondemand.com/api/v1/TestCases/"));

                var xmlResult = XDocument.Parse(client.DownloadString("https://targetprocess.cognosante.com/TargetProcess2/api/v1/TestCases"));

                File.WriteAllText(@"c:\temp\userstories.xml", xmlResult.ToString());

                //https://Targetprocess/api/v1/UserStories

                TestVerdict = QTF.TestVerdict.Pass;
                TestMessage = xmlResult.ToString();
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
        public QTF.TestVerdict GetTestCase()
        {
            try
            {
                Setup();

                var client = new WebClient()
                {
                    Credentials = new NetworkCredential("jim.mothershead@cognosante.com", "Invalid!")
                };

                client.Headers.Add(HttpRequestHeader.ContentType, "application/xml"); // you also can use application/json content type 

                //var xmlResult = XDocument.Parse(client.DownloadString("https://quintity.tpondemand.com/api/v1/Context/?ids=222"));

                //xmlResult = XDocument.Parse(client.DownloadString("https://quintity.tpondemand.com/api/v1/TestCases/?take=1000&acid=A46658CE6BB1613DBD74EF02B46E6514"));
                // https://targetprocess.cognosante.com/TargetProcess2/api/v1/TestCases/9285/TestSteps
                var xmlResult = XDocument.Parse(client.DownloadString("https://targetprocess.cognosante.com/TargetProcess2/api/v1/TestCases/9285/TestSteps"));



                File.WriteAllText(@"c:\temp\teststeps.xml", xmlResult.ToString());

                //https://Targetprocess/api/v1/UserStories

                TestVerdict = QTF.TestVerdict.Pass;
                TestMessage = xmlResult.ToString();
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
        public QTF.TestVerdict QuintityTargetProcessTest()
        {
            try
            {
                Setup();

                var client = new WebClient { Credentials = new NetworkCredential("jmothershead", "Invalid!") };
                client.Headers.Add(HttpRequestHeader.ContentType, "application/xml"); // you also can use application/json content type 

                var xmlResult = XDocument.Parse(client.DownloadString("https://quintity.tpondemand.com/api/v1/UserStories"));

                //xmlResult = XDocument.Parse(client.DownloadString("https://targetprocess.cognosante.com/TargetProcess2/api/v1/Userstories/?take=1000&acid=76E6466FCA9B3DB2D3D35BD16D342244"));

                File.WriteAllText(@"c:\temp\userstories.xml", xmlResult.ToString());

                //https://Targetprocess/api/v1/UserStories

                TestVerdict = QTF.TestVerdict.Pass;
                TestMessage = xmlResult.ToString();
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
        public QTF.TestVerdict QuintityCreateProject()
        {
            try
            {
                Setup();

                var client = new WebClient { Credentials = new NetworkCredential("jmothershead", "Invalid!") };
                client.Headers.Add(HttpRequestHeader.ContentType, "application/xml"); // you also can use application/json content type 

                string address = "https://quintity.tpondemand.com/api/v1";

                WebRequest request = (HttpWebRequest)WebRequest.Create(address + "/Projects");
                request.Credentials = new NetworkCredential("jmothershead", "Invalid!");
                request.Method = WebRequestMethods.Http.Post;
                request.ContentType = "application/xml";

                string postData = "<Project Name=\"My spanking new project\"/>";
                request.ContentLength = postData.Length;

                StreamWriter postStream = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                postStream.Write(postData);
                postStream.Close();

                WebResponse response = (HttpWebResponse)request.GetResponse();

                //var xmlResult = XDocument.Parse(client.DownloadString(address + "/Projects"));

                //File.WriteAllText(@"c:\temp\userstories.xml", xmlResult.ToString());

                TestVerdict = QTF.TestVerdict.Pass;
                //TestMessage = xmlResult.ToString();
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
        public QTF.TestVerdict GetProjects()
        {
            try
            {
                Setup();

                var client = new WebClient { Credentials = new NetworkCredential("jmothershead", "Invalid!") };
                client.Headers.Add(HttpRequestHeader.ContentType, "application/xml"); // you also can use application/json content type 

                var xmlResult = XDocument.Parse(client.DownloadString("https://quintity.tpondemand.com/api/v1/Projects"));

                File.WriteAllText(@"c:\temp\userstories.xml", xmlResult.ToString());

                TestVerdict = QTF.TestVerdict.Pass;
                TestMessage = xmlResult.ToString();
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

        //[QTF.TestMethod]
        //public QTF.TestVerdict GetUserStories(string uri)
        //{
        //    try
        //    {
        //        Setup();

        //        var client = new WebClient { Credentials = new NetworkCredential("jmothershead", "Invalid!") };
        //        client.Headers.Add(HttpRequestHeader.ContentType, "application/xml"); // you also can use application/json content type 

        //        var xmlResult = XDocument.Parse(client.DownloadString("https://quintity.tpondemand.com/api/v1/Projects"));

        //        File.WriteAllText(@"c:\temp\userstories.xml", xmlResult.ToString());

        //        TestVerdict = QTF.TestVerdict.Pass;
        //        TestMessage = xmlResult.ToString();
        //    }
        //    catch (Exception e)
        //    {
        //        TestMessage += e.ToString();
        //        TestVerdict = QTF.TestVerdict.Error;
        //    }
        //    finally
        //    {
        //        Teardown();
        //    }

        //    return TestVerdict;
        //}

        [QTF.TestMethod]
        public QTF.TestVerdict QuintityCreateUserStory()
        {
            try
            {
                Setup();

                var client = new WebClient { Credentials = new NetworkCredential("jmothershead", "Invalid!") };
                client.Headers.Add(HttpRequestHeader.ContentType, "application/xml"); // you also can use application/json content type 

                string address = "https://quintity.tpondemand.com/api/v1";

                WebRequest request = (HttpWebRequest)WebRequest.Create(address + "/UserStories");
                request.Credentials = new NetworkCredential("jmothershead", "Invalid!");
                request.Method = WebRequestMethods.Http.Post;
                request.ContentType = "application/xml";

                //string postData = "<Project Name=\"My spanking new project\"/>";
                //string postData = "<UserStory Name=\"My new story\" Project=\"My spanking new project\"/>";
                string postData = "<UserStory Name=\"My new story\"><Project Id=\"214\"/></UserStory>";
                request.ContentLength = postData.Length;

                StreamWriter postStream = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                postStream.Write(postData);
                postStream.Close();

                WebResponse response = (HttpWebResponse)request.GetResponse();

                //var xmlResult = XDocument.Parse(client.DownloadString(address + "/Projects"));

                //File.WriteAllText(@"c:\temp\userstories.xml", xmlResult.ToString());

                TestVerdict = QTF.TestVerdict.Pass;
                //TestMessage = xmlResult.ToString();
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
        public QTF.TestVerdict QuintityCreateTestCase()
        {
            try
            {
                Setup();

                var client = new WebClient { Credentials = new NetworkCredential("jmothershead", "Invalid!") };
                client.Headers.Add(HttpRequestHeader.ContentType, "application/xml"); // you also can use application/json content type 

                string address = "https://quintity.tpondemand.com/api/v1";

                WebRequest request = (HttpWebRequest)WebRequest.Create(address + "/TestCases");
                request.Credentials = new NetworkCredential("jmothershead", "Invalid!");
                request.Method = WebRequestMethods.Http.Post;
                request.ContentType = "application/xml";

                //string postData = "<Project Name=\"My spanking new project\"/>";
                //string postData = "<UserStory Name=\"My new story\" Project=\"My spanking new project\"/>";
                string postData = "<TestCase Name=\"My new test case\"><UserStory Id=\"219\"/></TestCase>";
                request.ContentLength = postData.Length;

                StreamWriter postStream = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                postStream.Write(postData);
                postStream.Close();

                WebResponse response = (HttpWebResponse)request.GetResponse();

                //var xmlResult = XDocument.Parse(client.DownloadString(address + "/Projects"));

                //File.WriteAllText(@"c:\temp\userstories.xml", xmlResult.ToString());

                TestVerdict = QTF.TestVerdict.Pass;
                //TestMessage = xmlResult.ToString();
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
        //public QTF.TestVerdict GetUserStories(string project, string sprint)
        public QTF.TestVerdict GetUserStories()
        {
            try
            {
                Setup();

                string address = QTF.TestProperties.GetPropertyValueAsString("TargetProcessUri");

                var client = new WebClient { Credentials = new NetworkCredential("jmothershead", "Invalid!") };
                client.Headers.Add(HttpRequestHeader.ContentType, "application/xml"); // you also can use application/json content type 

                var xmlResult = client.DownloadString(address + "/TestCases?take=50");

                File.WriteAllText(@"c:\temp\userstories.xml", xmlResult);

                TestVerdict = QTF.TestVerdict.Pass;
                TestMessage = xmlResult.ToString();
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
        //public QTF.TestVerdict GetUserStories(string project, string sprint)
        public QTF.TestVerdict GetEntities()
        {
            try
            {
                Setup();

                string address = QTF.TestProperties.GetPropertyValueAsString("TargetProcessUri");

                var client = new WebClient { Credentials = new NetworkCredential("jmothershead", "Invalid!") };
                client.Headers.Add(HttpRequestHeader.ContentType, "application/xml"); // you also can use application/json content type 

                //address = address + "/TestCases/222/TestSteps";
                address = address + "/Priorities/?take=100";

                var xmlResult = client.DownloadString(address);

                File.WriteAllText(@"c:\temp\userstories.xml", xmlResult);

                TestVerdict = QTF.TestVerdict.Pass;
                TestMessage = xmlResult.ToString();
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
        public QTF.TestVerdict GetTestCases()
        {
            try
            {
                Setup();

                string address = QTF.TestProperties.GetPropertyValueAsString("TargetProcessUri");

                var client = new WebClient { Credentials = new NetworkCredential("jmothershead", "Invalid!") };
                client.Headers.Add(HttpRequestHeader.ContentType, "application/xml"); // you also can use application/json content type 

                address = address + "/TestCases?take=100";

                var xmlResult = client.DownloadString(address);

                File.WriteAllText(QTF.TestProperties.TestOutput + @"\TestCases.xml", xmlResult);

                TestVerdict = QTF.TestVerdict.Pass;
                TestMessage = xmlResult.ToString();
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
        public QTF.TestVerdict GetTestSteps()
        {
            try
            {
                Setup();

                string address = QTF.TestProperties.GetPropertyValueAsString("TargetProcessUri");

                var client = new WebClient { Credentials = new NetworkCredential("jmothershead", "Invalid!") };
                client.Headers.Add(HttpRequestHeader.ContentType, "application/xml"); // you also can use application/json content type 

                address = address + "/TestSteps?take=200";

                var xmlResult = client.DownloadString(address);

                File.WriteAllText(QTF.TestProperties.TestOutput + @"\TestSteps.xml", xmlResult);

                TestVerdict = QTF.TestVerdict.Pass;
                TestMessage = xmlResult.ToString();
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
        public QTF.TestVerdict GetTestPlans()
        {
            try
            {
                Setup();

                string address = QTF.TestProperties.GetPropertyValueAsString("TargetProcessUri");

                var client = new WebClient { Credentials = new NetworkCredential("jmothershead", "Invalid!") };
                client.Headers.Add(HttpRequestHeader.ContentType, "application/xml"); // you also can use application/json content type 

                // address = address + "/TestCases?take=100";
                address = address + "TestPlans?take=100";

                var xmlResult = client.DownloadString(address);

                File.WriteAllText(QTF.TestProperties.TestOutput + @"\TestPlans.xml", xmlResult);

                TestVerdict = QTF.TestVerdict.Pass;
                TestMessage = xmlResult.ToString();
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
        public QTF.TestVerdict CreateTestPlanRun()
        {
            try
            {
                Setup();

                //var client = new WebClient { Credentials = new NetworkCredential("jmothershead", "Invalid!") };
                //client.Headers.Add(HttpRequestHeader.ContentType, "application/xml"); // you also can use application/json content type 

                string address = "https://quintity.tpondemand.com/api/v1";

                WebRequest request = (HttpWebRequest)WebRequest.Create(address + "/TestPlanRuns");
                request.Credentials = new NetworkCredential("jmothershead", "Invalid!");
                request.Method = WebRequestMethods.Http.Post;
                request.ContentType = "application/json";

                //string postData = "<Project Name=\"My spanking new project\"/>";
                //string postData = "<UserStory Name=\"My new story\" Project=\"My spanking new project\"/>";
                //string postData = "<TestCase Name=\"My new test case\"><UserStory Id=\"219\"/></TestCase>";

                string postData = "{\"TestPlan\": {\"Id\": 225}, \"Name\": \"Joe\", \"Project\": {\"Id\": 214}, \"TestCaseRuns\": [{\"Comment\": \"Nevermind\"}]}";

                //"{\"TestPlan\": {\"Id\": 225}, " +
                //"\"Name\": \"Test plan run 2\", " +
                //"\"Project\": {\"Id\": 214}, " +
                //"\"Priority\": {\"Name\" : \"-\"}, " +
                //"\"EntityState\": {\"Name\" : \"Open\"}";
                //"\"TestCaseRuns\": [{\"Comment\": \"Nevermind\"}]}";

                //string postData =
                //    "<TestPlanRun Name=\"Test plan run 2\">" +
                //    "<Project Id =\"214\"/>" +
                //    "<TestPlan Id=\"225\"/>" +
                //    "<Priority Name =\"-\"/>" +
                //    "<EntityState Name=\"Open\"/>" +
                //    "</TestPlanRun>";

                request.ContentLength = postData.Length;

                StreamWriter postStream = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                postStream.Write(postData);
                postStream.Close();

                WebResponse response = (HttpWebResponse)request.GetResponse();

                //var xmlResult = XDocument.Parse(client.DownloadString(address + "/Projects"));

                //File.WriteAllText(@"c:\temp\userstories.xml", xmlResult.ToString());

                TestVerdict = QTF.TestVerdict.Pass;
                //TestMessage = xmlResult.ToString();
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
        public QTF.TestVerdict CreateTestCase(string targetProcessUri)
        {
            try
            {
                Setup();

                string address = "https://quintity.tpondemand.com/api/v1/TestCases";

                string postData =
                    "<TestCase Name=\"Test case #25\" " +
                        "Description=\"Test case description\"> " +

                    "<TestSteps> " +

                        "<TestStep> " +
                            "<Description>Test step #1</Description> " +
                            "<Result>Test step #1 expected result</Result>" +
                        "</TestStep> " +

                        "<TestStep> " +
                            "<Description>Test step #2</Description> " +
                            "<Result>Test step #2 expected result</Result>" +
                        "</TestStep> " +


                        "<TestStep> " +
                            "<Description>Test step #3</Description> " +
                            "<Result>Test step #3 expected result</Result>" +
                        "</TestStep> " +

                        "<TestStep> " +
                            "<Description>Test step #4</Description> " +
                            "<Result>Test step #3 expected result</Result>" +
                        "</TestStep> " +
                    "</TestSteps> " +

                    "<Project Id=\"214\"/> " +
                    "</TestCase>";

                HttpWebResponse response = postRequest(address, postData);

                // Get the stream associated with the response.
                Stream receiveStream = response.GetResponseStream();

                // Pipes the stream to a higher level stream reader with the required encoding format. 
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                TestMessage = readStream.ReadToEnd();

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
        public QTF.TestVerdict AddTestStep(string targetProcessUri)
        {
            try
            {
                Setup();

                //var client = new WebClient { Credentials = new NetworkCredential("jmothershead", "Invalid!") };
                //client.Headers.Add(HttpRequestHeader.ContentType, "application/xml"); // you also can use application/json content type 

                string address = "https://quintity.tpondemand.com/api/v1"; ;

                WebRequest request = (HttpWebRequest)WebRequest.Create(address + "/TestSteps");
                request.Credentials = new NetworkCredential("jmothershead", "Invalid!");
                request.Method = WebRequestMethods.Http.Post;
                request.ContentType = "application/xml";

                //string postData = "<Project Name=\"My spanking new project\"/>";
                //string postData = "<UserStory Name=\"My new story\" Project=\"My spanking new project\"/>";
                //string postData = "<TestCase Name=\"Test case #15\" Description=\"Test case description\"><Steps>" +
                //    "<TestStep><Description>Step1</Description><Result>Step1Result</Result></TestStep></Steps><UserStory Id=\"219\"/></TestCase>";
                //string postData = "<TestCase Name=\"Test case #5\" Description=\"Test case description\"></TestCase>";

                string postData = "<TestStep><TestCase Id=\"221\"/><Description>Step1</Description><Result>Step1Result</Result></TestStep>";

                //<TestCase Id=\"222\"</TestCase>

                //string postData = "{\"TestPlan\": {\"Id\": 225}, \"Name\": \"Joe\", \"Project\": {\"Id\": 214}, \"TestCaseRuns\": [{\"Comment\": \"Nevermind\"}]}";

                //"{\"TestPlan\": {\"Id\": 225}, " +
                //"\"Name\": \"Test plan run 2\", " +
                //"\"Project\": {\"Id\": 214}, " +
                //"\"Priority\": {\"Name\" : \"-\"}, " +
                //"\"EntityState\": {\"Name\" : \"Open\"}";
                //"\"TestCaseRuns\": [{\"Comment\": \"Nevermind\"}]}";

                //string postData =
                //    "<TestPlanRun Name=\"Test plan run 2\">" +
                //    "<Project Id =\"214\"/>" +
                //    "<TestPlan Id=\"225\"/>" +
                //    "<Priority Name =\"-\"/>" +
                //    "<EntityState Name=\"Open\"/>" +
                //    "</TestPlanRun>";

                request.ContentLength = postData.Length;

                StreamWriter postStream = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                postStream.Write(postData);
                postStream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                //if (response.StatusCode == HttpStatusCode.OK)
                //{
                QTF.TestTrace.Trace(string.Format("Response Status Code is OK and StatusDescription is: {0}",
                                     response.StatusDescription));
                //}

                // Get the stream associated with the response.
                Stream receiveStream = response.GetResponseStream();

                // Pipes the stream to a higher level stream reader with the required encoding format. 
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                //Console.WriteLine("Response stream received.");
                TestMessage = readStream.ReadToEnd();

                // Releases the resources of the response.
                response.Close();

                //var xmlResult = XDocument.Parse(client.DownloadString(address + "/Projects"));

                //File.WriteAllText(@"c:\temp\userstories.xml", xmlResult.ToString());

                TestVerdict = QTF.TestVerdict.Pass;
                //TestMessage = xmlResult.ToString();
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
        public QTF.TestVerdict CreateTestCaseWithTestStep(string testSuiteFile)
        {
            try
            {
                Setup();

                QTF.TestSuite testSuite = QTF.TestSuite.ReadFromFile(testSuiteFile);

                foreach (QTF.TestScriptObject testScriptObject in testSuite.TestScriptObjects)
                {
                    QTF.TestTrace.Trace("Test case:  " + testScriptObject.Title);

                    if (testScriptObject is QTF.TestCase)
                    {
                        QTF.TestCase testCase = testScriptObject as QTF.TestCase;

                        foreach (QTF.TestStep testStep in testCase.TestSteps)
                        {
                            QTF.TestTrace.Trace("Test step:  " + testStep.Title);
                        }
                    }
                }

                /*              

                              //foreach(TestScriptObject testSuite.TestScriptObjects


                              //var client = new WebClient { Credentials = new NetworkCredential("jmothershead", "Invalid!") };
                              //client.Headers.Add(HttpRequestHeader.ContentType, "application/xml"); // you also can use application/json content type 

                              string address = "https://quintity.tpondemand.com/api/v1"; ;

                              WebRequest request = (HttpWebRequest)WebRequest.Create(address + "/TestSteps");
                              request.Credentials = new NetworkCredential("jmothershead", "Invalid!");
                              request.Method = WebRequestMethods.Http.Post;
                              request.ContentType = "application/xml";

                              //string postData = "<Project Name=\"My spanking new project\"/>";
                              //string postData = "<UserStory Name=\"My new story\" Project=\"My spanking new project\"/>";
                              //string postData = "<TestCase Name=\"Test case #15\" Description=\"Test case description\"><Steps>" +
                              //    "<TestStep><Description>Step1</Description><Result>Step1Result</Result></TestStep></Steps><UserStory Id=\"219\"/></TestCase>";
                              //string postData = "<TestCase Name=\"Test case #5\" Description=\"Test case description\"></TestCase>";

                              string postData = "<TestStep><TestCase Id=\"241\"/><Description>Step1</Description><Result>Step1Result</Result></TestStep>";

                              //<TestCase Id=\"222\"</TestCase>

                              //string postData = "{\"TestPlan\": {\"Id\": 225}, \"Name\": \"Joe\", \"Project\": {\"Id\": 214}, \"TestCaseRuns\": [{\"Comment\": \"Nevermind\"}]}";

                              //"{\"TestPlan\": {\"Id\": 225}, " +
                              //"\"Name\": \"Test plan run 2\", " +
                              //"\"Project\": {\"Id\": 214}, " +
                              //"\"Priority\": {\"Name\" : \"-\"}, " +
                              //"\"EntityState\": {\"Name\" : \"Open\"}";
                              //"\"TestCaseRuns\": [{\"Comment\": \"Nevermind\"}]}";

                              //string postData =
                              //    "<TestPlanRun Name=\"Test plan run 2\">" +
                              //    "<Project Id =\"214\"/>" +
                              //    "<TestPlan Id=\"225\"/>" +
                              //    "<Priority Name =\"-\"/>" +
                              //    "<EntityState Name=\"Open\"/>" +
                              //    "</TestPlanRun>";

                              request.ContentLength = postData.Length;

                              StreamWriter postStream = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                              postStream.Write(postData);
                              postStream.Close();

                              HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                              //if (response.StatusCode == HttpStatusCode.OK)
                              //{
                              QTF.TestTrace.Trace(string.Format("Response Status Code is OK and StatusDescription is: {0}",
                                                   response.StatusDescription));
                              //}

                              // Get the stream associated with the response.
                              Stream receiveStream = response.GetResponseStream();

                              // Pipes the stream to a higher level stream reader with the required encoding format. 
                              StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                              //Console.WriteLine("Response stream received.");
                              TestMessage = readStream.ReadToEnd();

                              // Releases the resources of the response.
                              response.Close();

                              //var xmlResult = XDocument.Parse(client.DownloadString(address + "/Projects"));

                              //File.WriteAllText(@"c:\temp\userstories.xml", xmlResult.ToString());

                              TestVerdict = QTF.TestVerdict.Pass;
                              //TestMessage = xmlResult.ToString();
                 */
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
        public QTF.TestVerdict CreateTestPlan()
        {
            try
            {
                Setup();

                string address = "https://quintity.tpondemand.com/api/v1/TestPlans";

                string postData = 
                    "<TestPlan Name=\"Test plan 99\">" +
                        "<Project Id=\"214\"/>" +
                        "<LinkedGeneral Id=\"264\"/>" +

                        "<TestCases>" +

                            "<TestCase Name=\"Test case #30\" " +
                                "Description=\"Test case description\"> " +
                                "<Project Id=\"214\"/>" +

                                "<TestSteps> " +

                                    "<TestStep> " +
                                        "<Description>Test step #1</Description> " +
                                        "<Result>Test step #1 expected result</Result>" +
                                    "</TestStep> " +

                                    "<TestStep> " +
                                        "<Description>Test step #2</Description> " +
                                        "<Result>Test step #2 expected result</Result>" +
                                    "</TestStep> " +

                                    "<TestStep> " +
                                        "<Description>Test step #3</Description> " +
                                        "<Result>Test step #3 expected result</Result>" +
                                    "</TestStep> " +

                                    "<TestStep> " +
                                        "<Description>Test step #4</Description> " +
                                        "<Result>Test step #3 expected result</Result>" +
                                    "</TestStep> " +
                                "</TestSteps> " +
                             "</TestCase>" +

                            "<TestCase Name=\"Test case #31\" " +
                                "Description=\"Test case description\"> " +
                                "<Project Id=\"214\"/>" +

                                "<TestSteps> " +

                                "<TestStep> " +
                                    "<Description>Test step #1</Description> " +
                                    "<Result>Test step #1 expected result</Result>" +
                                "</TestStep> " +

                                "<TestStep> " +
                                    "<Description>Test step #2</Description> " +
                                    "<Result>Test step #2 expected result</Result>" +
                                "</TestStep> " +

                                "<TestStep> " +
                                    "<Description>Test step #3</Description> " +
                                    "<Result>Test step #3 expected result</Result>" +
                                "</TestStep> " +

                                "<TestStep> " +
                                    "<Description>Test step #4</Description> " +
                                    "<Result>Test step #3 expected result</Result>" +
                                "</TestStep> " +

                                "</TestSteps>" +
                            "</TestCase>" +

                             "<TestCase Name=\"Test case #32\" " +
                                "Description=\"Test case description\"> " +
                                "<Project Id=\"214\"/>" +

                                "<TestSteps> " +

                                "<TestStep> " +
                                    "<Description>Test step #1</Description> " +
                                    "<Result>Test step #1 expected result</Result>" +
                                "</TestStep> " +

                                "<TestStep> " +
                                    "<Description>Test step #2</Description> " +
                                    "<Result>Test step #2 expected result</Result>" +
                                "</TestStep> " +

                                "<TestStep> " +
                                    "<Description>Test step #3</Description> " +
                                    "<Result>Test step #3 expected result</Result>" +
                                "</TestStep> " +

                                "<TestStep> " +
                                    "<Description>Test step #4</Description> " +
                                    "<Result>Test step #3 expected result</Result>" +
                                "</TestStep> " +

                                "</TestSteps>" +
                            "</TestCase>" +
                        
                        "</TestCases>" + 
                    "</TestPlan>";

                HttpWebResponse response = postRequest(address, postData);

                // Get the stream associated with the response.
                Stream receiveStream = response.GetResponseStream();

                // Pipes the stream to a higher level stream reader with the required encoding format. 
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                TestMessage = readStream.ReadToEnd();

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

        #region Private methods

        private void createTestPlan()
        { }

        private void CreateTestCase()
        { }

        #endregion

        #region Helper methods

        private HttpWebResponse postRequest(string requestUriString, string postData)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUriString);
            request.Credentials = new NetworkCredential("jmothershead", "Invalid!");
            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = "application/xml";

            request.ContentLength = postData.Length;

            StreamWriter postStream = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
            postStream.Write(postData);
            postStream.Close();

            return (HttpWebResponse)request.GetResponse();
        }

        #endregion
    }
}
