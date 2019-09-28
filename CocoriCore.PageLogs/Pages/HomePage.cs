using System.Linq;

namespace CocoriCore.PageLogs
{
    public class HomePageQuery : IPageQuery<HomePage>
    {

    }

    public class HomePage : PageBase<HomePageQuery, HomePage>
    {
        //public AsyncCall<string> PageGraph;
        public AsyncCall<PageListPageItem[]> PageNames;
        public Form<RunTestCommand, HomePageQuery> RunTests;
    }

    public class HomePageModule : PageModule
    {
        public HomePageModule()
        {
            On<HomePageQuery>()
                .ProvideQuery<PageGraphQuery>((p, q) => { })
                .WithResponse<string>()
                .AsModel();

            On<HomePageQuery>()
                .ProvideQuery<PageListQuery>((p, q) => { });

            MapForm<RunTestCommand, RunTestResponse, HomePageQuery>(
                (c, r) => new HomePageQuery()
            );

            HandlePage<HomePageQuery, HomePage>();
        }

    }
}