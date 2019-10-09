using System.Linq;
using CocoriCore.Page;

namespace CocoriCore.PageLogs
{
    public class PagePageQuery : IPageQuery<PagePage>
    {
        public string PageName;
    }

    public class PagePage : PageBase<PagePageQuery>
    {
        public AsyncCall<PageReportQuery, PageReportModel> PageReport;
        public AsyncCall<PageGraphQuery, PageGraphResponse> PageGraph;
        public AsyncCall<PageListQuery, PageLink[]> Pages;

        public PagePage()
        {
            AddRenderInfo(this, x => x.Pages, "hidden");
        }
    }

    public class PageReportModel
    {
        public string PageName;
        public PageReportRepositoryItemModel[] RepositoryItems;
        //public PageReportEmailItem[] Emails;
        public PageLink<TestPageQuery>[] TestNames;
    }

    public class PageReportRepositoryItemModel
    {
        public LogRepoOperation Operation;
        public PageLink<EntityPageQuery> EntityName;
        public string MessageName;
        public PageLink<TestPageQuery>[] TestNames;
    }

    public class PagePageModule : PageModule
    {
        public PagePageModule()
        {
            HandlePage<PagePageQuery, PagePage>((q, p) => { p.PageReport.Query.PageName = q.PageName; });

            OnMessage<PageReportQuery>()
            .WithResponse<PageReport>()
            .BuildModel<PageReportModel>((q, r, m) =>
            {
                m.PageName = r.PageName;
                m.RepositoryItems = r.RepositoryItems.Select(x => new PageReportRepositoryItemModel()
                {
                    Operation = x.Operation,
                    EntityName = new PageLink<EntityPageQuery>(new EntityPageQuery() { EntityName = x.EntityName }, x.EntityName),
                    MessageName = x.MessageName,
                    TestNames = x.TestNames.Select(t => new PageLink<TestPageQuery>(new TestPageQuery() { TestName = t }, t)).ToArray()
                }).ToArray();
                m.TestNames = r.TestNames.Select(t => new PageLink<TestPageQuery>(new TestPageQuery() { TestName = t }, t)).ToArray();

            });



        }

    }
}