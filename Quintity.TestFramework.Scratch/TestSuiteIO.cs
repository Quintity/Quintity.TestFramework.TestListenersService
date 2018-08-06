using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Text;
using Quintity.TestFramework.Core;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Scratch
{
    [TestClass]
    public class TestSuiteIO : TestClassBase
    {
        [TestMethod]
        public TestVerdict ConvertFile(
            [Core.TestParameter("Target test suite", "Enter the full path of test suite to convert.")]
           string targetTestSuite
           )
        {
            try
            {
                Setup();
                TestAssert.IsFalse(string.IsNullOrEmpty(targetTestSuite), "The target test suite cannot be a null or empty string value.");
                TestAssert.IsTrue(File.Exists(targetTestSuite), $"The target test suite path \"{targetTestSuite}\" is not a valid.");

                var testSuite = TestSuite.ReadFromOldFileFormat(targetTestSuite);
                TestSuite.Write(testSuite, true);

                //var testSuite = TestProperties.CurrentTestSuite;
                //TestSuite.SaveToFile2(testSuite);

                //TestSuite.ReadFromFile2(@"C:\DevProjects\Quintity\Quintity.TestFramework\Quintity.TestFramework - Trunk\Quintity.TestFramework.Scratch\TestOutput\TestSuiteIO.ste");

                TestVerdict = TestVerdict.Pass;
                TestMessage = "This is the initial test message.";
            }
            catch (Exception e)
            {
                TestMessage += e.ToString();
                TestVerdict = TestVerdict.Error;
            }
            finally
            {
                Teardown();
            }

            return TestVerdict;
        }

        [TestMethod]
        public TestVerdict WriteTestSuite(
            string testSuiteFile,
            [Core.TestParameter("Big dummy1")]
            int dummy1,
            [Core.TestParameter("Big dummy2")]
            bool dummy2)
        {
            try
            {
                Setup();

                //var testSuite = TestProperties.CurrentTestSuite;
                //TestSuite.SaveToFile2(testSuite);

                //TestSuite.ReadFromFile2(@"C:\DevProjects\Quintity\Quintity.TestFramework\Quintity.TestFramework - Trunk\Quintity.TestFramework.Scratch\TestOutput\TestSuiteIO.ste");

                TestVerdict = TestVerdict.Pass;
                TestMessage = "This is the initial test message.";
            }
            catch (Exception e)
            {
                TestMessage += e.ToString();
                TestVerdict = TestVerdict.Error;
            }
            finally
            {
                Teardown();
            }

            return TestVerdict;
        }

        private void writeTestSuite(TestSuite testSuite, string outputFolder)
        {
            XmlTextWriter xmlWriter = null;

            try
            {
                // First make backup....
               // testSuite.CreateBackup();

                // Write up queue information.
                xmlWriter = new XmlTextWriter($"{outputFolder}\\{testSuite.FileName}", null);
                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.WriteStartDocument();

                // Prepare suite results header.
                xmlWriter.WriteStartElement("TestSuite");

                // Write attributes.
                xmlWriter.WriteAttributeString("testStatus", testSuite.Status.ToString());

                // Suite header information.
                xmlWriter.WriteElementString("SystemID", testSuite.SystemID.ToString());
                xmlWriter.WriteElementString("UserID", "", testSuite.UserID);
                xmlWriter.WriteElementString("Author", "", testSuite.Author);
                xmlWriter.WriteElementString("Created", "", testSuite.Created.Ticks.ToString());
                xmlWriter.WriteElementString("Project", "", testSuite.Project);
                xmlWriter.WriteElementString("Title", "", testSuite.Title);
                xmlWriter.WriteElementString("Reference", testSuite.Reference);

                //testSuite.WriteProperties(xmlWriter);

                // Write pre-process method.
                xmlWriter.WriteStartElement("PreProcess");
                //testSuite.PreProcessor.Write(xmlWriter);
                xmlWriter.WriteEndElement();

                // Write post-process method
                xmlWriter.WriteStartElement("PostProcess");
                //testSuite.PostProcessor.Write(xmlWriter);
                xmlWriter.WriteEndElement();

                // Write description
                xmlWriter.WriteStartElement("Description");
                xmlWriter.WriteCData(testSuite.Description);
                xmlWriter.WriteEndElement();

                // Write elements
                xmlWriter.WriteStartElement("TestScriptObjects");

                // If test suites or cases exist, write....
                foreach (TestScriptObject testScriptObject in testSuite.TestScriptObjects)
                {
                    if (testScriptObject is TestCase)
                    {
                        //xmlWriter.WriteStartElement("TestScriptObject");
                        ////var bob = SerializeToString(testScriptObject as TestCase);
                        //xmlWriter.WriteElementString("TestCase", bob);
                        //xmlWriter.WriteEndElement();
                        // Write test case.
                        //((TestCase)testScriptObject).Write(xmlWriter);
                        WriteCase(testScriptObject as TestCase, xmlWriter);
                    }
                    else if (testScriptObject is TestSuite) // Must be test suite.
                    {
                        // Write test suite file information.
                        xmlWriter.WriteStartElement("TestScriptObject");
                        xmlWriter.WriteAttributeString("objectType", "TestSuite");
                        writeTestSuite(testScriptObject as TestSuite, outputFolder);
                        xmlWriter.WriteString(((TestSuite)testScriptObject).FilePath);
                        xmlWriter.WriteEndElement();
                    }
                }

                // Clean up.
                xmlWriter.WriteEndElement();	// TestScriptObjects.	
                xmlWriter.WriteEndElement();	// TestSuite header.	
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                if (xmlWriter != null)
                {
                    //Error opening results file, close (if open) and stop writting.
                    xmlWriter.Close();
                    xmlWriter = null;
                }
            }
        }

        public void WriteCase(TestCase testCase, XmlTextWriter xmlWriter)
        {
            // Write test case attributes.
            xmlWriter.WriteStartElement("TestScriptObject");
            xmlWriter.WriteAttributeString("objectType", "TestCase");
            xmlWriter.WriteAttributeString("testStatus", testCase.Status.ToString());

            // Write test case elements.
            xmlWriter.WriteElementString("SystemID", "", testCase.SystemID.ToString());
            xmlWriter.WriteElementString("UserID", "", testCase.UserID);
            xmlWriter.WriteElementString("Author", "", testCase.Author);
            xmlWriter.WriteElementString("Created", "", testCase.Created.Ticks.ToString());
            xmlWriter.WriteElementString("FunctionalArea", testCase.FunctionalArea);
            xmlWriter.WriteElementString("Title", "", testCase.Title);

            // Write test case properties.
            //WriteProperties(xmlWriter);

            //// Write pre-process method.
            //xmlWriter.WriteStartElement("PreProcess");
            //PreProcessor.Write(xmlWriter);
            //xmlWriter.WriteEndElement();

            //// Write post-process method
            //xmlWriter.WriteStartElement("PostProcess");
            //PostProcessor.Write(xmlWriter);
            //xmlWriter.WriteEndElement();

            xmlWriter.WriteElementString("OnFailure", testCase.OnTestStepFailure.ToString());
            xmlWriter.WriteElementString("Tags", "", testCase.Tags);
            xmlWriter.WriteElementString("Defect", testCase.Defects);
            xmlWriter.WriteElementString("KnownDefects", testCase.KnownDefects.ToString());
            xmlWriter.WriteElementString("Reference", testCase.Reference);

            // Write comment
            //xmlWriter.WriteStartElement("Comment");
            //xmlWriter.WriteCData(testCase.);
            //xmlWriter.WriteEndElement();

            // Write description
            xmlWriter.WriteStartElement("Description");
            xmlWriter.WriteCData(testCase.Description);
            xmlWriter.WriteEndElement();

            // Write expected
            //xmlWriter.WriteStartElement("Expected");
            //xmlWriter.WriteCData(testCase);
            //xmlWriter.WriteEndElement();

            // Write test steps
            xmlWriter.WriteStartElement("TestScriptObjects");

            // If test actions exist, iterate through and write....
            if (testCase.TestSteps != null)
            {
                foreach (TestStep testStep in testCase.TestSteps)
                {
                    writeTestStep(testStep, xmlWriter);
                    //testStep.Write(xmlWriter);
                }
            }

            xmlWriter.WriteEndElement();  // TestSteps
            xmlWriter.WriteEndElement();  // TestScriptObject
        }

        private void writeTestStep(TestStep testStep, XmlTextWriter xmlWriter)
        {
            // Setup up test element container.
            xmlWriter.WriteStartElement("TestScriptObject");
            xmlWriter.WriteAttributeString("objectType", "TestStep");
            xmlWriter.WriteAttributeString("testStatus", testStep.Status.ToString());
            //xmlWriter.WriteAttributeString("testType", testStep.Type.ToString());

            // Suite header information.
            xmlWriter.WriteElementString("SystemID", testStep.SystemID.ToString());
            xmlWriter.WriteElementString("UserID", testStep.UserID);
            xmlWriter.WriteElementString("Title", "", testStep.Title);

            // Write automated test expected results (Pass/Fail)
            xmlWriter.WriteElementString("ExpectedResult", testStep.ExpectedTestVerdict.ToString());

            xmlWriter.WriteElementString("AlwaysExecute", testStep.AlwaysExecute.ToString());

            // Write description
            xmlWriter.WriteStartElement("Description");
            xmlWriter.WriteCData(testStep.Description);
            xmlWriter.WriteEndElement();

            // Write expected
            xmlWriter.WriteStartElement("Expected");
            xmlWriter.WriteCData(testStep.ExpectedBehavior);
            xmlWriter.WriteEndElement();

            // Write test method.
            xmlWriter.WriteStartElement("TestMethod");
             
            if (testStep.TestAutomationDefinition != null)
            {
                writeTestAutomationDefinition(testStep.TestAutomationDefinition, xmlWriter);
            }

            xmlWriter.WriteEndElement();  // TestMethod

            xmlWriter.WriteEndElement();  // TestScriptObject
        }

        private void writeTestAutomationDefinition(TestAutomationDefinition testAutomationDefinition, XmlTextWriter xmlWriter)
        {
            // Automation values.
            xmlWriter.WriteElementString("Assembly", (testAutomationDefinition.TestAssembly == null) ?
                null : testAutomationDefinition.TestAssembly);

            xmlWriter.WriteElementString("Class", testAutomationDefinition.TestClass);
            xmlWriter.WriteElementString("Method", testAutomationDefinition.TestMethod);

            xmlWriter.WriteStartElement("Parameters");

            // If test parameters exist, write.
            if (testAutomationDefinition.TestParameters != null)
            {
                foreach (TestParameter param in testAutomationDefinition.TestParameters)
                {
                    // Writes parent element name.
                    xmlWriter.WriteStartElement("Parameter");

                    // Write element attributes for name.
                    xmlWriter.WriteAttributeString("key", param.Name);
                    xmlWriter.WriteAttributeString("dtype", param.TypeAsString);
                    xmlWriter.WriteAttributeString("value", param.ValueAsString);

                    // Ends parent element.
                    xmlWriter.WriteEndElement();
                }
            }

            xmlWriter.WriteEndElement();
        }

        //public override void Read(string suiteFile)
        //{
        //    try
        //    {
        //        var testSuite = new TestSuite();
        //        // Save name away.
        //        testSuite.FilePath = suiteFile;

        //        // Read document and prepare navigator.
        //        testSuite.FilePath = suiteFile;
        //        XPathDocument doc = new XPathDocument(suiteFile);
        //        XPathNavigator nav = doc.CreateNavigator();

        //        // Move to element.
        //        nav.MoveToFirstChild();

        //        // Get status attribute.
        //        testSuite.m_testStatus = testSuite.toStatus(nav.GetAttribute("testStatus", ""));

        //        // Extract object system ID.
        //        testSuite.SystemID = new Guid(getXPathValue(nav, "SystemID"));

        //        // Extract data fields.
        //        testSuite.m_userID = getXPathValue(nav, "UserID");
        //        testSuite.m_author = getXPathValue(nav, "Author");
        //        testSuite.m_dateCreated = new DateTime(Convert.ToInt64(getXPathValue(nav, "Created")));
        //        testSuite.m_project = getXPathValue(nav, "Project");
        //        testSuite.m_title = getXPathValue(nav, "Title");
        //        testSuite.m_description = getXPathValue(nav, "Description");
        //        testSuite.m_reference = getXPathValue(nav, "Reference");

        //        XPathNodeIterator iterator = null;

        //        // Extract suite properties.
        //        iterator = nav.Select("Properties");
        //        iterator.MoveNext();
        //        testSuite.ReadProperties(iterator.Current);

        //        // Extract pre-process event
        //        iterator = nav.Select("PreProcess");
        //        iterator.MoveNext();
        //        testSuite.PreProcessor = new TestPreprocessContainer(this, iterator.Current);

        //        // Extract post-process event.
        //        iterator = nav.Select("PostProcess");
        //        iterator.MoveNext();
        //        testSuite.PostProcessor = new TestPostprocessContainer(this, iterator.Current);

        //        // Collect test case locations from group file.
        //        iterator = nav.Select("TestScriptObjects/TestScriptObject");

        //        // Create a test suite container element.
        //        TestScriptObject testObject = null;
        //        string testtype = null;
        //        string testStatus = null;

        //        while (iterator.MoveNext())
        //        {
        //            // Get suite attributes.
        //            testtype = iterator.Current.GetAttribute("objectType", "");
        //            testStatus = iterator.Current.GetAttribute("testStatus", "");

        //            TestStatus status = QTF.TestStatus.Unavailable;

        //            Enum.TryParse<TestStatus>(testStatus, out status);

        //            //HACK until change is complete to objectType;
        //            testtype = (testtype == "") ? iterator.Current.GetAttribute("type", "") : testtype;

        //            if (testtype == "TestCase")
        //            {
        //                testObject = new TestCase(iterator.Current, this);
        //            }
        //            else if (testtype == "TestSuite")
        //            {
        //                testObject = new TestSuite(QTF.TestProperties.ExpandPropertyAsString(iterator.Current.Value), this);
        //            }

        //            TestScriptObjectWrapper wrapper = new TestScriptObjectWrapper(testObject, status);

        //            testSuite.TestScriptObjects.Add(testObject);
        //        }
        //    }
        //    catch (System.IO.FileNotFoundException)
        //    {
        //        testSuite.m_title = "Invalid suite source file";
        //        testSuite.m_description = "The file does not appear to be a valid or well-formed test suite.";
        //        testSuite.m_testStatus = TestStatus.Unavailable;

        //        // If client event handler defined...
        //        if (TestSuite.OnUnavailableRead != null)
        //        {
        //            TestSuite.OnUnavailableRead(this);
        //        }
        //    }
        //    catch (System.Xml.XmlException e)
        //    {
        //        testSuite.m_title = "Invalid suite source file";
        //        testSuite.m_description = "The file does not appear to be a valid or well-formed test suite.";
        //        testSuite.m_testStatus = TestStatus.Unavailable;

        //        // If client event handler defined...
        //        if (TestSuite.OnUnavailableRead != null)
        //        {
        //            TestSuite.OnUnavailableRead(this);
        //        }
        //    }
        //}

        //protected string getXPathValue(XPathNavigator navigator, string expression)
        //{
        //    return this.getXPathValue(navigator, expression, null);
        //}

        public string ToXmlString(object value, string defaultNamespace)
        {
            var writer = new StringWriter();
            (new XmlSerializer(value.GetType(), defaultNamespace)).Serialize(writer, value);
            return writer.ToString();
        }

        private string SerializeToString(object objectToSerialize)
        {
            var serializer = new DataContractSerializer(objectToSerialize.GetType());
            var settings = new XmlWriterSettings() { Indent = true };
            var output = new StringBuilder();
            var xmlWriter = XmlWriter.Create(output, settings);

            serializer.WriteObject(xmlWriter, objectToSerialize);
            xmlWriter.Close();

            return output.ToString();
        }
    }
}
