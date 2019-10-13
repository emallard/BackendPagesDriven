using System.Linq;

namespace CocoriCore.PageLogs
{
    public class EntityListPageQuery : IPageQuery<EntityListPage>
    {

    }

    public class EntityListPage : PageBase<EntityListPageQuery>
    {
        public OnInitCall<PageGraphQuery, EntityListPageItem[]> PageGraph;
    }

    public class EntityListPageItem
    {
        public EntityPageQuery Link;
        public string EntityName;
    }

    public class EntityListPageModule : PageModule
    {
        public EntityListPageModule()
        {
            HandlePage<EntityListPageQuery, EntityListPage>()
                .ForAsyncCall(p => p.PageGraph);
        }

    }
}