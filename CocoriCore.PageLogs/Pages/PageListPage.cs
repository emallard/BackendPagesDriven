using System.Linq;

namespace CocoriCore.PageLogs
{
    public class PageListPageQuery : IPageQuery<PageListPage>
    {

    }

    public class PageListPage : PageBase<PageListPageQuery, PageListPage>
    {
        public AsyncCall<PageListPageItem[]> PageGraph;
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
            On<PageListPageQuery>()
                .ProvideQuery<PageListQuery>((p, q) => { })
                .WithResponse<string[]>()
                .ToModel<PageListPageItem[]>((q, r) =>
                    r.Select(x => new PageListPageItem()
                    {
                        PageName = x,
                        Link = new PagePageQuery() { PageName = x }
                    }).ToArray()
                );


            HandlePage<PageListPageQuery, PageListPage>();
        }

    }
}