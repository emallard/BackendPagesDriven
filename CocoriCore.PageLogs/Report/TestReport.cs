using System;
using System.Linq;
using System.Threading.Tasks;
using CocoriCore.Linq.Async;

namespace CocoriCore.PageLogs
{
    public class TestReportQuery : IMessage<TestReport>
    {
        public string TestName;
    }


    public class TestReport
    {
        public string TestName;
        public object[] Logs;
    }

    public class WithIndex
    {
        public int Index;
        public object Object;

        public WithIndex(int index, object @object)
        {
            Index = index;
            Object = @object;
        }
    }

    public class TestReportBuilder
    {
        private readonly IRepository repository;

        public TestReportBuilder(IRepository repository)
        {
            this.repository = repository;
        }
        public async Task<TestReport> Build(string testName)
        {

            var testReport = new TestReport()
            {
                TestName = testName
            };

            var pages = (await repository.Query<TestPage>()
                .Where(x => x.TestName == testName)
                .ToArrayAsync())
                .Select(x => new WithIndex(x.IndexInTest, x));

            var emailsRead = (await repository.Query<TestEmailRead>()
                .Where(x => x.TestName == testName)
                .ToArrayAsync())
                .Select(x => new WithIndex(x.IndexInTest, x));


            testReport.Logs = pages
                                .Concat(emailsRead)
                                .OrderBy(x => x.Index)
                                .Select(x => x.Object)
                                .ToArray();

            return testReport;
        }
    }



    public class TestReportHandler : MessageHandler<TestReportQuery, TestReport>
    {
        private readonly TestReportBuilder builder;

        public TestReportHandler(TestReportBuilder builder)
        {
            this.builder = builder;
        }

        public async override Task<TestReport> ExecuteAsync(TestReportQuery message)
        {
            return await builder.Build(message.TestName);
        }
    }
}