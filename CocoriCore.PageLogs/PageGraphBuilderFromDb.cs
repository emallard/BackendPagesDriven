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
            var pageNames = await repository
                                    .Query<TestPage>()
                                    .Select(x => x.PageName)
                                    .Distinct()
                                    .ToArrayAsync();
            return pageNames.Select(x => new PageNode()
            {
                ParameterizedUrl = x,
                IndexedName = x
            }).ToList();

        }

        private async Task<List<PageEdge>> GetPageEdgesAsync(List<PageNode> nodes)
        {
            var edges = new List<PageEdge>();
            var testNames = await repository.Query<Test>().Select(x => x.TestName).Distinct().ToArrayAsync();
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
                if (!edges.Any(e => e.Name == x.MemberName
                                 && e.From.ParameterizedUrl == x.PageName
                                 && e.To.ParameterizedUrl == x.ToPageName))
                {
                    var edge = new PageEdge()
                    {
                        Name = x.MemberName,
                        From = nodes.First(n => n.ParameterizedUrl == x.PageName),
                        To = nodes.First(n => n.ParameterizedUrl == x.ToPageName),
                        IsLink = x.IsLink,
                        IsForm = x.IsForm
                    };
                }
            }
        }
    }
}