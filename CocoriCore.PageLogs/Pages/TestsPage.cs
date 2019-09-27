namespace CocoriCore.PageLogs
{
    public class TestsPageQuery : IPageQuery<TestsPage>
    {

    }

    public class TestsPage : PageBase<TestsPageQuery, TestsPage>
    {
        public AsyncCall<SvgResponse> PageGraph;
    }

    public class TestsPageModule : PageModule
    {
        public TestsPageModule()
        {
            /*
            On<TestsPageQuery>()
                .ProvideQuery<PageGraphQuery>((p, q) => { })
                .WithResponse<SvgResponse>()
                .ToModel<SvgResponse>((q, r, m) => { });*/

            HandlePage<TestsPageQuery, TestsPage>();
        }
    }
}