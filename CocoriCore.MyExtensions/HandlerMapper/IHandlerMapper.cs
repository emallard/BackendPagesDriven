using System;

namespace CocoriCore
{
    public interface IHandlerMapper
    {
        void Add<TMessage, TResponse>(Func<TMessage, TResponse> func) where TMessage : IMessage<TResponse>;
        bool TryHandle(object message, out object response);
    }

}