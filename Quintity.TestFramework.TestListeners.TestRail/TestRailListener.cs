using System;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;
using Quintity.TestFramework.Core;
using Quintity.TestFramework.Runtime;
using TR = TestRail;
using TestRail.Types;
using log4net;
using System.Text;

namespace Quintity.TestFramework.TestListeners.TestRail
{

    public class TestRailListener : TestListener
    {
        private static readonly log4net.ILog LogEvent = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Dictionary<string, string> _parameters;
        TR.TestRailClient _testRailClient = null;
        Project _testRailProject = null;
        Run _testRailRun = null;

        public TestRailListener()
        {
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name}");
        }

        public TestRailListener(Dictionary<string, string> args)
        {
            _parameters = args;
        }

        public override void OnTestExecutionBegin(TestExecutor testExecutor, TestExecutionBeginArgs args)
        {
            LogEvent.Debug($"Listener instance virtual user:  {VirtualUser}");

            // Create TestRail client.
            _testRailClient = new TR.TestRailClient(_parameters["TestRailUrl"],
                _parameters["TestRailUser"], _parameters["TestRailPassword"]);

            _testRailProject = getTestRailProject(_parameters["TestRailProject"]);

            _testRailRun = addTestRailRun(_testRailProject, args.TestScriptObject as TestSuite);

            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {args.VirtualUser}");
        }

        private TR.Types.Project getTestRailProject(string projectName)
        {
            return _testRailClient.GetProjects().Find(x => x.Name.Equals(projectName));
        }

        private Run addTestRailRun(Project testRailProject, TestSuite testSuite)
        {
            var commandResult = _testRailClient.AddRun(testRailProject.ID, Convert.ToUInt32(testSuite.UserID),
                $"Very important test run:  {DateTime.Now}", "Blah, blah, blah", 1);

            return _testRailClient.GetRun(commandResult.Value);
        }

        public override void OnTestExecutionComplete(TestExecutor testExecutor, TestExecutionCompleteArgs args)
        {
            int time = 500;
            Thread.Sleep(time);
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {VirtualUser}, sleep time: {time}");

        }

        private StringBuilder _testStepResultBuilder = new StringBuilder();
        private int _testStepCounter = 0;

        public override void OnTestCaseExecutionBegin(TestCase testCase, TestCaseBeginExecutionArgs args)
        {
            _testStepCounter = 1;
            _testStepResultBuilder = new StringBuilder();

            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {args.VirtualUser}");
        }

        public override void OnTestCaseExecutionComplete(TestCase testCase, TestCaseResult testCaseResult)
        {
#if DEBUG
            var testStepContent = _testStepResultBuilder.ToString();
#endif
            var messageBuilder = new StringBuilder();

            messageBuilder.AppendLine($"TestCase message:  {testCaseResult.TestMessage ?? "None"}");
            messageBuilder.AppendLine();
            messageBuilder.AppendLine();
            messageBuilder.AppendLine($"{_testStepResultBuilder.ToString()}");
            messageBuilder.AppendLine($"Start time:  {testCaseResult.StartTime}, End time: {testCaseResult.EndTime}, Elapsed time:{testCaseResult.ElapsedTime}");

            var result = _testRailClient.AddResultForCase((ulong)_testRailRun.ID, Convert.ToUInt32(testCase.UserID),
                convertToTestRailStatus(testCaseResult.TestVerdict), messageBuilder.ToString(), "version 1.0",
                testCaseResult.ElapsedTime);

            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {testCaseResult.VirtualUser}");

        }

        public override void OnTestStepExecutionComplete(TestStep testStep, TestStepResult testStepResult)
        {
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {testStepResult.VirtualUser}");

            if (testStepResult.TestVerdict != TestVerdict.DidNotExecute)
            {
                _testStepResultBuilder.AppendLine($"TestStep {_testStepCounter++} - {testStep.Title} ({testStepResult.TestVerdict})");

                _testStepResultBuilder.AppendFormat("TestStep message:  {0}", 
                    string.IsNullOrEmpty(testStepResult.TestMessage) ? "None" : testStepResult.TestMessage);
                _testStepResultBuilder.AppendLine();

                //_testStepResultBuilder.AppendLine($"Test step message:  \"{testStepResult.TestMessage ?? "None"}\"{Environment.NewLine}");

                //_testStepResultBuilder.AppendLine($"\"{testStepResult.TestMessage ?? "None"}{Environment.NewLine}");

                // If there are test checks, process each.
                if (testStepResult.TestChecks != null && testStepResult.TestChecks.Count > 0)
                {
                    //_testStepResultBuilder.AppendFormat("TestChecks:" + Environment.NewLine);

                    // For ease of use.
                    var testChecks = testStepResult.TestChecks;

                    for (int index = 0; index < testChecks.Count; index++)
                    {
                        // For ease of use;
                        var testCheck = testChecks[index];

                        _testStepResultBuilder.AppendLine($"TestCheck {index + 1} - {testCheck.Description} ({testCheck.TestVerdict})");
                        _testStepResultBuilder.AppendLine($"  Comment:  {testCheck.Comment ?? "None"}");
                    }
                }
                else
                {
                    _testStepResultBuilder.AppendFormat("TestChecks:  None" + Environment.NewLine);
                }

                _testStepResultBuilder.AppendLine($"TestStep start time:  {testStepResult.StartTime}, End time: {testStepResult.EndTime}, " +
                    $"Elapsed time:{testStepResult.ElapsedTime}{Environment.NewLine}");

#if DEBUG
                var testStepContent = _testStepResultBuilder.ToString();
#endif
            }
        }

        private ResultStatus convertToTestRailStatus(TestVerdict testVerdict)
        {
            ResultStatus resultStatus = ResultStatus.Untested;

            if (testVerdict == TestVerdict.Pass)
            {
                resultStatus = ResultStatus.Passed;
            }
            else if (testVerdict == TestVerdict.Fail)
            {
                resultStatus = ResultStatus.Failed;
            }

            return resultStatus;
        }

        private void throwException()
        {
            throw new InvalidOperationException();
        }

        public override void OnTestStepExecutionBegin(TestStep testStep, TestStepBeginExecutionArgs args)
        {
            int time = 500;
            Thread.Sleep(time);
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {args.VirtualUser}, sleep time: {time} seconds");

        }



        public override void OnTestSuiteExecutionBegin(TestSuite testSuite, TestSuiteBeginExecutionArgs args)
        {
            int time = 500;
            Thread.Sleep(time);
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {args.VirtualUser}, sleep time: {time} seconds");
        }

        public override void OnTestSuiteExecutionComplete(TestSuite testSuite, TestSuiteResult testSuiteResult)
        {
            int time = 500;
            Thread.Sleep(time);
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
