namespace CocoriCore.PageLogs
{
    public class TestPageQuery : IPageQuery<TestPageResponse>
    {
        public string TestName;
    }

    public class TestPageResponse : PageBase<TestPageQuery>
    {
        public AsyncCall<TestReportQuery, TestReport> TestReport;
    }

    public class TestPageModule : PageModule
    {
        public TestPageModule()
        {
            HandlePage<TestPageQuery, TestPageResponse>((q, p) =>
            {
                p.TestReport.Query.TestName = q.TestName;
            })
                .ForAsyncCall(p => p.TestReport)
                .MapResponse<TestReport>()
                .ToSelf();
        }

    }
}