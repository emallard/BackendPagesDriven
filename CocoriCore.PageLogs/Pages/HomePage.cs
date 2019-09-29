using System.Linq;

namespace CocoriCore.PageLogs
{
    public class HomePageQuery : IPageQuery<HomePage>
    {

    }

    public class HomePage : PageBase<HomePageQuery>
    {
        public Form<RunTestCommand, HomePageQuery> RunTests;
        public AsyncCall<PageListQuery, PageListPageItem[]> PageNames;
        public AsyncCall<EntityListQuery, EntityListPageItem[]> EntityNames;

    }

    public class HomePageModule : PageModule
    {
        public HomePageModule()
        {
            HandlePage<HomePageQuery, HomePage>((p, q) => { })

                .ForAsyncCall(p => p.PageNames)
                .MapResponse<string[]>()
                .ToModel<PageListPageItem[]>((q, r) =>
                    r.Select(x => new PageListPageItem()
                    {
                        PageName = x,
                        Link = new PagePageQuery() { PageName = x }
                    }).ToArray()
                )

                .ForAsyncCall(p => p.EntityNames)
                .MapResponse<string[]>()
                .ToModel<EntityListPageItem[]>((q, r) =>
                    r.Select(x => new EntityListPageItem()
                    {
                        EntityName = x,
                        Link = new EntityPageQuery() { EntityName = x }
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