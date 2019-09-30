using System;
using System.Linq;
using System.Threading.Tasks;
using CocoriCore.Linq.Async;
using CocoriCore.Page;

namespace CocoriCore.PageLogs
{
    public class EntityReportQuery : IMessage<EntityReport>
    {
        public string EntityName;
    }


    public class EntityReport
    {
        public string EntityName;
        public EntityReportItem[] RepositoryItems;
        //public EntityReportEmailItem[] Emails;
    }

    public class EntityReportItem
    {
        public LogRepoOperation Operation;
        public string MessageName;
        public string PageName;
        public string[] TestNames;
    }

    public class EntityReportBuilder
    {
        private readonly IRepository repository;

        public EntityReportBuilder(IRepository repository)
        {
            this.repository = repository;
        }
        public async Task<EntityReport> Build(string entityName)
        {

            var entityReport = new EntityReport()
            {
                EntityName = entityName
            };

            var operationGroups = (await repository.Query<TestRepositoryOperation>()
                .Where(x => x.EntityName == entityName)
                .ToArrayAsync())
                .GroupBy(x => Tuple.Create(x.Operation, x.MessageName, x.PageName))
                .ToArray();


            entityReport.RepositoryItems =
                operationGroups
                    .Select(x => new EntityReportItem()
                    {
                        Operation = x.Key.Item1,
                        MessageName = x.Key.Item2,
                        PageName = x.Key.Item3,
                        TestNames = x.Select(t => t.TestName).Distinct().ToArray()
                    })
                    .ToArray();

            return entityReport;
        }
    }



    public class EntityReportHandler : MessageHandler<EntityReportQuery, EntityReport>
    {
        private readonly EntityReportBuilder builder;

        public EntityReportHandler(EntityReportBuilder builder)
        {
            this.builder = builder;
        }

        public async override Task<EntityReport> ExecuteAsync(EntityReportQuery message)
        {
            return await builder.Build(message.EntityName);
        }
    }
}