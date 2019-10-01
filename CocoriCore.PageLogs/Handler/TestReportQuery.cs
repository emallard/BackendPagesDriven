using System;
using System.Collections.Generic;
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
        //public object[] Logs;
        public PagePath Path;
    }

    public class PagePath
    {
        public IPagePathItem[] Items;
    }

    public class IPagePathItem
    {
        public string _Type;
        public IPagePathItem()
        {
            this._Type = GetType().Name;
        }
    }

    public class PagePathPage : IPagePathItem
    {
        public string PageName;
        public bool HasAssert;
    }

    public class PagePathRedirection : IPagePathItem
    {
        public int IndexInTest;
        public LinkModel PageFrom;
        public string PageFromMemberName;
        public LinkModel PageTo;
        public bool PageToHasAssert;
    }

    public class PagePathPageAssert : IPagePathItem
    {
        public string PageName;
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

            var redirections = (await repository.Query<TestPageRedirection>()
                .Where(x => x.TestName == testName)
                .ToArrayAsync())
                .Select(x => new WithIndex(x.IndexInTest, x));

            var emailsRead = (await repository.Query<TestEmailRead>()
                .Where(x => x.TestName == testName)
                .ToArrayAsync())
                .Select(x => new WithIndex(x.IndexInTest, x));

            var items = pages.Concat(redirections).Concat(emailsRead).OrderBy(x => x.Index).Select(x => x.Object).ToArray();
            var pathItems = new List<PagePathRedirection>();
            foreach (var item in items)
            {
                if (item is TestPage page)
                {
                }
                if (item is TestPageRedirection redirection)
                {
                    LinkModel pageFrom = null;
                    if (redirection.FromPageName != null)
                        pageFrom = new LinkModel(
                                        new PagePageQuery() { PageName = redirection.FromPageName },
                                        redirection.FromPageName
                                        );
                    var pageTo = new LinkModel(
                                    new PagePageQuery() { PageName = redirection.ToPageName },
                                    redirection.ToPageName
                                    );

                    pathItems.Add(
                        new PagePathRedirection()
                        {
                            IndexInTest = redirection.IndexInTest,
                            PageFromMemberName = redirection.MemberName,
                            PageFrom = pageFrom,
                            PageTo = pageTo,
                            PageToHasAssert = redirection.ToPageHasAssert
                        }
                    );
                }
            }

            testReport.Path = new PagePath()
            {
                Items = pathItems.ToArray()
            };

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