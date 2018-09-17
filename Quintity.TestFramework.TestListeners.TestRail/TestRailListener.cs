using System;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;
using Quintity.TestFramework.Core;
using Quintity.TestFramework.Runtime;
using TestRail;
using TestRail.Types;
using log4net;


namespace Quintity.TestFramework.TestListeners.TestRail
{
    public class TestRailListener : TestListener
    {
        #region Data members

        private static readonly log4net.ILog LogEvent = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<string, string> _parameters;
        private TestRailClient _testRailClient = null;
        private Project _testRailProject = null;
        private Run _testRailRun = null;
        private StringBuilder _testStepResultBuilder;
        private readonly string testStepDivider = "____________________________________________________________";
        private int _testStepCounter = 0;
        private string _currentTestSuiteFile = String.Empty;
        private bool _validExecution = true;

        private string _testRailUrl = String.Empty;
        private string _testRailUser = String.Empty;
        private string _testRailPassword = String.Empty;
        private string _testRailProjectName = String.Empty;
        private string _productVersion = String.Empty;
        private string _targetEnvironment = String.Empty;
        private string _browser = String.Empty;

        private int _totalRunTestCases = 0;
        private int _totalRunTestSteps = 0;
        private int _totalRunTestChecks = 0;

        private int _testStepChecksPass = 0;
        private int _testStepChecksFail = 0;
        private int _testStepChecksError = 0;

        #endregion

        #region Constructors

        public TestRailListener()
        {
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name}");
        }

        public TestRailListener(Dictionary<string, string> args)
        {
            try
            {
                _parameters = args;

                TestAssert.IsTrue(_parameters.TryGetValue("TestRailUrl", out _testRailUrl),
                    "TestRailUrl not provided as listener parameter.");
                TestAssert.IsTrue(_parameters.TryGetValue("TestRailUser", out _testRailUser),
                    "TestRailUser not provided as listener parameter.");
                TestAssert.IsTrue(_parameters.TryGetValue("TestRailPassword", out _testRailPassword),
                    "TestRailPassword not provided as listener parameter.");
                TestAssert.IsTrue(_parameters.TryGetValue("TestRailProject", out _testRailProjectName),
                    "TestRailProject not provided as listener parameter.");
                TestAssert.IsTrue(_parameters.TryGetValue("ProductVersion", out _productVersion),
                    "ProductVersion not provided as listener parameter.");
                TestAssert.IsTrue(_parameters.TryGetValue("TargetEnvironment", out _targetEnvironment),
                    "TargetEnvironment not provided as listener parameter.");
                TestAssert.IsTrue(_parameters.TryGetValue("Browser", out _browser),
                   "Browser not provided as listener parameter.");
            }
            catch (Exception)
            {
                _validExecution = false;

                throw;
            }
        }

        #endregion

        #region Used listener event handlers

        public override void OnTestExecutionBegin(TestExecutor testExecutor, TestExecutionBeginArgs args)
        {
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {args.VirtualUser}");

            try
            {
                if (_validExecution)
                {
                    onTestExecutionBegin(testExecutor, args);
                }

            }
            catch (Exception e)
            {
                _validExecution = false;
                LogEvent.Error(e.ToString());
                throw;
            }
        }

        [NoOperation]
        public override void OnTestSuiteExecutionBegin(TestSuite testSuite, TestSuiteBeginExecutionArgs args)
        {
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {args.VirtualUser}");

            try
            {
                if (_validExecution)
                {
                    onTestSuiteExecutionBegin(testSuite, args);
                }
            }
            catch (Exception e)
            {
                _validExecution = false;
                LogEvent.Error(e.ToString());
                throw;
            }
        }

        public override void OnTestCaseExecutionBegin(TestCase testCase, TestCaseBeginExecutionArgs args)
        {
            try
            {
                if (_validExecution)
                {
                    onTestCaseExecutionBegin(testCase, args);
                }
            }
            catch (Exception e)
            {
                _validExecution = false;
                LogEvent.Error(e.ToString());
                throw;
            }

        }

        public override void OnTestStepExecutionBegin(TestStep testStep, TestStepBeginExecutionArgs args)
        {
            try
            {
                if (_validExecution)
                {
                    onTestStepExecutionBegin(testStep, args);
                }
            }
            catch (Exception e)
            {
                _validExecution = false;
                LogEvent.Error(e.ToString());
                throw;
            }
        }

        public override void OnTestCheck(TestCheck testCheck)
        {
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {testCheck.VirtualUser}");

            try
            {
                if (_validExecution)
                {
                    onTestCheck(testCheck);
                }
            }
            catch (Exception e)
            {
                _validExecution = false;
                LogEvent.Error(e.ToString());
                throw;
            }
        }

        public override void OnTestStepExecutionComplete(TestStep testStep, TestStepResult testStepResult)
        {
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {testStepResult.VirtualUser}");

            try
            {
                if (_validExecution)
                {
                    onTestStepExecutionComplete(testStep, testStepResult);
                }
            }
            catch (Exception e)
            {
                _validExecution = false;
                LogEvent.Error(e.ToString());
                throw;
            }
        }

        public override void OnTestCaseExecutionComplete(TestCase testCase, TestCaseResult testCaseResult)
        {
            try
            {
                if (_validExecution)
                {
                    onTestCaseExecutionComplete(testCase, testCaseResult);
                }
            }
            catch (Exception e)
            {
                _validExecution = false;
                LogEvent.Error(e.ToString());
                throw;
            }
        }

        public override void OnTestSuiteExecutionComplete(TestSuite testSuite, TestSuiteResult testSuiteResult)
        {
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {testSuiteResult.VirtualUser}");

            try
            {
                if (_validExecution)
                {
                    onTestSuiteExecutionComplete(testSuite, testSuiteResult);
                }
            }
            catch (Exception e)
            {
                _validExecution = false;
                LogEvent.Error(e.ToString());
                throw;
            }
        }

        public override void OnTestExecutionComplete(TestExecutor testExecutor, TestExecutionCompleteArgs args)
        {
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {args.VirtualUser}");

            try
            {
                if (_validExecution)
                {
                    onTestExecutionComplete(testExecutor, args);
                }
            }
            catch (Exception e)
            {
                _validExecution = false;
                LogEvent.Error(e.ToString());
                throw;
            }
        }

        #endregion

        #region Unused listener event handlers
        [NoOperation]
        public override void OnTestMetric(string virtualUser, TestMetricEventArgs args) { }

        [NoOperation]
        public override void OnTestPostprocessorBegin(TestSuite testSuite, TestProcessorBeginExecutionArgs args) { }

        [NoOperation]
        public override void OnTestPostprocessorComplete(TestSuite testSuite, TestProcessorResult testProcessorResult) { }

        [NoOperation]
        public override void OnTestPreprocessorBegin(TestSuite testSuite, TestProcessorBeginExecutionArgs args) { }

        [NoOperation]
        public override void OnTestPreprocessorComplete(TestSuite testSuite, TestProcessorResult testProcessorResult) { }

        [NoOperation]
        public override void OnTestTrace(string virtualString, string traceMessage) { }

        [NoOperation]
        public override void OnTestWarning(TestWarning testWarning) { }

        #endregion

        #region Private listeners methods

        private void onTestExecutionBegin(TestExecutor testExecutor, TestExecutionBeginArgs args)
        {
            TestAssert.IsFalse(string.IsNullOrEmpty(args.TestScriptObject.UserID), "");

            // Reset run time totals
            _totalRunTestCases = _totalRunTestSteps = _totalRunTestChecks = 0;

            // Create TestRail client.
            _testRailClient = new TestRailClient(_testRailUrl, _testRailUser, _testRailPassword);

            // Get applicable project
            _testRailProject = getTestRailProject(_testRailProjectName);

            // Project milestone
            var milestones = _testRailClient.GetMilestones(_testRailProject.ID);
            var milestone = milestones.Find(x => x.Name.Contains(_testRailProjectName) &&
                x.Name.Contains(_productVersion));

            // Create test run
            _testRailRun = addTestRailRun(_testRailProject, milestone, (TestSuite)args.TestScriptObject);
        }

        private void onTestSuiteExecutionBegin(TestSuite testSuite, TestSuiteBeginExecutionArgs args)
        {
            //if (!string.IsNullOrEmpty(testSuite.UserID))
            //{
                _currentTestSuiteFile = testSuite.FileName;
            //}
        }

        private void onTestCaseExecutionBegin(TestCase testCase, TestCaseBeginExecutionArgs args)
        {
            _totalRunTestCases++;
            _testStepCounter = 0;
            _testStepResultBuilder = new StringBuilder();
            _testStepResultBuilder.AppendLine("Test case test steps:");
            _testStepResultBuilder.AppendLine();
        }

        private void onTestStepExecutionBegin(TestStep testStep, TestStepBeginExecutionArgs args)
        {
            _totalRunTestSteps++;
            _testStepChecksPass = _testStepChecksFail = _testStepChecksError = 0;
        }

        private void onTestCheck(TestCheck testCheck)
        {
            _totalRunTestChecks++;

            if (testCheck.TestVerdict == TestVerdict.Pass)
            {
                _testStepChecksPass++;
            }
            else if (testCheck.TestVerdict == TestVerdict.Fail)
            {
                _testStepChecksFail++;
            }
            else if (testCheck.TestVerdict == TestVerdict.Error)
            {
                _testStepChecksError++;
            }
        }

        private void onTestStepExecutionComplete(TestStep testStep, TestStepResult testStepResult)
        {
            if (testStepResult.TestVerdict != TestVerdict.DidNotExecute)
            {
                _testStepResultBuilder.AppendLine($"TestStep {_testStepCounter++} - {testStep.Title} ({testStepResult.TestVerdict})");

                _testStepResultBuilder.AppendFormat("TestStep message:  {0}",
                    string.IsNullOrEmpty(testStepResult.TestMessage) ? "None" : testStepResult.TestMessage);
                _testStepResultBuilder.AppendLine();

                // If there are test checks, process each.
                if (testStepResult.TestChecks != null && testStepResult.TestChecks.Count > 0)
                {
                    _testStepResultBuilder.AppendFormat("TestChecks:" + Environment.NewLine);

                    // For ease of use.
                    var testChecks = testStepResult.TestChecks;

                    for (int index = 0; index < testChecks.Count; index++)
                    {
                        // For ease of use;
                        var testCheck = testChecks[index];

                        _testStepResultBuilder.AppendLine($"TestCheck {index + 1} - {testCheck.Description} ({testCheck.TestVerdict})");

                        if (testCheck.TestVerdict != TestVerdict.Pass)
                        {
                            _testStepResultBuilder.AppendLine($"Comment:  {testCheck.Comment ?? "None"}");
                        }
                    }
                }
                else
                {
                    _testStepResultBuilder.AppendFormat("TestChecks:  None" + Environment.NewLine);
                }

                _testStepResultBuilder.AppendLine($"Passed: {_testStepChecksFail}, Failed: {_testStepChecksPass}, Errored: {_testStepChecksError}," +
                    $" Total: {_testStepChecksPass + _testStepChecksFail + _testStepChecksError}");

                _testStepResultBuilder.AppendLine($"Start:  {testStepResult.StartTime}, End: {testStepResult.EndTime}, " +
                    $"Elapsed time:{testStepResult.ElapsedTime}{Environment.NewLine}");

                _testStepResultBuilder.AppendLine(testStepDivider);

#if DEBUG
                var testStepContent = _testStepResultBuilder.ToString();
#endif
            }
        }

        private void onTestCaseExecutionComplete(TestCase testCase, TestCaseResult testCaseResult)
        {
#if DEBUG
            var testStepContent = _testStepResultBuilder.ToString();
#endif
            try
            {
                LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {testCaseResult.VirtualUser}");

                var messageBuilder = new StringBuilder();
                messageBuilder.AppendLine($"{_testStepResultBuilder.ToString()}");
                messageBuilder.AppendLine();
                messageBuilder.AppendLine("Test case summary:");
                messageBuilder.AppendLine($"Test suite:  \"{_currentTestSuiteFile}\"");
                messageBuilder.AppendLine($"Test message:  {testCaseResult.TestMessage ?? "None"}");
                messageBuilder.AppendLine($"Test verdict:  {testCaseResult.TestVerdict}");
                messageBuilder.AppendLine(testCaseResult.FormattedCounters);
                messageBuilder.AppendLine($"Start:  {testCaseResult.StartTime}, End: {testCaseResult.EndTime}, Elapsed time:{testCaseResult.ElapsedTime}");

#if DEBUG
                var uUserId = Convert.ToUInt32(testCase.UserID);
                var trStatus = convertToTestRailStatus(testCaseResult.TestVerdict);
                var message = messageBuilder.ToString();
#endif
                var commandResult = _testRailClient.AddResultForCase((ulong)_testRailRun.ID, Convert.ToUInt32(testCase.UserID),
                    convertToTestRailStatus(testCaseResult.TestVerdict), messageBuilder.ToString(), _parameters["ProductVersion"],
                    testCaseResult.ElapsedTime);

                LogEvent.Info(string.Format("Test case result addition {0}.", commandResult.WasSuccessful ? "successful" : "unsuccessful"));

                if (!commandResult.WasSuccessful)
                {
                    //LogEvent.Info(commandResult.Exception.ToString());
                    throw commandResult.Exception;
                }
            }
            catch (Exception e)
            {
                LogEvent.Error(e.ToString());
                throw;
            }
            finally
            {
            }
        }

        private void onTestSuiteExecutionComplete(TestSuite testSuite, TestSuiteResult testSuiteResult)
        {
            var description = $"Browser:  {_browser}{Environment.NewLine}Environment:  {_targetEnvironment}{Environment.NewLine}" +
             $"Total test cases: { _totalRunTestCases}, total test steps: { _totalRunTestSteps}, total test checks: { _totalRunTestChecks}";
            _testRailClient.UpdateRun(_testRailRun.ID ?? 0, name: _testRailRun.Name, description: description);

            _currentTestSuiteFile = string.Empty;
        }

        private void onTestExecutionComplete(TestExecutor testExecutor, TestExecutionCompleteArgs args)
        {
            var description = $"Browser:  {_browser}{Environment.NewLine}Environment:  {_targetEnvironment}{Environment.NewLine}" +
                    $"Total test cases: { _totalRunTestCases}, total test steps: { _totalRunTestSteps}, total test checks: { _totalRunTestChecks}";
            _testRailClient.UpdateRun(_testRailRun.ID ?? 0, name: _testRailRun.Name, description: description);
        }

        #endregion

        #region Private helper methods

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

        private Project getTestRailProject(string projectName)
        {
            return _testRailClient.GetProjects().Find(x => x.Name.Equals(projectName));
        }

        private Run addTestRailRun(Project testRailProject, Milestone milestone, TestSuite testSuite)
        {
            var commandResult = _testRailClient.AddRun(testRailProject.ID, Convert.ToUInt32(testSuite.UserID),
                $"{testSuite.Title} (S{testSuite.UserID}) - {DateTime.Now}", testSuite.Description, milestone.ID);

            return _testRailClient.GetRun(commandResult.Value);
        }

        #endregion

    }
}
