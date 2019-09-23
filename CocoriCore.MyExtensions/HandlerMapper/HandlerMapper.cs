using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CocoriCore
{

    public class HandlerMapper : IHandlerMapper
    {
        Dictionary<Type, HandlerFunc> handlings;

        public void Add<TMessage, TResponse>(Func<TMessage, TResponse> func) where TMessage : IMessage<TResponse>
        {
            var handlerFunc = HandlerFunc.Create(func);
            handlings[handlerFunc.Key] = handlerFunc;
        }


        public bool TryHandle(object message, out object response)
        {
            response = null;
            HandlerFunc handling;
            var found = this.handlings.TryGetValue(message.GetType(), out handling);
            if (found)
                response = handling.Func(message);
            return found;
        }
    }
}