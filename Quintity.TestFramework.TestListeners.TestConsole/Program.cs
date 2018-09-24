using System;
using System.ServiceModel;
using System.Text;
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
                var fred = new string('x', 65200);

                var length = client.TestMethod(fred);

                var available = client.ServiceAvailability();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

            }
            finally
            {
                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
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
