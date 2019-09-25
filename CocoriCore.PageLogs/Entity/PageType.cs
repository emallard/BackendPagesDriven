using System;

namespace CocoriCore.PageLogs
{
    public class PageType : EntityBase<PageType>
    {
        public Type QueryType;
        public Type ResponseType;
        public string Name;
        public string ParameterizedUrl;
    }
}