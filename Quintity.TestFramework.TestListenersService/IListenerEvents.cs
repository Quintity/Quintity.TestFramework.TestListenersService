using System.ServiceModel;
using System.Collections.Generic;
using Quintity.TestFramework.Core;
using Quintity.TestFramework.Runtime;

namespace Quintity.TestFramework.TestListenersService
{
    //  [ServiceContract(CallbackContract = typeof(IListenerEventsCallbacks), SessionMode = SessionMode.Required)]
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IListenerEvents
    {
        [OperationContract]
        bool ServiceAvailability();

        [OperationContract]
        int TestMethod(string testString);

        [OperationContract]
        int InitializeService(List<TestListenerDescriptor> testListeners, TestProfile testProfile);

        [OperationContract(IsOneWay=true)]
        void OnTestExecutionBegin(TestExecutionBeginArgs args);

        [OperationContract(IsOneWay=true)]
        void OnTestExecutionComplete(TestExecutionCompleteArgs args);

        [OperationContract(IsOneWay=true)]
        void OnTestSuiteExecutionBegin(TestSuite testSuite, TestSuiteBeginExecutionArgs args);

        [OperationContract(IsOneWay=true)]
        void OnTestPreprocessorBegin(TestSuite testSuite, TestProcessorBeginExecutionArgs args);

        [OperationContract(IsOneWay=true)]
        void OnTestPreprocessorComplete(TestSuite testSuite, TestProcessorResult testProcessorResult);

        [OperationContract(IsOneWay=true)]
        void OnTestPostprocessorBegin(TestSuite testSuite, TestProcessorBeginExecutionArgs args);

        [OperationContract(IsOneWay=true)]
        void OnTestPostprocessorComplete(TestSuite testSuite, TestProcessorResult testProcessorResult);

        [OperationContract(IsOneWay=true)]
        void OnTestSuiteExecutionComplete(TestSuite testSuite, TestSuiteResult testSuiteResult);

        [OperationContract(IsOneWay=true)]
        void OnTestCaseExecutionBegin(TestCase testCase, TestCaseBeginExecutionArgs args);

        [OperationContract(IsOneWay=true)]
        void OnTestCaseExecutionComplete(TestCase testCase, TestCaseResult testCaseResult);

        [OperationContract(IsOneWay=true)]
        void OnTestStepExecutionBegin(TestStep testStep, TestStepBeginExecutionArgs args);

        [OperationContract(IsOneWay=true)]
        void OnTestStepExecutionComplete(TestStep testStep, TestStepResult testStepResult);

        [OperationContract(IsOneWay=true)]
        void OnTestCheck(TestCheck testCheck);

        [OperationContract(IsOneWay=true)]
        void OnTestWarning(TestWarning testWarning);

        [OperationContract(IsOneWay=true)]
        void OnTestTrace(string virtualUser, string traceMessage);

        [OperationContract(IsOneWay=true)]
        void OnTestMetric(string virtualUser, TestMetricEventArgs args);
    }
}
