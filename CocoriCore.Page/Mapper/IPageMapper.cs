using System;

namespace CocoriCore
{
    public interface IPageMapper
    {
        object Map(Type targetType, object o);
        TTarget Map<TTarget>(object o, object p);
        //Type GetIntermediateType<TPageQuery, TModel>();
        Type GetIntermediateType(Type pageQueryType, Type modelType);
        bool TryHandle(object message, out object response);
    }

}