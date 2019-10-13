using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CocoriCore
{
    public class PageModule
    {
        public List<PageMapping3> Mappings3 = new List<PageMapping3>();
        public List<PageHandling> Handlings = new List<PageHandling>();

        protected PageModuleHandlePage<TPageQuery, TPage> HandlePage<TPageQuery, TPage>(
                )
                    where TPageQuery : IMessage<TPage>
                    where TPage : PageBase<TPageQuery>
        {
            return HandlePage<TPageQuery, TPage>(null);
        }
        protected PageModuleHandlePage<TPageQuery, TPage> HandlePage<TPageQuery, TPage>(
            Action<TPageQuery, TPage> action
        )
            where TPageQuery : IMessage<TPage>
            where TPage : PageBase<TPageQuery>
        {
            Handlings.Add(new PageHandling()
            {
                PageQueryType = typeof(TPageQuery),
                Func = m =>
                {
                    var pageQuery = (TPageQuery)m;
                    var page = (TPage)Activator.CreateInstance<TPage>();
                    CreateFields(pageQuery, page);
                    if (action != null)
                        action(pageQuery, page);
                    page.PageQuery = pageQuery;
                    page.PageTypeName = page.GetType().GetFriendlyName();
                    return page;
                }
            });

            return new PageModuleHandlePage<TPageQuery, TPage>(this);
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

                if (member is IForm form)
                    form.SetMemberName(mi.Name);

                if (member is IAsyncCall asyncCall)
                    asyncCall.SetMemberName(mi.Name);
            }
        }

        public PageModuleOnMessage<TMessage> ForMessage<TMessage>()
        {
            return new PageModuleOnMessage<TMessage>(this);
        }

    }

    public class PageModuleHandlePage<TPageQuery, TPage>
    {
        private readonly PageModule module;

        public PageModuleHandlePage(PageModule module)
        {
            this.module = module;
        }

        public PageModuleProvideQuery<TPageQuery, TPage, TQuery> ForAsyncCall<TQuery, TModel>(
            Expression<Func<TPage, AsyncCall<TQuery, TModel>>> member)
            where TQuery : IMessage, new()
        {
            return new PageModuleProvideQuery<TPageQuery, TPage, TQuery>(this, module);
        }

        public PageModuleProvideQuery<TPageQuery, TPage, TQuery> ForAsyncCall<TQuery>()
            where TQuery : IMessage, new()
        {
            return new PageModuleProvideQuery<TPageQuery, TPage, TQuery>(this, module);
        }


        public PageModuleProvideQuery<TPageQuery, TPage, TCommand> ForForm<TCommand, TModel>(
            Expression<Func<TPage, Form<TCommand, TModel>>> member)
            where TCommand : IMessage, new()
        {
            return new PageModuleProvideQuery<TPageQuery, TPage, TCommand>(this, module);
        }

        public PageModuleProvideQuery<TPageQuery, TPage, TCommand> ForForm<TCommand>()
            where TCommand : IMessage, new()
        {
            return new PageModuleProvideQuery<TPageQuery, TPage, TCommand>(this, module);
        }
    }

    public class PageModuleProvideQuery<TPageQuery, TPage, TQuery>
    {
        private readonly PageModuleHandlePage<TPageQuery, TPage> on;
        private readonly PageModule module;

        public PageModuleProvideQuery(PageModuleHandlePage<TPageQuery, TPage> on, PageModule module)
        {
            this.on = on;
            this.module = module;
        }

        public PageModuleProvideQueryWithResponse<TPageQuery, TPage, TQuery, TResponse> MapResponse<TResponse>() //where TQuery : IMessage<TResponse>
        {
            return new PageModuleProvideQueryWithResponse<TPageQuery, TPage, TQuery, TResponse>(on, module);
        }
    }

    public class PageModuleProvideQueryWithResponse<TPageQuery, TPage, TQuery, TResponse> // where TQuery : IMessage<TResponse>
    {
        private PageModuleHandlePage<TPageQuery, TPage> on;
        private readonly PageModule module;

        public PageModuleProvideQueryWithResponse(PageModuleHandlePage<TPageQuery, TPage> on, PageModule module)
        {
            this.on = on;
            this.module = module;
        }

        public PageModuleHandlePage<TPageQuery, TPage> ToModel<TModel>(Action<TQuery, TResponse, TModel> action)
            where TModel : new()
        {
            var mapping3 = new PageMapping3();
            mapping3.Init<TQuery, TResponse, TModel>((q, r) =>
            {
                var model = new TModel();
                action(q, r, model);
                return model;
            });
            module.Mappings3.Add(mapping3);
            return on;
        }

        public PageModuleHandlePage<TPageQuery, TPage> ToModel<TModel>(Func<TQuery, TResponse, TModel> func)
        {
            var mapping3 = new PageMapping3();
            mapping3.Init<TQuery, TResponse, TModel>(func);
            module.Mappings3.Add(mapping3);
            return on;
        }

        public PageModuleHandlePage<TPageQuery, TPage> ToSelf()
        {
            var mapping3 = new PageMapping3();
            mapping3.Init<TQuery, TResponse, TResponse>((q, r) => r);
            module.Mappings3.Add(mapping3);
            return on;
        }
    }

    ////////////////////////////////////////

    public class PageModuleOnMessage<TMessage>
    {
        private readonly PageModule module;

        public PageModuleOnMessage(PageModule module)
        {
            this.module = module;
        }

        public PageModuleWithResponse<TMessage, TResponse> WithResponse<TResponse>()
        {
            return new PageModuleWithResponse<TMessage, TResponse>(module);
        }
    }

    public class PageModuleWithResponse<TMessage, TResponse>
    {
        private readonly PageModule module;

        public PageModuleWithResponse(PageModule module)
        {
            this.module = module;
        }

        public PageModule BuildModel<TModel>(Action<TMessage, TResponse, TModel> action) where TModel : new()
        {
            var mapping3 = new PageMapping3();
            mapping3.Init<TMessage, TResponse, TModel>((q, r) =>
            {
                var model = new TModel();
                action(q, r, model);
                return model;
            });
            module.Mappings3.Add(mapping3);
            return module;
        }

        public PageModule BuildModel<TModel>(Func<TMessage, TResponse, TModel> func)
        {
            var mapping3 = new PageMapping3();
            mapping3.Init<TMessage, TResponse, TModel>(func);
            module.Mappings3.Add(mapping3);
            return module;
        }
    }
}