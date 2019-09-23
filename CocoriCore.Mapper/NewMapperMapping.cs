using System;

namespace CocoriCore
{
    public class NewMapperMapping
    {
        public Type FromType;
        public Type ToType;
        public Action<object, object> MapAction;

    }
}