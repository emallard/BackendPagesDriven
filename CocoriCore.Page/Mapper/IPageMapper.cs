using System;

namespace CocoriCore
{
    public interface IPageMapper
    {
        TTarget Map<TTarget>(object o, object p);
        bool TryHandle(object message, out object response);
    }

}