using System;
using System.ServiceProcess;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quintity.TestFramework.ListenersService.Host
{
    public class WinService : ServiceBase
    {
        public WinService()
        { }

        protected override void OnStart(string[] args)
        {
            try
            {
                Common.StartWebService();
                Program.LogEvent.Info("Quintity.TestFramework.ListenersService started.");
            }
            catch (Exception e)
            {
                Program.LogEvent.Fatal("Unhandled exception.", e);
                Stop();
            }
        }

        protected override void OnStop()
        {
            Program.LogEvent.Info("Quintity.TestFramework.ListenersService stopped.");
        }
    }
}
