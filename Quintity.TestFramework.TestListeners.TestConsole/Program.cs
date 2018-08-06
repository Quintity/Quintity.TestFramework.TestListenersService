using System;
using System.ServiceModel;
using Quintity.TestFramework.TestListeners.TestConsole.TestListenerService;

namespace Quintity.TestFramework.TestListeners.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var bob = new Bob();
                var client = bob.GetListenerEventsClient();

                // var client = new ListenerEventsClient();

                var available = client.ServiceAvailability();

                var state = client.State;

                var xxx = client.InitializeService(null, null);

                int i = 1;

            }
            catch (Exception e)
            {
                int i = 1;

            }
        }
    }

    public class Bob
    {
        public ListenerEventsClient GetListenerEventsClient()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.ReceiveTimeout = TimeSpan.FromDays(7);
            binding.SendTimeout = TimeSpan.FromDays(7);

            EndpointAddress endPoint = new EndpointAddress("net.tcp://localhost:10101//Quintity.TestFramework.TestListenersService/");
            return new ListenerEventsClient(binding, endPoint);
        }
    }
}
