using System;

namespace Quintity.TestFramework.ListenersService.Host
{
    public class ConsoleApp
    {
        public void Run()
        {
            try
            {
                Common.StartWebService();
            }
            catch (Exception e)
            {
                Program.LogEvent.Fatal("Unhandled exception.", e);
            }
        }
    }
}
