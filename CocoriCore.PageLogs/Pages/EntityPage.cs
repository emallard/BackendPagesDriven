namespace CocoriCore.PageLogs
{
    public class EntityPageQuery : IPageQuery<EntityPage>
    {
        public string EntityName;
    }

    public class EntityPage : PageBase<EntityPageQuery>
    {
        public OnInitCall<EntityReportQuery, EntityReport> EntityReport;
    }

    public class EntityPageModule : PageModule
    {
        public EntityPageModule()
        {
            HandlePage<EntityPageQuery, EntityPage>((q, p) =>
            {
                p.EntityReport.Message.EntityName = q.EntityName;
            })
                .ForAsyncCall(p => p.EntityReport)
                .MapResponse<EntityReport>()
                .ToSelf();
        }

    }
}