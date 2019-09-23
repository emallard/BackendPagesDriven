using System;
using System.Collections.Generic;

namespace CocoriCore
{
    public class PageModule
    {
        public List<PageMapping3> Mappings3 = new List<PageMapping3>();
        public List<PageMapping2> Mappings2 = new List<PageMapping2>();
        public List<PageHandling> Handlings = new List<PageHandling>();

        protected void Map<TMessage, TResponse, TModel>(Func<TMessage, TResponse, TModel> mappingFunc)
        {
            Mappings3.Add(PageMapping3.Create(mappingFunc));
        }

        protected void Map<TPageQuery, TMessage>(Func<TPageQuery, TMessage> mappingFunc)
        {
            Mappings2.Add(PageMapping2.Create(mappingFunc));
        }

        protected void MapForm<TMessage, TResponse, TModel>(Func<TMessage, TResponse, TModel> mappingFunc)
        {
            Map(mappingFunc);
        }

        protected void MapAsyncCall<TPageQuery, TMessage, TResponse, TModel>(
            Func<TPageQuery, TMessage> pageQueryToMessageFunc,
            Func<TMessage, TResponse, TModel> resultMappingFunc)
        {
            Map(pageQueryToMessageFunc);
            Map(resultMappingFunc);
        }

        protected void Handle<TMessage, TResponse>(Func<TMessage, TResponse> func) where TMessage : IMessage<TResponse>
        {
            Handlings.Add(PageHandling.Create(func));
        }
    }
}