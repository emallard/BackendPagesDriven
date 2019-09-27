using System.Linq;

namespace CocoriCore.PageLogs
{
    public class PagesPageQuery : IPageQuery<PagesPage>
    {

    }

    public class PagesPage : PageBase<PagesPageQuery, PagesPage>
    {
        public AsyncCall<PagesPageItem[]> PageGraph;
    }

    public class PagesPageItem
    {
        public PagePageQuery Link;
        public string PageName;
    }

    public class PagesPageModule : PageModule
    {
        public PagesPageModule()
        {
            On<PagesPageQuery>()
                .ProvideQuery<PageListQuery>((p, q) => { })
                .WithResponse<string[]>()
                .ToModel<PagesPageItem[]>((q, r) =>
                    r.Select(x => new PagesPageItem()
                    {
                        PageName = x,
                        Link = new PagePageQuery() { PageName = x }
                    }).ToArray()
                );


            HandlePage<PagesPageQuery, PagesPage>();
        }

    }
}