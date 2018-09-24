using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Quintity.TestFramework.ListenersService.Host
{
    public class Common
    {
        internal static System.ServiceModel.ServiceHost _serviceHost = null;

        internal static void StartWebService()
        {
            try
            {
                startService();
            }
            catch (Exception e)
            {
                Program.LogEvent.Fatal("Unhandled exception.", e);
            }
            finally
            { }
        }

        internal static void startService()
        {
            var uri = new Uri("net.tcp://localhost:10101//Quintity.TestFramework.TestListenersService/");
            var _serviceHost = new ServiceHost(typeof(TestListenersService.ListenerEvents));
            var netTcpBinding = new NetTcpBinding();

            netTcpBinding.MaxReceivedMessageSize = int.MaxValue;
            netTcpBinding.ReceiveTimeout = TimeSpan.FromDays(7);
            netTcpBinding.SendTimeout = TimeSpan.FromDays(7);
            netTcpBinding.MaxBufferPoolSize = int.MaxValue; ;
            netTcpBinding.MaxBufferSize = int.MaxValue; ;
            netTcpBinding.MaxReceivedMessageSize = int.MaxValue; ;
            netTcpBinding.ReaderQuotas.MaxArrayLength = int.MaxValue; ;
            netTcpBinding.ReaderQuotas.MaxStringContentLength = int.MaxValue;

            _serviceHost.AddServiceEndpoint(typeof(Quintity.TestFramework.TestListenersService.IListenerEvents),
                netTcpBinding, "net.tcp://localhost:10101//Quintity.TestFramework.TestListenersService/");

            //_serviceHost = new System.ServiceModel.ServiceHost(typeof(Quintity.TestFramework.TestListenersService.ListenerEvents),
            //    new Uri("net.tcp://localhost:10101//Quintity.TestFramework.TestListenersService/"));

            // Check to see if the service host already has a ServiceMetadataBehavior
            ServiceMetadataBehavior smb = _serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();

            // If not, add one
            //if (smb == null)
            //{
            //    smb = new ServiceMetadataBehavior();
            //}

            //_host.Faulted += new EventHandler(_host_Faulted);

            _serviceHost.Open();
        }
    }
}
