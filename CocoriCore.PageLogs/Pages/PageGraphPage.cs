using System.Linq;

namespace CocoriCore.PageLogs
{
    public class PageGraphPageQuery : IPageQuery<PageGraphPage>
    {

    }

    public class PageGraphPage : PageBase<PageGraphPageQuery>
    {
        public OnInitCall<PageGraphQuery, PageGraphResponse> Graph;
    }

    public class PageGraphPageModule : PageModule
    {
        public PageGraphPageModule()
        {
            HandlePage<PageGraphPageQuery, PageGraphPage>();
        }
    }
}