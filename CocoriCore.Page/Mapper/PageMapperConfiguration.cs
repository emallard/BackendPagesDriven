using System.Reflection;

namespace CocoriCore
{
    public class PageMapperConfiguration
    {
        public PageMapperConfiguration(params Assembly[] assemblies)
        {
            Assemblies = assemblies;
        }

        public Assembly[] Assemblies { get; }
    }
}