using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QTF = Quintity.TestFramework.Core;

namespace Quintity.TestFramework.Scratch
{
    [QTF.TestClass]
    public class TestProperitiesTests : QTF.TestClassBase
    {
        [QTF.TestMethod]
        public QTF.TestVerdict SystemTestPropertiesTests(
            [QTF.TestParameter("File path")]
            string filePath)
        {
            try
            {
                Setup();

                var result = QTF.TestProperties.GetPropertyValue("Now");
                result = QTF.TestProperties.GetPropertyValue("Null");
                result = QTF.TestProperties.GetPropertyValue("EmptyString");
                result = QTF.TestProperties.GetPropertyValue("Space");
                result = QTF.TestProperties.GetPropertyValue("Today");

                QTF.TestProperties.RemoveProperty("Now");

                TestMessage = QTF.TestProperties.ToString();
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
        public QTF.TestVerdict SaveTestPropertiesToFile(
            [QTF.TestParameter("File path")]
            string filePath)
        {
            try
            {
                Setup();

                QTF.TestProperties.Save(filePath);

                TestMessage = QTF.TestProperties.ToString();
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
        public QTF.TestVerdict TestPropertyCollectionTest(string param1, int count)
        {
            try
            {
                Setup();

                QTF.TestProperty property = new QTF.TestProperty()
                {
                    Name = "MyNewStringProperty",
                    Description = "This is my new fancy property",
                    Value = "Wing Dang Doodle",
                    Active = true,
                };

                QTF.TestProperties.AddProperty(property);

                property = new QTF.TestProperty()
                {
                    Name = "MyNewBooleanProperty",
                    Description = "This is my new fancy property",
                    Value = true,
                    Active = true,
                };

                property = new QTF.TestProperty()
                {
                    Name = "MyNewIntegerProperty",
                    Description = "This is my new integer property",
                    Value = 99,
                    Active = true,
                };

                QTF.TestProperties.AddProperty(property);

                QTF.TestProperties.Save(@"C:\TestProjects\Quintity\Quintity.TestFramework\Quintity.TestFramework - Trunk\Quintity.TestFramework.Scratch\TestProperties\Quintity.TestFramework.Scratch.props");

                TestMessage = "\r\n" + property.ToString();
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
        public QTF.TestVerdict TestPropertyCollectionIOTest()
        {
            try
            {
                Setup();

                QTF.TestPropertyCollection2 properties = new QTF.TestPropertyCollection2();

                QTF.TestProperty property = new QTF.TestProperty()
                {
                    Name = "String property",
                    Description = "This is a string property",
                    Value = "Wing Dang Doodle",
                    Active = true
                };

                properties.Add(property);

                property = new QTF.TestProperty()
                 {
                     Name = "Boolean property",
                     Description = "This is a boolean property",
                     Value = true,
                     Active = true
                 };

                properties.Add(property);

                property = new QTF.TestProperty()
                {
                    Name = "Integer property",
                    Description = "This is an integer property",
                    Value = -99,
                    Active = false
                };

                properties.Add(property);

                property = new QTF.TestProperty()
                {
                    Name = "Null property",
                    Description = "This is a null property",
                    Value = null,
                    Active = false
                };

                properties.Add(property);

                //serializeToFile(properties, @"c:\temp\TestProperties.xml", null);

                //properties = deserializeFromFile(@"c:\temp\TestProperties.xml", null);
                object value;
                bool success = TryGetValue(properties, "Null property", out value);
                success = TryGetValue(properties, "Bogus property", out value);
                success = TryGetValue(properties, "BooLean property", out value);
                success = TryGetValue(properties, "Integer property", out value);


                //string name = "Null property";

                //var spud = properties.Where(i => i.Name.ToUpper() == name.ToUpper()).FirstOrDefault();

                //if (null != spud)
                //{
                //    TestVerdict = QTF.TestVerdict.Pass;
                //    TestMessage = spud.Value.ToString();
                //}
                //else
                //{
                //    TestVerdict = QTF.TestVerdict.Fail;
                //    TestMessage = spud.Value.ToString();
                //}

                //TestMessage = "\r\n" + properties.ToString();
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
        public QTF.TestVerdict RemoveSystemProperty(string name)
        {
            try
            {
                Setup();

                var testProperty = QTF.TestProperties.RemoveProperty(name);

                TestMessage = QTF.TestProperties.ToString();
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
        public QTF.TestVerdict ChangeSystemProperty()
        {
            try
            {
                Setup();

                var testProperty = QTF.TestProperties.GetProperty("Now");

                QTF.TestProperties.SetPropertyValue("Now", DateTime.Now);

                TestMessage = QTF.TestProperties.ToString();
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

        public bool TryGetValue(QTF.TestPropertyCollection2 properties, string name, out object value)
        {
            bool success = false;
            value = null;

            var testProperty = properties.Where(i => i.Name.ToUpper() == name.ToUpper()).FirstOrDefault();

            if (null != testProperty)
            {
                value = testProperty.Value;
                success = true;
            }

            return success;
        }

        private static void serializeToFile(QTF.TestPropertyCollection2 testProperties, string filePath, List<Type> knownTypes)
        {
            FileStream writer = null;

            try
            {
                System.Runtime.Serialization.DataContractSerializer serializer =
                    new System.Runtime.Serialization.DataContractSerializer(typeof(QTF.TestPropertyCollection2), knownTypes);

                // Create a FileStream to write with.
                writer = new FileStream(filePath, FileMode.Create);

                // Write object out.
                serializer.WriteObject(writer, testProperties);

                writer.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (writer != null)
                {
                    // Close file.
                    writer.Close();
                }
            }
        }

        private static QTF.TestPropertyCollection2 deserializeFromFile(string filePath, List<Type> knownTypes)
        {
            FileStream reader = null;
            QTF.TestPropertyCollection2 testProperties = null;

            try
            {
                // Create DataContractSerializer.
                System.Runtime.Serialization.DataContractSerializer serializer =
                    new System.Runtime.Serialization.DataContractSerializer(typeof(QTF.TestPropertyCollection2), knownTypes);

                // Create a file stream to read into.
                reader = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                // Read into object.
                testProperties = serializer.ReadObject(reader) as QTF.TestPropertyCollection2;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    // Close file.
                    reader.Close();
                }
            }

            return testProperties;
        }
    }
}