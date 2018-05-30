using System.Collections.Generic;

namespace Quintity.TestFramework.TestListenersService
{
    internal class RuntimeStatus
    {
        public Queue<TestListenerEvent> ListenerEventQueue { get; set; }

        public RuntimeStatus()
        {
            ListenerEventQueue = new Queue<TestListenerEvent>();
        }
    }
}
