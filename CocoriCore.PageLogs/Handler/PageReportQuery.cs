using System;
using System.Linq;
using System.Threading.Tasks;
using CocoriCore.Linq.Async;
using CocoriCore.Page;

namespace CocoriCore.PageLogs
{
    public class PageReportQuery : IMessage<PageReport>
    {
        public string PageName;
    }


    public class PageReport
    {
        public string PageName;
        public PageReportRepositoryItem[] RepositoryItems;
        //public PageReportEmailItem[] Emails;

        public string[] TestNames;
    }

    public class PageReportRepositoryItem
    {
        public LogRepoOperation Operation;
        public string EntityName;
        public string MessageName;
        public string[] TestNames;
    }

    public class PageReportEmailItem
    {
        public string EmailName;
        public string MessageName;
        public string[] TestNames;
    }

    public class PageReportBuilder
    {
        private readonly IRepository repository;

        public PageReportBuilder(IRepository repository)
        {
            this.repository = repository;
        }
        public async Task<PageReport> Build(string pageName)
        {

            var pageReport = new PageReport();

            var operationGroups = (await repository.Query<TestRepositoryOperation>()
                .Where(x => x.PageName == pageName)
                .ToArrayAsync())
                .GroupBy(x => Tuple.Create(x.Operation, x.EntityName, x.MessageName))
                .ToArray();


            pageReport.RepositoryItems =
                operationGroups
                    .Select(x => new PageReportRepositoryItem()
                    {
                        Operation = x.Key.Item1,
                        EntityName = x.Key.Item2,
                        MessageName = x.Key.Item3,
                        TestNames = x.Select(t => t.TestName).Distinct().ToArray()
                    })
                    .ToArray();


            pageReport.TestNames =
                (await repository.Query<TestPage>().Where(x => x.PageName == pageName)
                .Select(x => x.TestName)
                .ToArrayAsync())
                .Distinct()
                .ToArray();


            return pageReport;
        }
    }



    public class PageReportHandler : MessageHandler<PageReportQuery, PageReport>
    {
        private readonly PageReportBuilder builder;

        public PageReportHandler(PageReportBuilder builder)
        {
            this.builder = builder;
        }

        public async override Task<PageReport> ExecuteAsync(PageReportQuery message)
        {
            return await builder.Build(message.PageName);
        }
    }
}