namespace CocoriCore.PageLogs
{
    public class HomePageQuery : IPageQuery<HomePage>
    {

    }

    public class HomePage : PageBase<HomePageQuery, HomePage>
    {
        public AsyncCall<SvgResponse> PageGraph;
        public AsyncCall<string[]> PageNames;
        public Form<RunTestCommand, HomePageQuery> RunTests;
    }

    public class HomePageModule : PageModule
    {
        public HomePageModule()
        {
            On<HomePageQuery>()
                .ProvideQuery<PageGraphQuery>((p, q) => { })
                .WithResponse<SvgResponse>()
                .ToModel<SvgResponse>((q, r, m) => { });

            MapForm<RunTestCommand, RunTestResponse, HomePageQuery>(
                (c, r) => new HomePageQuery()
            );

            HandlePage<HomePageQuery, HomePage>();
        }

    }
}