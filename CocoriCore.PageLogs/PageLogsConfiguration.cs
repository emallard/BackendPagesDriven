using System.Reflection;

namespace CocoriCore.PageLogs
{
    public class PageLogsConfiguration
    {
        public Assembly[] Assemblies;
        public PageLogsConfiguration(params Assembly[] assemblies)
        {
            this.Assemblies = assemblies;
        }
    }
}