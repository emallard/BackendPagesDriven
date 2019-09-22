using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CocoriCore;
using CocoriCore.Router;

namespace CocoriCore.Page
{

    public class PageInspector
    {
        private readonly RouterOptions routerOptions;

        public PageInspector(RouterOptions routerOptions)
        {
            this.routerOptions = routerOptions;
        }

        public IEnumerable<FrontTypeInfo> GetPageTypeInfos()
        {
            return GetPageTypes()
                    .Select(r => GetPageTypeInfo(r))
                    .ToList();
        }

        public IEnumerable<Type> GetPageTypes()
        {
            return routerOptions.AllRoutes
                    .Where(r => r.MessageType.IsAssignableTo(typeof(IPageQuery)))
                    .Select(r => r.MessageType)
                    .ToList();
        }

        public IEnumerable<Type> GetPageResponseTypes()
        {
            return GetPageTypes().Select(x => this.GetPageResponseType(x)).ToArray();
        }

        public FrontTypeInfo GetPageTypeInfo(Type pageType)
        {
            var pageResponseType = GetPageResponseType(pageType);
            var typeInfo = new FrontTypeInfo();
            typeInfo.Name = pageType.Name;
            typeInfo.IsPage = true;
            typeInfo.PageUrl = GetPageParameterizedUrl(pageType);
            typeInfo.FieldInfos = GetFields(pageResponseType);
            typeInfo.LinkMemberInfos = GetLinks(pageResponseType);
            typeInfo.FormMemberInfos = GetForms(pageResponseType);
            return typeInfo;
        }

        public IEnumerable<Type> GetNeededTypes(FrontTypeInfo pageTypeInfo)
        {
            return pageTypeInfo.FormMemberInfos
                                .SelectMany(f => new Type[] { f.MessageType, f.ResponseType })
                                .Concat(pageTypeInfo.FieldInfos
                                    .Where(f => f.FieldType != typeof(string)
                                             && f.FieldType != typeof(string[]))
                                    .Select(f => f.FieldType)
                                    .ToList()
                                )
                                .Distinct()
                                .ToList();

        }
        public FrontTypeInfo GetTypeInfo(Type type)
        {
            var isPage = type.IsAssignableTo(typeof(IPageQuery));
            return new FrontTypeInfo()
            {
                Name = type.Name,
                IsPage = isPage,
                PageUrl = !isPage ? null : GetPageParameterizedUrl(type),
                FieldInfos = GetFields(type),
                LinkMemberInfos = GetLinks(type),
                FormMemberInfos = GetForms(type)
            };
        }

        public string GetPageParameterizedUrl(Type pageType)
        {
            var route = routerOptions.AllRoutes.First(r => r.MessageType == pageType);
            return route.ParameterizedUrl;
        }

        public Type GetPageResponseType(Type pageType)
        {
            return pageType.GetGenericArguments(typeof(IPageQuery<>))[0];
        }

        public FieldInfo[] GetFields(Type type)
        {
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);

            return fields
                .Where(f => !f.GetMemberType().IsAssignableTo(typeof(IPageQuery))
                        && !f.GetMemberType().IsAssignableTo(typeof(Call))
                        )
                .ToArray();
        }

        public LinkMemberInfo[] GetLinks(Type type)
        {
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);

            return fields
                .Where(f => f.GetMemberType().IsAssignableTo(typeof(IPageQuery)))
                .Select(l => new LinkMemberInfo() { Name = l.Name })
                .ToArray();
        }

        public FormMemberInfo[] GetForms(Type type)
        {
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);

            return fields
                .Where(f => f.GetMemberType().IsAssignableTo(typeof(Call)))
                .Select(f =>
                {
                    var generics = f.GetMemberType().GetGenericArguments(typeof(Call<,>));
                    return new FormMemberInfo
                    {
                        Name = f.Name,
                        MessageType = generics[0],
                        ResponseType = generics[1]
                    };
                })
                .ToArray();
        }
    }
}
