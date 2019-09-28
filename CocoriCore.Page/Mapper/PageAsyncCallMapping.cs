using System;

namespace CocoriCore
{
    public class PageAsyncCallMapping
    {
        public Type PageQueryType;
        public Type QueryType;
        public string MemberName;
        public Func<object, object> Func { get; set; }
    }
}
