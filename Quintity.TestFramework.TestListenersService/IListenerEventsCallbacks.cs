using System.Collections.Generic;
using System.ServiceModel;
using Quintity.TestFramework.Runtime;

namespace Quintity.TestFramework.TestListenersService
{
    public interface IListenerEventsCallbacks
    {
        [OperationContract(IsOneWay = true)]
        void TestListenersCompleteNotification(List<TestListenerDescriptor> testListeners, TestListenersCompleteArgs args);
    }
}
