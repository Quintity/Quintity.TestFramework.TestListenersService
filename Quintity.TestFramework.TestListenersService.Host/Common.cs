using System;

namespace Quintity.TestFramework.ListenersService.Host
{
    public class Common
    {
        internal static System.ServiceModel.ServiceHost _host = null;

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
            _host = new System.ServiceModel.ServiceHost(typeof(Quintity.TestFramework.TestListenersService.ListenerEvents),
                new Uri("net.tcp://localhost:10101//Quintity.TestFramework.TestListenersService/"));

            //_host.Faulted += new EventHandler(_host_Faulted);

            _host.Open();
        }
    }
}
