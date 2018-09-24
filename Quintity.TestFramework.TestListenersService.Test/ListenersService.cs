using System;
using System.ServiceModel;
using Quintity.TestFramework.Core;
using Quintity.TestFramework.TestListenersService.Tests;


namespace Quintity.TestFramework.TestListenersService.Tests
{
    [TestClass]
    public class TestMethods : TestClassBase
    {
        [TestMethod]
        public TestVerdict ListenersService()
        {
            try
            {


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

        //private ListenerEventsClient getListenerEventsClient()
        //{
        //    NetTcpBinding binding = new NetTcpBinding();
        //    //binding.Security.Mode = SecurityMode.None;
        //    binding.ReceiveTimeout = TimeSpan.FromDays(7);
        //    binding.SendTimeout = TimeSpan.FromDays(7);

        //    InstanceContext context = new InstanceContext(this);

        //    TestListenersService.Tests.TestMethods.



        //    EndpointAddress endPoint = new EndpointAddress("net.tcp://localhost:10101//Quintity.TestFramework.TestListenersService");



        //    ListenerEventsClient proxy = new ListenerEventsClient(binding, endPoint);

        //    return proxy;
        //    //return new ListenerEventsClient("net.tcp://localhost:10101//Quintity.TestFramework.TestListenersService");
        //    // return new ListenersService.ListenerEventsClient(new InstanceContext(this));
        //    //return new ListenerEventsClient();
        //}
    }
}