using System.Linq;
using CocoriCore.Page;

namespace CocoriCore.PageLogs
{
    public class HomePageQuery : IPageQuery<HomePage>
    {

    }

    public class HomePage : PageBase<HomePageQuery>
    {
        public Form<RunTestCommand, HomePageQuery> RunTests;
        public AsyncCall<PageListQuery, PageLink[]> Pages;
        public AsyncCall<EntityListQuery, PageLink[]> Entities;
        public AsyncCall<TestListQuery, PageLink[]> Tests;
        public PageGraphPageQuery ViewPageGraph;
    }

    public class HomePageModule : PageModule
    {
        public HomePageModule()
        {
            HandlePage<HomePageQuery, HomePage>((q, p) =>
            {
                p.ViewPageGraph = new PageGraphPageQuery();
            })

                .ForAsyncCall(p => p.Pages)
                .MapResponse<string[]>()
                .ToModel<PageLink[]>((q, r) =>
                    r.Select(x => new PageLink()
                    {
                        Text = x,
                        PageQuery = new PagePageQuery() { PageName = x }
                    }).ToArray()
                )

                .ForAsyncCall(p => p.Entities)
                .MapResponse<string[]>()
                .ToModel<PageLink[]>((q, r) =>
                    r.Select(x => new PageLink()
                    {
                        Text = x,
                        PageQuery = new EntityPageQuery() { EntityName = x }
                    }).ToArray()
                )

                .ForAsyncCall(p => p.Tests)
                .MapResponse<string[]>()
                .ToModel<PageLink[]>((q, r) =>
                    r.Select(x => new PageLink()
                    {
                        Text = x,
                        PageQuery = new TestPageQuery() { TestName = x }
                    }).ToArray()
                )

                .ForForm(p => p.RunTests)
                .MapResponse<RunTestResponse>()
                .ToModel<HomePageQuery>((c, r, m) => { });


            /*On<HomePageQuery>()
                .ProvideQuery<PageGraphQuery>((p, q) => { })
                .WithResponse<string>()
                .AsModel();

            On<HomePageQuery>()
                .ProvideQuery<PageListQuery>((p, q) => { });
            
            MapForm<RunTestCommand, RunTestResponse, HomePageQuery>(
                (c, r) => new HomePageQuery()
            );
            */

        }

    }
}