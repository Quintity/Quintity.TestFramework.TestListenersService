using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Reflection;
using System.Collections.Generic;
using log4net;
using Quintity.TestFramework.Core;
using Quintity.TestFramework.Runtime;
using System.Text;

namespace Quintity.TestFramework.TestListenersService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
     IncludeExceptionDetailInFaults = true, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class ListenerEvents : IListenerEvents
    {
        //public delegate void TestListenersCompleteHandler(TerminationReason terminationReason, string message);
        //public static event TestListenersCompleteHandler OnTestListenersComplete;

        //internal static void fireTestListenersCompleteEvent(TerminationReason terminationReason, string message)
        //{
        //    OnTestListenersComplete?.Invoke(terminationReason, message);
        //}

        #region Data members

        private readonly ILog logEvent;

        private Stopwatch stopWatch;

        private OperationContext operationContext = null;
        private List<TestListenerDescriptor> originalListeners;
        private TestProfile originalProfile;
        private int virtualUsers;
        private Dictionary<string, VirtualUserEventsHandler> virtualUserEventHandlers;

        public static Dictionary<int, TestListener> ListenerTypeDictionary;

        #endregion

        #region Constructors

        public ListenerEvents() : base() => logEvent = LogManager.GetLogger(typeof(ListenerEvents));

        #endregion

        #region Service interface implementations

        public bool ServiceAvailability() => true;

        public int TestMethod(string testString)
        {
            logEvent.Info($"Test string:  {testString}");

            return testString.Length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="testListeners"></param>
        /// <param name="testProfile"></param>
        /// <returns>Returns the number of qualified listeners.</returns>
        public int InitializeService(List<TestListenerDescriptor> testListeners, TestProfile testProfile)
        {
            // Saving operational context for callback posting (monitoring runs on separate thread so context not available during processing).
            operationContext = OperationContext.Current;

            // Preserve for later reference.
            originalListeners = testListeners;
            originalProfile = testProfile;

            // Qualify/validate listeners
            var qualifiedListeners = getQualifiedActiveListeners(testListeners);
            virtualUsers = testProfile.VirtualUsers;

            if (qualifiedListeners > 0)
            {
                string listenerText = $"Active listeners ({qualifiedListeners}):  ";

                foreach (var listener in ActiveListeners)
                {
                    listenerText += $"\"{listener.Name}\"  ";
                }

                logEvent.Info($"{listenerText}");
#if DEBUG
                stopWatch = new Stopwatch();
                stopWatch.Start();
#endif
               // VirtualUserEventsHandler.OnVirtualUserTestListenerEventsHandlerComplete += OnVirtualUserEventsHandlerComplete;
                virtualUserEventHandlers = new Dictionary<string, VirtualUserEventsHandler>();
            }

            return qualifiedListeners;
        }

        private object isDoneLock = new object();

        private void OnVirtualUserEventsHandlerComplete(string virtualUser, VirtualUserEventsHandlerCompleteArgs args)
        {
            logEvent.Info($"OnVirtualUserEventsHandlerComplete event received ({virtualUser})");

            lock (isDoneLock)
            {
                if (args.StopAll)
                {
                    virtualUserEventHandlers.Clear();
                }
                else
                {
                    virtualUsers--;

                    virtualUserEventHandlers.Remove(virtualUser);

                    if (virtualUsers <= 0)
                    {
#if DEBUG
                        stopWatch.Stop();
                        TimeSpan ts = stopWatch.Elapsed;
                        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                        logEvent.Debug($"Total listeners processing elapsed time:  {elapsedTime} ({virtualUser})");
#endif
                        // Removed handler until next iteration.
                        VirtualUserEventsHandler.OnVirtualUserTestListenerEventsHandlerComplete -= OnVirtualUserEventsHandlerComplete;
                    }
                }
            }
        }


        public void OnTestExecutionBegin(TestExecutionBeginArgs args) => 
            queueVirtualUserTestListenerEvent(args.VirtualUser, MethodInfo.GetCurrentMethod().Name, null, args);

        public void OnTestExecutionComplete(TestExecutionCompleteArgs args) => 
            queueVirtualUserTestListenerEvent(args.VirtualUser, MethodInfo.GetCurrentMethod().Name, null, args);

        public void OnTestCaseExecutionBegin(TestCase testCase, TestCaseBeginExecutionArgs args) => 
            queueVirtualUserTestListenerEvent(args.VirtualUser, MethodInfo.GetCurrentMethod().Name, testCase, args);

        public void OnTestCaseExecutionComplete(TestCase testCase, TestCaseResult testCaseResult) => 
            queueVirtualUserTestListenerEvent(testCaseResult.VirtualUser, MethodInfo.GetCurrentMethod().Name, testCase, testCaseResult);

        public void OnTestCheck(TestCheck testCheck) => 
            queueVirtualUserTestListenerEvent(testCheck.VirtualUser, MethodInfo.GetCurrentMethod().Name, testCheck);

        public void OnTestMetric(string virtualUser, TestMetricEventArgs args) => 
            queueVirtualUserTestListenerEvent(virtualUser, MethodInfo.GetCurrentMethod().Name, virtualUser, args);

        public void OnTestPostprocessorBegin(TestSuite testSuite, TestProcessorBeginExecutionArgs args) => 
            queueVirtualUserTestListenerEvent(args.VirtualUser, MethodInfo.GetCurrentMethod().Name, testSuite, args);

        public void OnTestPostprocessorComplete(TestSuite testSuite, TestProcessorResult testProcessorResult) => 
            queueVirtualUserTestListenerEvent(testProcessorResult.VirtualUser, MethodInfo.GetCurrentMethod().Name, testSuite, testProcessorResult);

        public void OnTestPreprocessorBegin(TestSuite testSuite, TestProcessorBeginExecutionArgs args) => 
            queueVirtualUserTestListenerEvent(args.VirtualUser, MethodInfo.GetCurrentMethod().Name, testSuite, args);

        public void OnTestPreprocessorComplete(TestSuite testSuite, TestProcessorResult testProcessorResult) => 
            queueVirtualUserTestListenerEvent(testProcessorResult.VirtualUser, MethodInfo.GetCurrentMethod().Name, testSuite, testProcessorResult);

        public void OnTestStepExecutionBegin(TestStep testStep, TestStepBeginExecutionArgs args) => 
            queueVirtualUserTestListenerEvent(args.VirtualUser, MethodInfo.GetCurrentMethod().Name, testStep, args);

        public void OnTestStepExecutionComplete(TestStep testStep, TestStepResult testStepResult) => 
            queueVirtualUserTestListenerEvent(testStepResult.VirtualUser, MethodInfo.GetCurrentMethod().Name, testStep, testStepResult);

        public void OnTestSuiteExecutionBegin(TestSuite testSuite, TestSuiteBeginExecutionArgs args) => 
            queueVirtualUserTestListenerEvent(args.VirtualUser, MethodInfo.GetCurrentMethod().Name, testSuite, args);

        public void OnTestSuiteExecutionComplete(TestSuite testSuite, TestSuiteResult testSuiteResult) => 
            queueVirtualUserTestListenerEvent(testSuiteResult.VirtualUser, MethodInfo.GetCurrentMethod().Name, testSuite, testSuiteResult);

        public void OnTestTrace(string virtualUser, string traceMessage) => 
            queueVirtualUserTestListenerEvent(virtualUser, MethodInfo.GetCurrentMethod().Name, virtualUser, traceMessage);

        public void OnTestWarning(TestWarning testWarning) => 
            queueVirtualUserTestListenerEvent(testWarning.VirtualUser, MethodInfo.GetCurrentMethod().Name, testWarning);

        #endregion

        /// <summary>
        /// This method initializes active and complete listeners.  It
        /// 1.  Iterates through each listener in listener descriptor collection, checking for 
        /// those marked active and are definitially complete.
        /// 2.  Verifies that the listener is correctly derived from the core library "Listener" abstract class.
        /// 3.  Assuming all the above, the listener type (class) is instantiated and added to dictionary indexed by hash code.
        /// The goal is to reference the instantiated class for each runtime event to save time and to maintain
        /// class data members between event handlers.
        /// </summary>
        /// <returns>Returns the number of active listeners.</returns>
        private int getQualifiedActiveListeners(List<TestListenerDescriptor> testListeners)
        {
            ActiveListeners = new List<TestListenerDescriptor>();

            ListenerTypeDictionary = new Dictionary<int, TestListener>();

            // Iterate through each test listener descriptor in collection.
            foreach (TestListenerDescriptor testListener in testListeners)
            {
                try
                {
                    // Verify the status and completeness of test listener
                    if (testListener.Status == Status.Active)
                    {
                        if (isComplete(testListener))
                        {
                            // Load assembly and look for named listener type (class) within loaded assembly.
                            Assembly listenerAssembly = Assembly.LoadFrom(testListener.Assembly);
                            Type listenerType = listenerAssembly.GetType(testListener.Type);

                            if (listenerType != null)
                            {
                                // Verify it is an actual core.listener derived class.
                                if (TestListener.IsTestListenerType(listenerType))
                                {
                                    // Verify listener instance can be created/accessed.
                                    var instance = Activator.CreateInstance(listenerType, testListener.Parameters) as TestListener;

                                    // Track only those listeners that have been successfully loaded and listener instance createds.
                                    ActiveListeners.Add(testListener);
                                }
                                else // Not a TestListener derived class
                                {
                                    initializationErrorHandler(testListener, $"The listener type {testListener.Type} must be derived from the TestListener base class.");
                                }
                            }
                            else  // Unable to find type.
                            {
                                initializationErrorHandler(testListener, $"Unable to find type \"{testListener.Type}\" in listener assembly \"{testListener.Assembly}\"");
                            }
                        }
                        else  // Incomplete descriptor definition.
                        {
                            initializationErrorHandler(testListener, "The configuration is set to active, however it's runtime information is incomplete.");
                        }
                    }
                }
                catch (System.IO.FileNotFoundException e)
                {
                    initializationErrorHandler(testListener, e.Message);
                }
            }

            return ActiveListeners.Count;
        }

        private void initializationErrorHandler(TestListenerDescriptor testListener, string message)
        {
            var state = testListener.OnFailure.ToString().ToLower();

            logEvent.Error($"Test listener (\"{testListener.Name}\") initialization error:  {message}{Environment.NewLine}" +
                $"Test execution is configured to {state} upon a listener failure.");

            if (testListener.OnFailure == OnFailure.Stop)
            {
                // TODO - Callback to stop processing.
            }
        }

        public static List<TestListenerDescriptor> ActiveListeners;

        public static List<TestListenerDescriptor> GetActiveListeners() => ActiveListeners.FindAll(x => x.Status == Status.Active);

        public static bool AreActiveListeners() => GetActiveListeners().Count > 0 ? true : false;

        public static TestListenerDescriptor GetListenerDescriptor(List<TestListenerDescriptor> testListeners, string assembly, string type) =>
         testListeners.Find(x => x.Assembly.Contains("") && x.Type.Equals(type));

        private TestListenerDescriptor getListenerDescriptor(List<TestListenerDescriptor> testListeners, string assembly, string type) =>
          testListeners.Find(x => x.Assembly.Contains("") && x.Type.Equals(type));

        private bool isComplete(TestListenerDescriptor testListener) => (!string.IsNullOrEmpty(testListener.Assembly) &&
            !string.IsNullOrEmpty(testListener.Type)) ? true : false;

        private object eventLock = new object();

        private void queueVirtualUserTestListenerEvent(string virtualUser, string method, params object[] parameters)
        {
            if (method.Equals("OnTestExecutionComplete"))
            {
                int i = 1;
            }
            // Create the listener event for queueing.
            var listenerEvent = new TestListenerEvent(virtualUser, method, parameters);

            lock (eventLock)
            {
                if (virtualUser != null)
                {
                    if (!virtualUserEventHandlers.TryGetValue(virtualUser, out VirtualUserEventsHandler virtualUserEventsHandler))
                    {
                        virtualUserEventsHandler = new VirtualUserEventsHandler(virtualUser);
                        virtualUserEventHandlers.Add(virtualUser, virtualUserEventsHandler);

                        logEvent.Info($"VirtualUserEventsHandler created for ({virtualUser}) ({virtualUserEventHandlers.Count})");
                    }

                    virtualUserEventsHandler.EnqueueListenerEvent(listenerEvent);
                }

                logEvent.Debug($"Listener event \"{method}\" ({virtualUser}) enqueued.");
            }
        }

        private void fireTestListenersCompleteCallback(TerminationReason terminationReason, string explanation)
        {
            try
            {
                logEvent.Info("Post TestListenersCompleteNotification to client.");

                IListenerEventsCallbacks clientChannel = operationContext.GetCallbackChannel<IListenerEventsCallbacks>();

                if (clientChannel != null)
                {
                    var args = new TestListenersCompleteArgs()
                    {
                        TerminationSource = terminationReason,
                        Explanation = explanation,
                        TestProfile = originalProfile
                    };

                    clientChannel.TestListenersCompleteNotification(originalListeners, args);

                    logEvent.Info("TestListenersCompleteNotification posted to client.");
                }
            }
            catch (CommunicationObjectAbortedException e)
            {
                // Client channel has timed out.
                logEvent.Fatal(e.ToString());
            }
        }
    }
}
