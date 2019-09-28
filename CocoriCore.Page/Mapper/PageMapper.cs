using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CocoriCore
{

    public class PageMapper : IPageMapper
    {
        Dictionary<Tuple<Type, Type, Type>, PageMapping3> mappings3;
        Dictionary<Type, PageHandling> handlings;

        public PageMapper(PageMapperConfiguration configuration)
        {
            var assemblies = configuration.Assemblies;
            var moduleTypes = assemblies.SelectMany(a => a.GetTypes().Where(t => t.IsAssignableTo(typeof(PageModule)))).ToArray();
            var modules = moduleTypes.Select(t => Activator.CreateInstance(t)).Cast<PageModule>().ToArray();
            mappings3 = modules.SelectMany(m => m.Mappings3).ToDictionary(x => x.Key, x => x);
            handlings = modules.SelectMany(m => m.Handlings).ToDictionary(x => x.PageQueryType, x => x);
        }

        public TTarget Map<TTarget>(object o, object p)
        {
            var found = mappings3[Tuple.Create(o.GetType(), p.GetType(), typeof(TTarget))];
            return (TTarget)found.Func(o, p);
        }

        public bool TryHandle(object message, out object response)
        {
            response = null;
            PageHandling handling;
            var found = this.handlings.TryGetValue(message.GetType(), out handling);
            if (found)
                response = handling.Func(message);
            return found;
        }
    }
}