using Quintity.TestFramework.Core;
using Quintity.TestFramework.Runtime;
using System;
using System.ServiceModel;

namespace Quintity.TestFramework.Service.Tests
{
    [TestClass]
    public class ServiceTests : TestClassBase
    {
        private ListenerEventsClient _listenerEventsClient = null;

        [TestMethod]
        public TestVerdict ListenersService(string listenerConfig)
        {
            try
            {
                Setup();

                var bob = new TestListenerCollection();

                TestListeners.ReadFromFile(listenerConfig);
                var listeners = TestListeners.TestListenerCollection.ToArray();
                
                _listenerEventsClient.InitializeService(listeners, null);

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

        protected override void Setup()
        {
            if (_listenerEventsClient is null)
            {
                _listenerEventsClient = getListenerEventsClient();
            }

            base.Setup();
        }

        private ListenerEventsClient getListenerEventsClient()
        {
            NetTcpBinding binding = new NetTcpBinding();
            //binding.Security.Mode = SecurityMode.None;
            binding.ReceiveTimeout = TimeSpan.FromDays(7);
            binding.SendTimeout = TimeSpan.FromDays(7);

            InstanceContext context = new InstanceContext(this);


            EndpointAddress endPoint = new EndpointAddress("net.tcp://localhost:10101//Quintity.TestFramework.TestListenersService");

            ListenerEventsClient proxy = new ListenerEventsClient(binding, endPoint);

            return proxy;
        }
    }
}
