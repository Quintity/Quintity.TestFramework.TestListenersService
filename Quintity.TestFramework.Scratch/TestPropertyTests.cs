using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using QTF = Quintity.TestFramework.Core;
using Quintity.TestFramework.Runtime;

namespace Quintity.TestFramework.Scratch
{
    [DataContract]
    public class NewTestProperties
    {
        [DataMember]
        List<QTF.TestProperty> _testProperties;

        [DataMember]
        TestListenerCollection _testListeners;

        public NewTestProperties()
        {
            _testProperties = new List<QTF.TestProperty>();
            _testListeners = new TestListenerCollection();
        }

        public void AddProperty(QTF.TestProperty testProperty)
        {
            _testProperties.Add(testProperty);
        }

        public void AddListener(TestListenerDescriptor testListener)
        {
            _testListeners.Add(testListener);
        }
    }

    [QTF.TestClass]
    public class TestPropertyTests : ScratchBase
    {
        NewTestProperties _testProperties;

        [QTF.TestMethod]
        public QTF.TestVerdict CreateTestProperties()
        {
            try
            {
                Setup();

                _testProperties = new NewTestProperties();

                //QTF.TestProperty property = new QTF.TestProperty()
                //{

                //    Description = "String test property",
                //    Key = "Key1",
                //    Value = "Value1"
                //};

                //_testProperties.AddProperty(property);

                //property = new QTF.TestProperty()
                //{

                //    Description = "Bool test property",
                //    Key = "Key2",
                //    Value = false
                //};

                //_testProperties.AddProperty(property);

                //property = new QTF.TestProperty()
                //{

                //    Description = "Int32 test property",
                //    Key = "Key2",
                //    Value = 99
                //};

                //_testProperties.AddProperty(property);
                //_testProperties.AddListener(new QTF.TestListenerDescriptor());

                //serializeToFile(@"c:\temp\TestProperties", null);

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

        private void serializeToFile(string filePath, List<Type> knownTypes)
        {
            FileStream writer = null;

            try
            {
                //knownTypes.Add(typeof(TestListenerCollection));

                System.Runtime.Serialization.DataContractSerializer serializer =
                    new System.Runtime.Serialization.DataContractSerializer(typeof(NewTestProperties), knownTypes);

                // Create a FileStream to write with.
                writer = new FileStream(filePath, FileMode.Create);

                // Write object out.
                serializer.WriteObject(writer, _testProperties);

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
    }
}
