using System;
using Newtonsoft.Json.Serialization;

namespace CocoriCore
{
    public class DefaultAssemblyBinder : DefaultSerializationBinder
    {
        public string DefaultAssemblyName { get; private set; }
        public string DefaultNamespaceName { get; private set; }

        public DefaultAssemblyBinder()
        {
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                if (assembly.FullName.StartsWith(assemblyName))
                    assemblyName = assembly.FullName;
            {
                return base.BindToType(assemblyName, typeName);
            }
        }

        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            base.BindToName(serializedType, out assemblyName, out typeName);
            assemblyName = assemblyName.Substring(0, assemblyName.IndexOf("Version="));
        }
    }
}