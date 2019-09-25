using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CocoriCore
{

    public class PageMapper : IPageMapper
    {
        Dictionary<Tuple<Type, Type>, PageMapping2> mappings2;
        Dictionary<Tuple<Type, Type, Type>, PageMapping3> mappings3;
        Dictionary<Tuple<Type, Type>, Type> intermediateType;
        Dictionary<Type, PageHandling> handlings;

        public PageMapper(params Assembly[] assemblies)
        {
            var moduleTypes = assemblies.SelectMany(a => a.GetTypes().Where(t => t.IsAssignableTo(typeof(PageModule)))).ToArray();
            var modules = moduleTypes.Select(t => Activator.CreateInstance(t)).Cast<PageModule>().ToArray();
            mappings2 = modules.SelectMany(m => m.Mappings2).ToDictionary(x => x.Key, x => x);
            mappings3 = modules.SelectMany(m => m.Mappings3).ToDictionary(x => x.Key, x => x);

            intermediateType = new Dictionary<Tuple<Type, Type>, Type>();
            foreach (var m2 in mappings2)
            {
                var pageQueryType = m2.Key.Item1;
                var messageType = m2.Key.Item2;
                var modelTypes = mappings3.Keys
                    .Where(x => x.Item1 == messageType)
                    .Select(x => x.Item3)
                    .ToArray();

                foreach (var modelType in modelTypes)
                    intermediateType[Tuple.Create(pageQueryType, modelType)] = messageType;
            }

            handlings = modules.SelectMany(m => m.Handlings).ToDictionary(x => x.Key, x => x);
        }

        /*
        public Type GetIntermediateType<TPageQuery, TModel>()
        {
            return intermediateType[Tuple.Create(typeof(TPageQuery), typeof(TModel))];
        }*/

        public Type GetIntermediateType(Type pageQueryType, Type modelType)
        {
            return intermediateType[Tuple.Create(pageQueryType, modelType)];
        }

        public object Map(Type targetType, object o)
        {
            var found = mappings2[Tuple.Create(o.GetType(), targetType)];
            return found.Func(o);
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