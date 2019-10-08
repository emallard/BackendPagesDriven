using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CocoriCore.Linq.Async;
using CocoriCore.Page;
using CocoriCore.Router;

namespace CocoriCore.PageLogs
{

    public class PageGraphBuilderFromDb
    {
        private readonly IRepository repository;

        public PageGraphBuilderFromDb(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<PageGraph> Build()
        {
            var nodes = await GetPageNodesAsync();
            var edges = await GetPageEdgesAsync(nodes);
            return new PageGraph()
            {
                Nodes = nodes,
                Edges = edges
            };
        }

        private async Task<List<PageNode>> GetPageNodesAsync()
        {
            var pageNames = (await repository
                                    .Query<TestPage>()
                                    .Select(x => new { x.PageName, x.PageUrl })
                                    .ToArrayAsync())
                                    .Distinct()
                                    .ToArray();

            return pageNames.Select(x => new PageNode()
            {
                ParameterizedUrl = x.PageName + "\\n" + (x.PageUrl == "/" ? "//" : x.PageUrl),
                IndexedName = x.PageName
            }).ToList();

        }

        private async Task<List<PageEdge>> GetPageEdgesAsync(List<PageNode> nodes)
        {
            var edges = new List<PageEdge>();
            var testNames = (await repository.Query<Test>()
                                             .Select(x => x.TestName)
                                             .ToArrayAsync())
                                             .Distinct()
                                             .ToArray();
            foreach (var t in testNames)
                await AddPageEdgesAsync(t, nodes, edges);
            return edges;
        }

        private async Task AddPageEdgesAsync(string testName, List<PageNode> nodes, List<PageEdge> edges)
        {

            var redirections = await repository.Query<TestPageRedirection>()
                                          .Where(x => x.TestName == testName)
                                          .OrderBy(x => x.IndexInTest)
                                          .ToArrayAsync();

            foreach (var x in redirections)
            {
                if (x.FromPageName == null)
                    continue;

                if (!edges.Any(e => e.Name == x.MemberName
                                 && e.From.IndexedName == x.FromPageName
                                 && e.To.IndexedName == x.ToPageName))
                {
                    var edge = new PageEdge()
                    {
                        Name = x.MemberName,
                        From = nodes.First(n => n.IndexedName == x.FromPageName),
                        To = nodes.First(n => n.IndexedName == x.ToPageName),
                        IsLink = x.IsLink,
                        IsForm = x.IsForm
                    };
                    edges.Add(edge);
                }
            }
        }
    }
}