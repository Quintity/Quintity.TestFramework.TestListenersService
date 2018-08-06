using System.Collections.Generic;
using System.Threading;
using System.Reflection;
using Quintity.TestFramework.Core;
using Quintity.TestFramework.Runtime;

namespace Quintity.TestFramework.SampleListeners
{

    public class ScratchListener3 : TestListener
    {
        private static readonly log4net.ILog LogEvent =
          log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ScratchListener3() { }

        public ScratchListener3(Dictionary<string, string> args) { }

        public override void OnTestExecutionBegin(TestExecutor testExecutor, TestExecutionBeginArgs args)
        {
            int time = 1;
            Thread.Sleep(time * 1000);
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {args.VirtualUser}, sleep time: {time}");
        }

        public override void OnTestExecutionComplete(TestExecutor testExecutor, TestExecutionCompleteArgs args)
        {
            int time = 1;
            Thread.Sleep(time * 1000);
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {VirtualUser}, sleep time: {time}");
        }

        public override void OnTestCaseExecutionBegin(TestCase testCase, TestCaseBeginExecutionArgs args)
        {
            int time = 1;
            Thread.Sleep(time * 1000);
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {args.VirtualUser}, sleep time: {time}");

        }

        public override void OnTestCaseExecutionComplete(TestCase testCase, TestCaseResult testCaseResult)
        {
            int time = 1;
            Thread.Sleep(time * 1000);
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {testCaseResult.VirtualUser}, sleep time: {time}");

        }

        public override void OnTestStepExecutionBegin(TestStep testStep, TestStepBeginExecutionArgs args)
        {
            //throw new InvalidOperationException();

            int time = 1;
            Thread.Sleep(time * 1000);
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {args.VirtualUser}, sleep time: {time} seconds");

        }

        public override void OnTestStepExecutionComplete(TestStep testStep, TestStepResult testStepResult)
        {
            int time = 1;
            Thread.Sleep(time * 1000);
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {testStepResult.VirtualUser}, sleep time: {time} seconds");
        }

        public override void OnTestSuiteExecutionBegin(TestSuite testSuite, TestSuiteBeginExecutionArgs args)
        {
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {args.VirtualUser}");
        }

        public override void OnTestSuiteExecutionComplete(TestSuite testSuite, TestSuiteResult testSuiteResult)
        {
            int time = 1;
            Thread.Sleep(time * 1000);
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} Scratch:  {testSuiteResult.VirtualUser}, sleep time: {time}");

        }

        #region Not used

        public override void OnTestMetric(string virtualUser, TestMetricEventArgs args) { }

        public override void OnTestPostprocessorBegin(TestSuite testSuite, TestProcessorBeginExecutionArgs args) { }

        public override void OnTestPostprocessorComplete(TestSuite testSuite, TestProcessorResult testProcessorResult) { }

        public override void OnTestPreprocessorBegin(TestSuite testSuite, TestProcessorBeginExecutionArgs args) { }

        public override void OnTestPreprocessorComplete(TestSuite testSuite, TestProcessorResult testProcessorResult) { }

        public override void OnTestTrace(string virtualString, string traceMessage) { }

        public override void OnTestWarning(TestWarning testWarning) { }

        public override void OnTestCheck(TestCheck testCheck) { }

        #endregion
    }
}
