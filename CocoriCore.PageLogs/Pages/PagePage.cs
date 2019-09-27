namespace CocoriCore.PageLogs
{
    public class PagePageQuery : IPageQuery<PagePage>
    {
        public string PageName;
    }

    public class PagePage : PageBase<PagePageQuery, PagePage>
    {
        public AsyncCall<PageReport> PageReport;
    }

    public class PagePageModule : PageModule
    {
        public PagePageModule()
        {
            On<PagePageQuery>()
                .ProvideQuery<PageReportQuery>((p, q) => { q.PageName = p.PageName; })
                .WithResponse<PageReport>()
                .AsModel();


            HandlePage<PagePageQuery, PagePage>();
        }

    }
}