using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using CocoriCore.Router;

namespace CocoriCore.Page
{

    public class PageGraphBuilder
    {
        private readonly RouterOptions routerOptions;

        public PageGraphBuilder(RouterOptions routerOptions)
        {
            this.routerOptions = routerOptions;
        }

        public PageGraph Build(Assembly assembly)
        {
            var nodes = GetPageNodes(assembly);
            var edges = GetPageEdges(assembly, nodes);
            return new PageGraph()
            {
                Nodes = nodes,
                Edges = edges
            };
        }

        public PageGraph Build(Assembly assembly, Func<PageNode, bool> nodePredicate)
        {
            var nodes = GetPageNodes(assembly);
            nodes = nodes.Where(nodePredicate).ToList();
            var edges = GetPageEdges(assembly, nodes);
            return new PageGraph()
            {
                Nodes = nodes,
                Edges = edges
            };
        }

        private List<PageNode> GetPageNodes(Assembly assembly)
        {
            var pages = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(IPageQuery))).ToArray();
            int index = 0;
            var nodes = routerOptions.AllRoutes
                .Where(r => r.MessageType.IsAssignableTo<IPageQuery>())
                .Select(r => new PageNode()
                {
                    ParameterizedUrl = r.ParameterizedUrl,
                    QueryType = r.MessageType,
                    ResponseType = r.MessageType.GetGenericArguments(typeof(IMessage<>))[0],
                    IndexedName = "n" + (index++)
                })
                .ToList();
            return nodes;
        }

        private List<PageEdge> GetPageEdges(Assembly assembly, List<PageNode> nodes)
        {
            var edges = new List<PageEdge>();
            foreach (var n in nodes)
            {
                GetPageEdges(assembly, n, nodes, edges);
            }
            return edges;
        }

        private void GetPageEdges(Assembly assembly, PageNode node, List<PageNode> nodes, List<PageEdge> edges)
        {
            foreach (var fi in node.ResponseType.GetPropertiesAndFields())
            {
                GetPageEdges(assembly, node, nodes, edges, fi.Name, fi.GetMemberType(), false);
            }
        }

        private void GetPageEdges(Assembly assembly, PageNode node, List<PageNode> nodes, List<PageEdge> edges, string memberName, Type memberType, bool isForm)
        {
            if (memberType.IsArray)
            {
                GetPageEdges(assembly, node, nodes, edges, memberName, memberType.GetElementType(), isForm);
            }
            else if (memberType.IsAssignableTo(typeof(IPageQuery)))
            {
                var target = nodes.FirstOrDefault(n => n.QueryType == memberType);
                if (target != null)
                {
                    edges.Add(new PageEdge()
                    {
                        From = node,
                        To = target,
                        Name = memberName,
                        IsLink = true,
                        IsForm = isForm
                    });
                }
            }
            else if (memberType.IsAssignableToGeneric(typeof(Form<,>)))
            {
                var formResponseType = memberType.GetGenericArguments(typeof(Form<,>))[1];
                GetPageEdges(assembly, node, nodes, edges, memberName, formResponseType, true);
            }
            else if (memberType.IsAssignableToGeneric(typeof(AsyncCall<,>)))
            {
                var callResponseType = memberType.GetGenericArguments(typeof(AsyncCall<,>))[1];
                GetPageEdges(assembly, node, nodes, edges, memberName, callResponseType, isForm);
            }
            else
            {
                if (memberType.Assembly == assembly)
                {
                    foreach (var fi in memberType.GetPropertiesAndFields())
                    {
                        GetPageEdges(assembly, node, nodes, edges, isForm ? memberName : fi.Name, fi.GetMemberType(), isForm);
                    }
                }
            }
        }
    }
}