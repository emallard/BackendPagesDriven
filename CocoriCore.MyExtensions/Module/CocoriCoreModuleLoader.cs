using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CocoriCore
{
    public class CocoriCoreModuleLoader
    {
        private readonly IFactory factory;
        private readonly CocoriCoreModuleLoaderOptions options;
        Dictionary<Type, HandlerFunc> handlings;

        public CocoriCoreModuleLoader(
            IFactory factory,
            CocoriCoreModuleLoaderOptions options)
        {
            this.factory = factory;
            this.options = options;
        }

        public void Load()
        {
            var moduleTypes = options.Assemblies.SelectMany(a => a.GetTypes().Where(t => t.IsAssignableTo(typeof(ICocoriCoreModule)))).ToArray();
            foreach (var t in moduleTypes)
                factory.Create(t);
        }
    }
}