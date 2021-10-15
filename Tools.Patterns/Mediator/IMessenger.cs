using System;

namespace Tools.Patterns.Mediator
{
    public interface IMessenger<TMessage>
    {
        void Register(Action<TMessage> action);
        void Send(TMessage message);
        void Unregister(Action<TMessage> action);
    }
}