namespace CocoriCore.PageLogs
{
    public class TestPageQuery : IPageQuery<TestPageResponse>
    {
        public string TestName;
    }

    public class TestPageResponse : PageBase<TestPageQuery>
    {
        public AsyncCall<TestReportQuery, TestReport> TestReport;
        public AsyncCall<PageGraphQuery, PageGraphResponse> PageGraph;
        public AsyncCall<TestCsFileQuery, TestCsFileResponse> CsFile;
        public AsyncCall<RunWithSeleniumCommand, Empty> RunWithSelenium;
    }

    public class TestPageModule : PageModule
    {
        public TestPageModule()
        {
            HandlePage<TestPageQuery, TestPageResponse>((q, p) =>
            {
                p.TestReport.Query.TestName = q.TestName;

                p.RunWithSelenium.Query.TestName = q.TestName;
                p.RunWithSelenium.OnInit = false;

                p.CsFile.Query.TestName = q.TestName;
            });
        }

    }
}