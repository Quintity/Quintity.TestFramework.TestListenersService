using System;
using System.Threading;
using System.ServiceModel;
using System.Collections.Generic;
using Quintity.TestFramework.Core;
using Quintity.TestFramework.Runtime;
using Quintity.TestFramework.ListenersService;

namespace Quintity.TestFramework.ListenersService.Tests
{
    [TestClass]
    public class TestMethods : TestClassBase
    {
        [TestMethod]
        public TestVerdict ScratchMethod()
        {
            try
            {
                //using (var testMetric = new TestMetric("9999", "Test metric description"))
                //{
                //    testMetric.Start();
                //    Thread.Sleep(5000);
                //    testMetric.Stop();
                //};

                var testCheck = TestCheck.IsTrue("This is the first a sample test check", false, "I expected this to fail");
                testCheck = TestCheck.IsTrue("This is the second a sample test check", true, "I expected this to pass.");
                testCheck = TestCheck.IsTrue("This is the third a sample test check", false, $"Expected:  Pass, Actual:  Fail.");

                TestWarning.IsTrue(false, "This is a sample test warning.");

                TestTrace.Trace("This is a sample trace message");

                Thread.Sleep(5000);

                TestMessage += "Completed!";
                TestVerdict = TestVerdict.Pass;
            }
            catch (Exception e)
            {
                TestMessage += e.ToString();
                TestVerdict = TestVerdict.Error;
            }
            finally
            {

            }

            return TestVerdict;
        }

        [TestMethod]
        public TestVerdict TestListeners()
        {
            try
            {
                var testListeners = new TestListenerCollection();

                var parameters = new Dictionary<string, string>();
                parameters.Add("Param1", "Value1");
                parameters.Add("Param2", "Value2");

                var testListener = new TestListenerDescriptor()
                {
                    Name = "My listener",
                    Description = "My description",
                    Assembly = @"C:\temp\library",
                    OnFailure = OnFailure.Stop,
                    Status = Status.Inactive,
                    Parameters = parameters

                };

                testListeners.Add(testListener);

                var property = new TestProperty("TestListeners", "description", @"C:\temp", true);
                TestProperties.AddProperty(property);

                TestListenerCollection.SerializeToFile(testListeners, @"c:\temp\testlisteners.confg");

                var bob = TestListenerCollection.DeserializeFromFile(@"c:\temp\testlisteners.confg");

                TestMessage += "Success";
                TestVerdict = TestVerdict.Pass;
            }
            catch (Exception e)
            {
                TestMessage += e.ToString();
                TestVerdict = TestVerdict.Error;
            }
            finally
            {

            }

            return TestVerdict;
        }

        [TestMethod]
        public TestVerdict Dictionary()
        {
            try
            {
                var parameters = new Dictionary<string, string>();
                parameters.Add("Param1", "Value1");
                parameters.Add("Param2", "Value2");

                foreach (var item in parameters)
                {
                    TestTrace.Trace($"Key:  {item.Key}, Value:  {item.Value}");
                }

                TestMessage += "Success";
                TestVerdict = TestVerdict.Pass;
            }
            catch (Exception e)
            {
                TestMessage += e.ToString();
                TestVerdict = TestVerdict.Error;
            }
            finally
            {

            }

            return TestVerdict;
        }

        [TestMethod]
        public TestVerdict RandomTimeout(
            int minTime,
            int maxTime)
        {
            try
            {
                Random rnd = new Random();

                var millisecs = rnd.Next(minTime, maxTime);

                var testMessage = $"Virtual user:  {GetCurrentUser()} random timeout:  {millisecs} millseconds.";
                TestTrace.Trace(testMessage);

                Thread.Sleep(millisecs);

                TestMessage += $"{GetCurrentUser()}:  Success!";
                TestVerdict = TestVerdict.Pass;
            }
            catch (Exception e)
            {
                TestMessage += e.ToString();
                TestVerdict = TestVerdict.Error;
            }
            finally
            {

            }

            return TestVerdict;
        }

        //public TestVerdict TestListenersTest()
        //{

        //}

        //private void getRuntimeClient()
        //{
        //    NetTcpBinding binding = new NetTcpBinding();
        //    //binding.ReceiveTimeout = TimeSpan.FromDays(7);
        //    //binding.SendTimeout = TimeSpan.FromDays(7);
        //    //binding.MaxReceivedMessageSize = 1048575;
        //    //binding.MaxBufferSize = 1048575;
        //    //binding.ReaderQuotas.MaxStringContentLength = 1048575;

        //    //InstanceContext context = new InstanceContext(new OperationsCallbacks());
        //    //InstanceContext context = new InstanceContext(new ServiceHostBase("localhost"));
        //    //var bob = new TestListenersService.ListenerEventsClient((binding,
        //    //    new EndpointAddress("net.tcp://localhost:10101//Quintity.TestFramework.TestListenersService/"));

        //    return new TestListenerService.ListenerEventsClient(binding,
        //        new EndpointAddress("net.tcp://localhost:10101//Quintity.TestFramework.TestListenersService/"));
        //}
    }
}
