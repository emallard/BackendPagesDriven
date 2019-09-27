using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
            where TMessage : IMessage<TResponse>
        {
            Map(mappingFunc);
        }

        protected void MapAsyncCall<TPageQuery, TQuery, TResponse, TModel>(
            Func<TPageQuery, TQuery> pageQueryToMessageFunc,
            Func<TQuery, TResponse, TModel> resultMappingFunc)
            where TQuery : IMessage<TResponse>
        {
            Map(pageQueryToMessageFunc);
            Map(resultMappingFunc);
        }

        protected void Handle<TMessage, TResponse>(Func<TMessage, TResponse> func) where TMessage : IMessage<TResponse>
        {
            Handlings.Add(PageHandling.Create(func));
        }

        protected void HandlePage<TMessage, TResponse>()
            where TMessage : IMessage<TResponse>
            where TResponse : PageBase<TMessage, TResponse>
        {
            Func<TMessage, TResponse> func = m =>
            {
                var page = (PageBase<TMessage, TResponse>)Activator.CreateInstance<TResponse>();
                page.PageQuery = m;
                CreateFields(m, page);
                return (TResponse)page;
            };


            Handlings.Add(PageHandling.Create(func));
        }

        private void CreateFields(object pageQuery, object o)
        {
            if (o == null)
                return;

            List<MemberInfo> propertiesAndFields = new List<MemberInfo>();
            propertiesAndFields.AddRange(o.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            propertiesAndFields.AddRange(o.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            foreach (var mi in propertiesAndFields)
            {
                var memberType = mi.GetMemberType();
                var member = Activator.CreateInstance(memberType);
                mi.InvokeSetter(o, member);

                if (member is ISetPageQuery setPageQuery)
                    setPageQuery.SetPageQuery(pageQuery);
            }
        }


        protected PageModuleOn<T> On<T>() where T : IPageQuery
        {

            var mapping2 = new PageMapping2();
            var mapping3 = new PageMapping3();
            Mappings2.Add(mapping2);
            Mappings3.Add(mapping3);
            return new PageModuleOn<T>(mapping2, mapping3);
        }
    }

    public class PageModuleOn<TPageQuery>
    {
        private readonly PageMapping2 mapping2;
        private readonly PageMapping3 mapping3;

        public PageModuleOn(PageMapping2 mapping2, PageMapping3 mapping3)
        {
            this.mapping2 = mapping2;
            this.mapping3 = mapping3;
        }

        public PageModuleProvide<TPageQuery, TQuery> ProvideQuery<TQuery>(Action<TPageQuery, TQuery> action) where TQuery : new()
        {

            this.mapping2.Init<TPageQuery, TQuery>(pageQuery =>
            {
                var query = new TQuery();
                action(pageQuery, query);
                return query;
            });
            return new PageModuleProvide<TPageQuery, TQuery>(this, mapping3);
        }
    }

    public class PageModuleProvide<TPageQuery, TQuery>
    {
        private readonly PageModuleOn<TPageQuery> on;
        private readonly PageMapping3 mapping3;

        public PageModuleProvide(PageModuleOn<TPageQuery> on, PageMapping3 mapping3)
        {
            this.on = on;
            this.mapping3 = mapping3;
        }

        public PageModuleWithResponse<TPageQuery, TQuery, TResponse> WithResponse<TResponse>() //where TQuery : IMessage<TResponse>
        {
            return new PageModuleWithResponse<TPageQuery, TQuery, TResponse>(on, mapping3);
        }


    }

    public class PageModuleWithResponse<TPageQuery, TQuery, TResponse> // where TQuery : IMessage<TResponse>
    {
        private PageModuleOn<TPageQuery> on;
        private PageMapping3 mapping3;

        public PageModuleWithResponse(PageModuleOn<TPageQuery> on, PageMapping3 mapping3)
        {
            this.on = on;
            this.mapping3 = mapping3;
        }

        public PageModuleOn<TPageQuery> ToModel<TModel>(Action<TQuery, TResponse, TModel> action)
            where TModel : new()
        {
            mapping3.Init<TQuery, TResponse, TModel>((q, r) =>
            {
                var model = new TModel();
                action(q, r, model);
                return model;
            });
            return on;
        }

        public PageModuleOn<TPageQuery> ToModel<TModel>(Func<TQuery, TResponse, TModel> func)
        {
            mapping3.Init<TQuery, TResponse, TModel>(func);
            return on;
        }

        /*public PageModuleOn<TPageQuery> ToModelArray<TModel>(Action<TQuery, TResponse, TModel> action)
            where TModel : new()
        {
            mapping3.Init<TQuery, TResponse[], TModel[]>((q, r) =>
            {
                return r.Select(x =>
                {
                    var model = new TModel();
                    action(q, x, model);
                    return model;
                }).ToArray();
            });
            return on;
        }*/

        public PageModuleOn<TPageQuery> AsModel()
        {
            mapping3.Init<TQuery, TResponse, TResponse>((q, r) => r);
            return on;
        }
    }
}