namespace CocoriCore.PageLogs
{
    public class HomePageQuery : IPageQuery<HomePage>
    {

    }

    public class HomePage : PageBase<HomePageQuery, HomePage>
    {
        public AsyncCall<SvgResponse> PageGraph;

        //public AsyncCall<SvgResponse> EntityGraph;
        //public AsyncCall<ListQuery<Test>> Tests;
        /*
        public AsyncCall<EntityTypeListReponseItem[]> Entities;
        public AsyncCall<PageTypeListReponseItem[]> Pages;
        */
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