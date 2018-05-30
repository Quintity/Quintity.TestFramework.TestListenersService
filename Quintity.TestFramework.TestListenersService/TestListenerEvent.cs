using Quintity.TestFramework.Runtime;

namespace Quintity.TestFramework.TestListenersService
{
    public class TestListenerEvent
    {
        private string _virtualUser;
        public string VirtualUser
        { get { return _virtualUser; } }

        private string _method;
        public string Method
        { get { return _method; } }

        private object[] _parameters;
        public object[] Parameters
        { get { return _parameters; } }

        public TestListenerDescriptor TestListenerDescriptor
        { get; set; }

        public TestListenerEvent(string virtualUser, string method, params object[] parameters)
        {
            _virtualUser = virtualUser;
            _method = method;
            _parameters = parameters;
        }
    }
}
