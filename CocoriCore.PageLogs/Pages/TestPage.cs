namespace CocoriCore.PageLogs
{
    public class TestPageQuery : IPageQuery<TestPageResponse>
    {
        public string TestName;
    }

    public class TestPageResponse : PageBase<TestPageQuery>
    {
        public OnInitCall<TestReportQuery, TestReport> TestReport;
        public OnInitCall<PageGraphQuery, PageGraphResponse> PageGraph;
        public OnInitCall<TestCsFileQuery, TestCsFileResponse> CsFile;
        public ActionCall<RunWithSeleniumCommand, Empty> RunWithSelenium;
    }

    public class TestPageModule : PageModule
    {
        public TestPageModule()
        {
            HandlePage<TestPageQuery, TestPageResponse>((q, p) =>
            {
                p.TestReport.Message.TestName = q.TestName;

                p.RunWithSelenium.Message.TestName = q.TestName;

                p.CsFile.Message.TestName = q.TestName;
            });
        }

    }
}