using System;
using System.Threading;
using System.ServiceProcess;
using Quintity.TestFramework.Core;
using Quintity.TestFramework.TestListenersService;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Quintity.TestFramework.ListenersService.Host
{
    class Program
    {
        internal static readonly log4net.ILog LogEvent =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static ManualResetEvent manualReset = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += OnUnhandledException;

           // ListenerEvents.OnTestListenersComplete += ListenerEvents_OnTestListenersComplete;

            string @switch = args.Length > 0 ? args[0].ToUpper().Trim().Substring(0, 2) : null;

            try
            {
                if (null != @switch && "/S" == @switch.ToUpper())
                {
                    runAsWindowsService();

                }
                else
                {
                    runAsConsoleApp();
                }
            }
            catch (Exception e)
            {
                LogEvent.Fatal("A fatal error has occurred", e);
            }
        }

        private static void ListenerEvents_OnTestListenersComplete(TerminationReason terminationReason, string message)
        {
            LogEvent.Info(message: $"TestListener processing complete.  {message} ({terminationReason})");
            manualReset.Set();
        }

        private static void runAsWindowsService()
        {
            ServiceBase.Run(new WinService());
        }

        private static void runAsConsoleApp()
        {
            var consoleApp = new ConsoleApp();
            LogEvent.Info(string.Format("Quintity.TestFramework.ListenersService started."));

            // Begin processing queued journal entries
            consoleApp.Run();

            //LogEvent.Info(string.Format("The Quintity ListenersService is running (\"{0}\").\n",
            //        Common._serviceHost.Description.Endpoints[0].Address));

            manualReset.WaitOne();

            LogEvent.Info(string.Format("Quintity.TestFramework.ListenersService exiting."));
            Thread.Sleep(1000);
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogEvent.Fatal("An unhandled exception has occurred.", (Exception)e.ExceptionObject);
        }
    }
}
