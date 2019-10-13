namespace CocoriCore.PageLogs
{
    public class TestListPageQuery : IPageQuery<TestListPage>
    {

    }

    public class TestListPage : PageBase<TestListPageQuery>
    {
        public OnInitCall<PageGraphQuery, PageGraphResponse> PageGraph;
    }

    public class TestListPageModule : PageModule
    {
        public TestListPageModule()
        {
            HandlePage<TestListPageQuery, TestListPage>();
        }
    }
}