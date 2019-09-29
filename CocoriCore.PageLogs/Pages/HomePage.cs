using System.Linq;

namespace CocoriCore.PageLogs
{
    public class HomePageQuery : IPageQuery<HomePage>
    {

    }

    public class HomePage : PageBase<HomePageQuery>
    {
        public Form<RunTestCommand, HomePageQuery> RunTests;
        public AsyncCall<PageListQuery, PageListPageItem[]> Pages;
        public AsyncCall<EntityListQuery, EntityListPageItem[]> Entities;
        public AsyncCall<TestListQuery, TestListPageItem[]> Tests;
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
                .ToModel<PageListPageItem[]>((q, r) =>
                    r.Select(x => new PageListPageItem()
                    {
                        PageName = x,
                        Link = new PagePageQuery() { PageName = x }
                    }).ToArray()
                )

                .ForAsyncCall(p => p.Entities)
                .MapResponse<string[]>()
                .ToModel<EntityListPageItem[]>((q, r) =>
                    r.Select(x => new EntityListPageItem()
                    {
                        EntityName = x,
                        Link = new EntityPageQuery() { EntityName = x }
                    }).ToArray()
                )

                .ForAsyncCall(p => p.Tests)
                .MapResponse<string[]>()
                .ToModel<TestListPageItem[]>((q, r) =>
                    r.Select(x => new TestListPageItem()
                    {
                        TestName = x,
                        Link = new TestPageQuery() { TestName = x }
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