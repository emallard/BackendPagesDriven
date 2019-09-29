using System.Linq;

namespace CocoriCore.PageLogs
{
    public class PageListPageQuery : IPageQuery<PageListPage>
    {

    }

    public class PageListPage : PageBase<PageListPageQuery>
    {
        public AsyncCall<PageGraphQuery, PageListPageItem[]> PageGraph;
    }

    public class PageListPageItem
    {
        public PagePageQuery Link;
        public string PageName;
    }

    public class PageListPageModule : PageModule
    {
        public PageListPageModule()
        {
            HandlePage<PageListPageQuery, PageListPage>()
                .ForAsyncCall(p => p.PageGraph);
        }

    }
}