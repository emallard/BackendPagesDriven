using System.Reflection;

namespace CocoriCore
{
    public class CocoriCoreModuleLoaderOptions
    {
        public Assembly[] Assemblies;

        public CocoriCoreModuleLoaderOptions(params Assembly[] assemblies)
        {
            Assemblies = assemblies;
        }
    }
}