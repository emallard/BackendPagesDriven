using System;

namespace CocoriCore
{
    public class NewMapperUpdateMapping
    {
        public Func<object, Guid> IdFunc;
        public Type FromType;
        public Type ToType;
        public Action<object, object> MapAction;
    }
}