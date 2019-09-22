using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CocoriCore.Page
{
    public class TypeDiscovery
    {
        HashSet<Type> DiscoveredTypes = new HashSet<Type>();

        public Type[] GetNeededTypes(IEnumerable<Type> allTypes)
        {
            foreach (var t in allTypes)
                Discover(t);

            return DiscoveredTypes.ToArray();
        }

        private void Discover(Type type)
        {
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);

            var forms = fields.Where(f => f.FieldType.IsAssignableTo(typeof(Call)));

            var formTypes = forms.SelectMany(f =>
                    {
                        var generics = f.GetMemberType().GetGenericArguments(typeof(Call<,>));
                        return new Type[] { generics[0], generics[1] };
                    }).ToArray();

            var arrays = fields.Where(f => f.FieldType.IsArray)
                           .Select(f => f.FieldType.GetElementType())
                           .ToArray();

            var notArrays = fields.Where(
                                f => !f.FieldType.IsArray
                                  && !f.FieldType.IsAssignableTo(typeof(Call))
                                  && !f.FieldType.IsAssignableTo(typeof(IPageQuery))
                                  )
                           .Select(f => f.FieldType)
                           .ToArray();

            var all = formTypes.Concat(arrays.Concat(notArrays)).ToArray();
            foreach (var a in all)
            {
                if (!this.IsBaseType(a)
                 && !DiscoveredTypes.Contains(a))
                {
                    DiscoveredTypes.Add(a);
                    this.Discover(a);
                }
            }

        }

        private bool IsBaseType(Type t)
        {
            return (t == typeof(string)
                 || t == typeof(DateTime)
                 || t == typeof(double)
                 || t == typeof(float)
                 || t == typeof(int));
        }
    }
}
