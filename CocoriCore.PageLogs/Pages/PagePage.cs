namespace CocoriCore.PageLogs
{
    public class PagePageQuery : IPageQuery<PagePage>
    {
        public string PageName;
    }

    public class PagePage : PageBase<PagePageQuery>
    {
        public AsyncCall<PageReportQuery, PageReport> PageReport;
    }

    public class PagePageModule : PageModule
    {
        public PagePageModule()
        {
            HandlePage<PagePageQuery, PagePage>((q, p) => { p.PageReport.Query.PageName = q.PageName; })
                .ForAsyncCall(p => p.PageReport)
                .MapResponse<PageReport>()
                .ToSelf();



        }

    }
}