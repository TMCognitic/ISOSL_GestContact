using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Patterns.Mediator
{
    public class Messenger<TMessage> : IMessenger<TMessage>
    {
        private Action<TMessage> _broadcast;

        public void Register(Action<TMessage> action)
        {
            _broadcast += action;
        }

        public void Unregister(Action<TMessage> action)
        {
            _broadcast -= action;
        }

        public void Send(TMessage message)
        {
            _broadcast?.Invoke(message);
        }
    }
}
