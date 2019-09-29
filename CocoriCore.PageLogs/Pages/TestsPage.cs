namespace CocoriCore.PageLogs
{
    public class TestListPageQuery : IPageQuery<TestListPage>
    {

    }

    public class TestListPage : PageBase<TestListPageQuery>
    {
        public AsyncCall<PageGraphQuery, PageGraphResponse> PageGraph;
    }

    public class TestListPageItem
    {
        public TestPageQuery Link;
        public string TestName;
    }

    public class TestListPageModule : PageModule
    {
        public TestListPageModule()
        {
            HandlePage<TestListPageQuery, TestListPage>();
        }
    }
}