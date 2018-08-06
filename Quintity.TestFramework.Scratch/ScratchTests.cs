using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using QTF = Quintity.TestFramework.Core;
using Quintity.TestFramework.Runtime;

namespace Quintity.TestFramework.Scratch
{
    public class PerfomanceInfoData
    {
        public Int64 CommitTotalPages;
        public Int64 CommitLimitPages;
        public Int64 CommitPeakPages;
        public Int64 PhysicalTotalBytes;
        public Int64 PhysicalAvailableBytes;
        public Int64 SystemCacheBytes;
        public Int64 KernelTotalBytes;
        public Int64 KernelPagedBytes;
        public Int64 KernelNonPagedBytes;
        public Int64 PageSizeBytes;
        public int HandlesCount;
        public int ProcessCount;
        public int ThreadCount;
    }


    [Quintity.TestFramework.Core.TestClass("Sample Bonanza tests", "Really important collection of really important tests.")]
    public class ScratchTests : ScratchBase
    {
        [DllImport("psapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetPerformanceInfo([Out] out PsApiPerformanceInformation PerformanceInformation, [In] int Size);

        [StructLayout(LayoutKind.Sequential)]
        public struct PsApiPerformanceInformation
        {
            public int Size;
            public IntPtr CommitTotal;
            public IntPtr CommitLimit;
            public IntPtr CommitPeak;
            public IntPtr PhysicalTotal;
            public IntPtr PhysicalAvailable;
            public IntPtr SystemCache;
            public IntPtr KernelTotal;
            public IntPtr KernelPaged;
            public IntPtr KernelNonPaged;
            public IntPtr PageSize;
            public int HandlesCount;
            public int ProcessCount;
            public int ThreadCount;
        }


        #region Data members

        #endregion

        #region Constructors

        public ScratchTests()
        { }

        #endregion

        #region Test methods

        //static PerformanceCounter cpuCounter;
        //static PerformanceCounter memory;

        [QTF.TestMethod]
        public QTF.TestVerdict TestMetricTest()
        {
            try
            {
                Setup();

                PerfomanceInfoData data = new PerfomanceInfoData();
                PsApiPerformanceInformation perfInfo = new PsApiPerformanceInformation();
                if (GetPerformanceInfo(out perfInfo, Marshal.SizeOf(perfInfo)))
                {
                    /// data in pages
                    data.CommitTotalPages = perfInfo.CommitTotal.ToInt64();
                    data.CommitLimitPages = perfInfo.CommitLimit.ToInt64();
                    data.CommitPeakPages = perfInfo.CommitPeak.ToInt64();

                    /// data in bytes
                    Int64 pageSize = perfInfo.PageSize.ToInt64();
                    data.PhysicalTotalBytes = perfInfo.PhysicalTotal.ToInt64() * pageSize;
                    data.PhysicalAvailableBytes = perfInfo.PhysicalAvailable.ToInt64() * pageSize;
                    data.SystemCacheBytes = perfInfo.SystemCache.ToInt64() * pageSize;
                    data.KernelTotalBytes = perfInfo.KernelTotal.ToInt64() * pageSize;
                    data.KernelPagedBytes = perfInfo.KernelPaged.ToInt64() * pageSize;
                    data.KernelNonPagedBytes = perfInfo.KernelNonPaged.ToInt64() * pageSize;
                    data.PageSizeBytes = pageSize;

                    /// counters
                    data.HandlesCount = perfInfo.HandlesCount;
                    data.ProcessCount = perfInfo.ProcessCount;
                    data.ThreadCount = perfInfo.ThreadCount;
                }


                var bob = new PerformanceCounter("Processor", @"% Processor Time", @"_Total");

                bob.NextValue();
                var name = bob.CategoryName;
                Thread.Sleep(5000);

                //cpuCounter = new PerformanceCounter("Processor", @"% Processor Time", @"_Total");
                //var percent = cpuCounter.NextValue();
                //percent = cpuCounter.NextValue();
                //percent = cpuCounter.NextValue();
                //percent = cpuCounter.NextValue();

                //memory = new PerformanceCounter("Memory");

                TestVerdict = QTF.TestVerdict.Pass;
                TestMessage = "This is the initial test message.";
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
        public QTF.TestVerdict TestVerdictOverride()
        {
            try
            {
                Setup();

                string[] errorList = new string[] { "Error one", "Error two", "Error three", "Error four" };

                QTF.TestCheck.IsTrue("Verify error message count", errorList.Count<string>() == 5, "5", errorList.Count<string>());

                QTF.TestCheck.IsTrue("Verify error message count", errorList.Count<string>() == 5, "5", errorList.Count<string>().ToString());

                QTF.TestCheck.IsTrue("Verify error message count", errorList.Count<string>() == 5, "5 errors", errorList.Count<string>().ToString() + " errors");

                QTF.TestCheck.IsTrue("Verify error message count", errorList.Count<string>() == 5, "Expected:  {0} errors, Actual {1} errors", 5, errorList.Count<string>());

                QTF.TestCheck.IsTrue("Sample test check", false);

                TestVerdict = QTF.TestVerdict.Pass;
                TestMessage = "This is the initial test message.";
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
        public QTF.TestVerdict TestChecksTest()
        {
            try
            {
                Setup();

                string[] errorList = new string[] { "Error one", "Error two", "Error three", "Error four" };

                QTF.TestCheck.IsTrue("Verify error message count", errorList.Count<string>() == 5, "5", errorList.Count<string>());

                QTF.TestCheck.IsTrue("Verify error message count", errorList.Count<string>() == 5, "5", errorList.Count<string>().ToString());

                QTF.TestCheck.IsTrue("Verify error message count", errorList.Count<string>() == 5, "5 errors", errorList.Count<string>().ToString() + " errors");

                QTF.TestCheck.IsTrue("Verify error message count", errorList.Count<string>() == 5, "Expected:  {0} errors, Actual {1} errors", 5, errorList.Count<string>());

                QTF.TestCheck.IsTrue("Sample test check", false);

                TestVerdict = QTF.TestVerdict.Pass;
                TestMessage = "This is the initial test message.";
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
        public QTF.TestVerdict ThrowException()
        {
            try
            {
                Setup();

                QTF.TestAssert.IsFalse(true, "Always throws a test assert.");

                TestVerdict = QTF.TestVerdict.Pass;
                TestMessage = "This is the initial test message.";
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
        public QTF.TestVerdict ScratchTest( int duration)
        {
            try
            {
                Setup();

                Thread.Sleep(duration * 1000);

                QTF.TestTrace.Trace(QTF.TestProperties.ExpandString("[TestAssemblies]"));

                QTF.TestCheck.IsTrue("Sample test check", true);

                TestVerdict = QTF.TestVerdict.Pass;
                TestMessage = "This is the initial test message.";
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
        public QTF.TestVerdict TestPropertyCollectionTest()
        {
            try
            {
                Setup();

                QTF.TestPropertyCollection2 properties = new QTF.TestPropertyCollection2();

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
        public QTF.TestVerdict StringManipulation(string original)
        {
            try
            {
                Setup();

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < original.Length - 1; i++)
                {
                    char c = original[i];
                    QTF.TestTrace.Trace(Convert.ToString(c));
                }

                object bob = original.Reverse<char>();

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

        public bool ConstructNodeTree(QTF.TestScriptObject testScriptObject)
        {
            QTF.TestTrace.Trace(testScriptObject.Title);
            return true;
        }

        public bool TraceOut(QTF.TestScriptObject testScriptObject)
        {
            QTF.TestTrace.Trace(testScriptObject.ToString());
            return true;
        }

        [QTF.TestMethod]
        public QTF.TestVerdict XmlDeserializeFromString(string filePath)
        {
            try
            {
                Setup();

                string objectData = File.ReadAllText(filePath);

                var serializer = new XmlSerializer(typeof(Core.TestSuite));
                object result;

                using (TextReader reader = new StringReader(objectData))
                {
                    result = serializer.Deserialize(reader);
                }

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
        public QTF.TestVerdict XmlSerializeToString(string filePath)
        {
            try
            {
                Setup();

                Core.TestSuite ts = Core.TestSuite.DeserializeFromFile(filePath);

                string bob = DataContractSerializeObject<Core.TestSuite>(ts);

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

        public static string DataContractSerializeObject<T>(T objectToSerialize)
        {
            using (var output = new StringWriter())
            using (var writer = new XmlTextWriter(output) { Formatting = Formatting.Indented })
            {
                new DataContractSerializer(typeof(T)).WriteObject(writer, objectToSerialize);
                return output.GetStringBuilder().ToString();
            }
        }

        [QTF.TestMethod]
        public QTF.TestVerdict ConfigurationTest(int i)
        {
            try
            {
                Setup();

                QTF.TestWarning.IsTrue(1 < 0, "Warning:  {0}", "Bob");

                //object spud = Quintity.TestFramework.TestEngineer.Properties.Settings.Default;

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
        public QTF.TestVerdict DelegateTest(int i)
        {
            try
            {
                Setup();

                //QTF.TestSuite testSuite = QTF.TestSuite.DeserializeFromFile(@"C:\TestProjects\Quintity\Quintity.TestFramework\Quintity.TestFramework - Trunk\Quintity.TestFramework.Scratch\TestSuites\Quintity.TestFramework.Scratch.Tests.ste");

                //var foundObject = testSuite.Find(new Guid("90729eba-619e-4b9d-b734-73b2e132a7ce"));

                //i = 1;
                //QTF.TestSuite testSuite = new QTF.TestSuite("MyTestSuite");

                //QTF.TestExecutor.TraverseTestTree(testSuite, new QTF.TestExecutor.TraverseTestTreeDelegate(ConstructNodeTree));

                //QTF.TestExecutor.TraverseTestTree(testSuite, new QTF.TestExecutor.TraverseTestTreeDelegate(TraceOut));

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

        [QTF.TestMethod("My very important tests", "This is an extremely important test method.")]
        public QTF.TestVerdict MyTestMethod(
            [QTF.TestParameter("String parameter", "Enter string parameter", "Default value")]
                string param1,
            [QTF.TestParameter("Int parameter", "Enter integer parameter", 0)]
                int param2,
            [QTF.TestParameter("Boolean parameter", "Enter boolean parameter", true)]
                bool param3)
        {
            try
            {
                Setup();

                QTF.TestSuite testSuite = QTF.TestSuite.DeserializeFromFile(@"C:\TestProjects\Quintity\Quintity.TestFramework\Quintity.TestFramework - Trunk\Quintity.TestFramework.Scratch\TestSuites\Quintity.TestFramework.Scratch.Tests.ste");

                //QTF.TestSuite testSuite = new QTF.TestSuite("MyTestSuite");

                // QTF.TestExecutor.TraverseTestTree(testSuite, new QTF.TestExecutor.TraverseTestTreeDelegate(ConstructNodeTree));

                Thread.Sleep(5000);

                //QTF.TestExecutor.WalkTestTree(testSuite, new QTF.TestExecutor.WalkTestTreeDelegate(TraceOut));

                //QTF.TestAutomationDefinition firstdefinition = new QTF.TestAutomationDefinition();
                //firstdefinition.TestAssembly = "TestAssembly";
                //firstdefinition.TestClass = "TestClass";
                //firstdefinition.TestMethod = "TestMethod";
                //firstdefinition.TestParameters = new QTF.TestParameterCollection();
                //firstdefinition.TestParameters.Add(new QTF.TestParameter("Parameter1", "Parameter2", "OriginalName", typeof(string)));

                //QTF.TestAutomationDefinition seconddefinition = new QTF.TestAutomationDefinition(firstdefinition);

                //firstdefinition.TestParameters[0].Name = "spud";


                //QTF.TestWarning.Fail("This is a failed warning comment.");

                //QTF.TestTrace.Trace("This is a sample TestTrace message.");

                ////for (int i = 1; i <= param2; i++)
                ////{
                ////    QTF.TestCheck.IsFalse(string.Format("Test check {0}", i), param3, "This is a sample TestCheck.");
                ////}

                //QTF.TestCheck.Fail("This is the first failed test check.");
                //QTF.TestCheck.Fail("This is the second failed test check.");
                //QTF.TestCheck.Fail("This is the third failed test check.");

                //Thread.Sleep(3000);

                TestMessage += "This is the test message.";
                TestVerdict = param3 ? QTF.TestVerdict.Pass : QTF.TestVerdict.Fail;
            }
            catch (QTF.TestCheckFailedException e)
            {
                TestMessage += e.ToString();
                TestVerdict = QTF.TestVerdict.Fail;
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
        public QTF.TestVerdict PassingSimpleClassMethod(
            [QTF.TestParameter("String parameter", "Enter string parameter", "Default value")]
                string param1,
            [QTF.TestParameter("Int parameter", "Enter integer parameter", 0)]
                int param2,
            [QTF.TestParameter("Boolean parameter", "Enter boolean parameter", true)]
                bool param3,
            QTF.TestCase param4)
        {
            try
            {
                Setup();

                //QTF.TestCase testcase = QTF.TestProperties.GetValue("TestCase") as QTF.TestCase;

                //QTF.TestProperties.SetProperty("bummer", new SimpleClass());
                System.Reflection.Assembly assy = System.Reflection.Assembly.GetExecutingAssembly();
                QTF.TestProperties.SetPropertyValue("assembly", assy);

                List<Type> knownTypes = new List<Type>();
                knownTypes.Add(assy.GetType());
                QTF.TestProperties.Save(@"c:\temp\dork.xml", knownTypes);
                TestMessage += param1;
                TestVerdict = QTF.TestVerdict.Pass;
            }
            catch (QTF.TestCheckFailedException e)
            {
                TestMessage += e.ToString();
                TestVerdict = QTF.TestVerdict.Fail;
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
        public QTF.TestVerdict AllDatatypes(
            [QTF.TestParameter("String parameter", "Enter string parameter", "Default value")]
                string param1,
            [QTF.TestParameter("Int parameter", "Enter integer parameter")]
                int param2,
            [QTF.TestParameter("Boolean parameter", "Enter boolean parameter", true)]
                bool param3,
            [QTF.TestParameter("Nullable int parameter", "Enter nullable int parameter")]
            int? intParam,
            [QTF.TestParameter("Datetime parameter", "Enter datetime parameter")]
            DateTime dateParam)
        {
            try
            {
                Setup();

                TestVerdict = QTF.TestVerdict.Pass;
            }
            catch (QTF.TestCheckFailedException e)
            {
                TestMessage += e.ToString();
                TestVerdict = QTF.TestVerdict.Fail;
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
        public QTF.TestVerdict WriteTestProperties()
        {
            try
            {
                Setup();

                //TestListenerCollection listeners = QTF.TestProperties.TestListeners;

                //QTF.TestProperties.SetProperty("Uri", "http://google.com");

                TestListenerCollection testListeners = new TestListenerCollection();
                var arguments = new List<string>();
                //Dictionary<string, string> arguments = new Dictionary<string, string>();
                //arguments.Add("Argument1", "Milky Way");
                testListeners.Add(new TestListenerDescriptor()
                {
                    Name = "TestListener1",
                    Description = "Sample test listner",
                    Assembly = @"C:\TestProjects\Quintity\Quintity.TestFramework\Quintity.TestFramework - Trunk\Quintity.TestFramework.SampleListener\bin\Debug\Quintity.TestFramework.SampleListener.dll",
                    Type = "Quintity.TestFramework.SampleListener",
                    OnFailure = QTF.OnFailure.Stop,
                    Status = QTF.Status.Active,
                    //Arguments = arguments
                });

                //testListeners.Add(new TestListenerDescriptor()
                //{
                //    Name = "TestListener2"
                //});

                QTF.TestProperties.SetPropertyValue("TestListeners", testListeners);

                QTF.TestProperties.Save(@"c:\temp\myproperties.props");

                //QTF.TestProperties.SetProperty("TestListeners", new TestListenerCollection());



                //QTF.TestProperties.Initialize(@"c:\temp\myproperties.props");



                //int i = 1;
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
        #endregion
        /*
        private void emitProperty(
    TypeBuilder tb,
    FieldBuilder hash,
    Setting s,
    string name)
        {
            //to figure out what opcodes to emit, i would compile a small class 
            //having the functionality i wanted, and viewed it with ildasm. 
            //peverify is also kinda nice to use to see what errors there are. 

            //define the property first 
            PropertyBuilder pb = tb.DefineProperty(
                name,
                PropertyAttributes.None,
                s.Value.GetType(),
                new Type[] { });
            Type objType = s.Value.GetType();

            //define the get method for the property 
            MethodBuilder getMethod = tb.DefineMethod(
                "get_" + name,
                MethodAttributes.Public,
                objType,
                new Type[] { });

            ILGenerator ilg = getMethod.GetILGenerator();
            ilg.DeclareLocal(objType);
            ilg.Emit(OpCodes.Ldarg_0);
            ilg.Emit(OpCodes.Ldfld, hash);
            ilg.Emit(OpCodes.Ldstr, name);
            ilg.EmitCall(OpCodes.Callvirt,
                typeof(Hashtable).GetMethod("get_Item"),
                null);

            if (objType.IsValueType)
            {
                ilg.Emit(OpCodes.Unbox, objType);
                if (typeHash[objType] != null)
                    ilg.Emit((OpCode)typeHash[objType]);
                else ilg.Emit(OpCodes.Ldobj, objType);
            }
            else
                ilg.Emit(OpCodes.Castclass, objType);

            ilg.Emit(OpCodes.Stloc_0);
            ilg.Emit(OpCodes.Br_S, (byte)0);
            ilg.Emit(OpCodes.Ldloc_0);
            ilg.Emit(OpCodes.Ret);

            //now we generate the set method for the property 
            MethodBuilder setMethod = tb.DefineMethod(
                "set_" + name,
                MethodAttributes.Public,
                null,
                new Type[] { objType });

            ilg = setMethod.GetILGenerator();
            ilg.Emit(OpCodes.Ldarg_0);
            ilg.Emit(OpCodes.Ldfld, hash);
            ilg.Emit(OpCodes.Ldstr, name);
            ilg.Emit(OpCodes.Ldarg_1);

            if (objType.IsValueType)
                ilg.Emit(OpCodes.Box, objType);
            ilg.EmitCall(OpCodes.Callvirt,
                typeof(Hashtable).GetMethod("set_Item"),
                null);

            ilg.Emit(OpCodes.Ret);

            //put the get/set methods in with the property 
            pb.SetGetMethod(getMethod);
            pb.SetSetMethod(setMethod);
        }
*/
        #region Private methods

        #endregion

        #region Protected methods

        protected override void Setup()
        {
            base.Setup();
        }

        protected override void Teardown()
        {
            base.Teardown();
        }

        #endregion
    }
}
