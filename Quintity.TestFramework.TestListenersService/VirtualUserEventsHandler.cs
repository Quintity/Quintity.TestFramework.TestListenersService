using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Quintity.TestFramework.Core;
using Quintity.TestFramework.Runtime;
using log4net;

namespace Quintity.TestFramework.TestListenersService
{
    internal class VirtualUserEventsHandlerCompleteArgs
    {
        public TerminationReason TerminationReason
        { get; set; }

        public string Explanation
        { get; set; }

        public bool StopAll
        { get; set; }
    }

    internal class VirtualUserEventsHandler
    {
        #region Class event definitions

        public delegate void VirtualUserEventsHandlerComplete(string virtualUser, VirtualUserEventsHandlerCompleteArgs args);
        public static event VirtualUserEventsHandlerComplete OnVirtualUserTestListenerEventsHandlerComplete;

        internal static void fireVirtualUserTestListenerEventsHandlerCompleteEvent(string virtualUser, VirtualUserEventsHandlerCompleteArgs args)
        {
            OnVirtualUserTestListenerEventsHandlerComplete?.Invoke(virtualUser, args);
        }

        #endregion

        public string VirtualUser
        { get; set; }

        private Dictionary<int, TestListener> listenerInstances;
        private readonly ILog logEvent;
        private Queue<TestListenerEvent> listenerEventQueue;
        private Timer queueMonitoryTimer;
        private int queueMonitorDueTime = 0;
        private int queueMonitoryPeriod = 1000;

        public VirtualUserEventsHandler(string virtualUser)
        {
            VirtualUser = virtualUser;

            logEvent = LogManager.GetLogger(typeof(VirtualUserEventsHandler));

            listenerEventQueue = new Queue<TestListenerEvent>();
            listenerInstances = new Dictionary<int, TestListener>();

            TimerCallback queueMonitoryCallback = new TimerCallback(queueMonitorDelegate);
            queueMonitoryTimer = new Timer(queueMonitoryCallback, null, queueMonitorDueTime, queueMonitoryPeriod);
        }

        public void EnqueueListenerEvent(TestListenerEvent listenerEvent) => listenerEventQueue.Enqueue(listenerEvent);

        private Object fireLock = new Object();

        private void queueMonitorDelegate(object stateInfo)
        {
            TestListenerEvent listenerEvent = null;
            var terminationReason = TerminationReason.Normal;
            var terminationExplanation = "Test listener execution complete.";

            try
            {
                if (listenerEventQueue.Count > 0)
                {
                    stopQueueMonitor();

                    while (true)
                    {
                        listenerEvent = listenerEventQueue.Dequeue();
                        logEvent.Debug($"Listener event \"{listenerEvent.Method}\" ({VirtualUser}) dequeued.");

                        if (!invokeListeners(listenerEvent))
                        {
                            var message = "Test listeners processing stopped due to listener error (see logs for more information)";

                            fireVirtualUserTestListenerEventsHandlerCompleteEvent(VirtualUser, new VirtualUserEventsHandlerCompleteArgs()
                            {
                                TerminationReason = TerminationReason.ListenerError,
                                Explanation = message,
                                StopAll = true
                            });

                            logEvent.Info(message);

                            break;
                        }

                        if (listenerEvent.Method.Equals("OnTestExecutionComplete"))
                        {
                            stopQueueMonitor();

                            logEvent.Info($"Firing VirtualUserTestListenerCompleteEvent {VirtualUser}.");

                            lock (fireLock)
                            {
                                fireVirtualUserTestListenerEventsHandlerCompleteEvent(VirtualUser, new VirtualUserEventsHandlerCompleteArgs()
                                {
                                    TerminationReason = terminationReason,
                                    Explanation = "Test listener execution complete.",
                                    StopAll = false
                                });
                            }

                            logEvent.Info($"VirtualUserTestListenerCompleteEvent fired {VirtualUser} ({listenerEventQueue.Count}).");
                        }
                    }
                }
            }
            catch (InvalidOperationException)
            {
                startQueueMonitor();
            }
            catch (Exception e)
            {
                terminationReason = TerminationReason.ListenerError;
                terminationExplanation = e.ToString();
                startQueueMonitor();
            }
        }

        private bool invokeListeners(TestListenerEvent testListenerEvent)
        {
            var @continue = true;

            var activeTestListeners = ListenerEvents.GetActiveListeners();

            try
            {
#if DEBUG
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
#endif
                var taskList = new List<Task>();

                foreach (var testListener in activeTestListeners)
                {
                    taskList.Add(Task.Factory.StartNew(() => invokeListener(testListener, testListenerEvent.Method, testListenerEvent.Parameters)));
                }

                Task.WaitAll(taskList.ToArray());

#if DEBUG
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                logEvent.Debug($"Virtual user:  {testListenerEvent.VirtualUser}, listener event \"{testListenerEvent.Method}\" elapsed time:  {elapsedTime}");
#endif
            }
            catch (AggregateException e)
            {
                foreach (var exception in e.InnerExceptions)
                {
                    for (int i = 0; i < e.InnerExceptions.Count; i++)
                    {
                        var testListener = ListenerEvents.GetListenerDescriptor(activeTestListeners,
                            e.InnerExceptions[i].InnerException.TargetSite.DeclaringType.Module.Name,
                            e.InnerExceptions[i].InnerException.TargetSite.DeclaringType.FullName);

                        if (testListener.OnFailure == OnFailure.Continue)
                        {
                            testListener.Status = Status.Inactive;

                            logEvent.Error($"Test listener \"{testListener.Name}\" runtime exception.  Test execution continues with this listener set to inactive.");

                            if (ListenerEvents.GetActiveListeners().Count == 0)
                            {
                                @continue = false;
                                logEvent.Error($"Test listeners runtime exception.  Test execution cancelled as there are no longer any active listeners");
                            }
                        }
                        else if (testListener.OnFailure == OnFailure.Stop)
                        {
                            @continue = false;
                            logEvent.Error($"Test listener \"{testListener.Name}\" runtime exception.  Test execution canceled (per listener configuration).");
                            break;
                        }
                    }
                }
            }

            return @continue;
        }

        private Object getInstanceLock = new Object();

        private void invokeListener(TestListenerDescriptor testListenerDescriptor, string method, params object[] parameters)
        {
            TestListener testListenerInstance = null;

            var testScriptObject = parameters[0] is TestScriptObject ? (TestScriptObject)parameters[0] : null;

            lock (getInstanceLock)
            {
                testListenerInstance = getListenerInstance(testListenerDescriptor);
            }

            testListenerInstance.VirtualUser = VirtualUser;
            Type type = testListenerInstance.GetType();
            MethodInfo methodInfo = type.GetMethod(method);

            // If method does not have the NoOperation attribute applied, continue
            if (isOperable(methodInfo))
            {
                if (testScriptObject is null)
                {
                    logEvent.Info($"Invoking listener \"{testListenerDescriptor.Name}\" event handler \"{method}\" ({VirtualUser})");
                }
                else
                {
                    logEvent.Info(
                        $"Invoking listener \"{testListenerDescriptor.Name}\" event handler \"{method}\":  \"{testScriptObject.Title}\" ({VirtualUser})");
                }
               
                methodInfo.Invoke(testListenerInstance, parameters);
            }
        }

        private bool isOperable(MethodInfo methodInfo)
        {
            return methodInfo.GetCustomAttributes(typeof(NoOperationAttribute), true).Length == 0 ? true : false;
        }

        private TestListener getListenerInstance(TestListenerDescriptor testListenerDescriptor)
        {
            TestListener listenerInstance;

            if (!listenerInstances.TryGetValue(testListenerDescriptor.GetHashCode(), out listenerInstance))
            {
                Assembly listenerAssembly = Assembly.LoadFrom(testListenerDescriptor.Assembly);
                Type listenerType = listenerAssembly.GetType(testListenerDescriptor.Type);
                listenerInstance = Activator.CreateInstance(listenerType, testListenerDescriptor.Parameters) as TestListener;
                listenerInstances.Add(testListenerDescriptor.GetHashCode(), listenerInstance);
            }

            return listenerInstance;
        }

        private void startQueueMonitor() => queueMonitoryTimer.Change(queueMonitoryPeriod, queueMonitoryPeriod);

        private void stopQueueMonitor() => queueMonitoryTimer.Change(Timeout.Infinite, 0);
    }
}
