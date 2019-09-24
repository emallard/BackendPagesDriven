using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace CocoriCore.Mapper
{

    public class MapperGraphBuilder
    {
        public MapperGraph Build(Assembly assembly)
        {
            var nodes = GetMapperNodes(assembly);
            var edges = GetMapperEdges(assembly, nodes);
            return new MapperGraph()
            {
                Nodes = nodes,
                Edges = edges
            };
        }

        public MapperGraph Build(Assembly assembly, Func<MapperNode, bool> nodePredicate)
        {
            var nodes = GetMapperNodes(assembly);
            nodes = nodes.Where(nodePredicate).ToList();
            var edges = GetMapperEdges(assembly, nodes);
            return new MapperGraph()
            {
                Nodes = nodes,
                Edges = edges
            };
        }

        private List<MapperNode> GetMapperNodes(Assembly assembly)
        {
            var entityTypes = assembly.GetTypes().Where(t =>
                t.IsAssignableTo(typeof(IEntity))
                || t.IsAssignableToGeneric(typeof(IView<>))
                || t.IsAssignableToGeneric(typeof(ICreate<>))
                || t.IsAssignableToGeneric(typeof(IUpdate<>))
            ).ToArray();
            var nodes =
                entityTypes.Select(t => new MapperNode()
                {
                    Type = t,
                })
                .ToList();
            return nodes;
        }

        private List<MapperEdge> GetMapperEdges(Assembly assembly, List<MapperNode> nodes)
        {
            var edges = new List<MapperEdge>();
            foreach (var n in nodes)
            {
                if (n.Type.IsAssignableToGeneric(typeof(IView<>)))
                    edges.Add(new MapperEdge()
                    {
                        From = nodes.First(x => x.Type == n.Type.GetGenericArguments(typeof(IView<>))[0]),
                        To = n
                    });

                if (n.Type.IsAssignableToGeneric(typeof(ICreate<>)))
                    edges.Add(new MapperEdge()
                    {
                        From = n,
                        To = nodes.First(x => x.Type == n.Type.GetGenericArguments(typeof(ICreate<>))[0])
                    });

                if (n.Type.IsAssignableToGeneric(typeof(IUpdate<>)))
                    edges.Add(new MapperEdge()
                    {
                        From = n,
                        To = nodes.First(x => x.Type == n.Type.GetGenericArguments(typeof(IUpdate<>))[0])
                    });
            }
            return edges;
        }
    }
}