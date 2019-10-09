using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CocoriCore.Linq.Async;
using CocoriCore.Page;

namespace CocoriCore.PageLogs
{
    public class TestRedirectionListQuery : IMessage<TestRedirectionListResponseItem[]>
    {
        public string TestName;
    }

    public class TestRedirectionListResponseItem
    {
        public int IndexInTest;
        public PageLink PageFrom;
        public string PageFromMemberName;
        public PageLink PageTo;
        public bool PageToHasAssert;
    }
    public class TestRedirectionListHandler : MessageHandler<TestRedirectionListQuery, TestRedirectionListResponseItem[]>
    {
        private readonly IRepository repository;

        public TestRedirectionListHandler(IRepository repository)
        {
            this.repository = repository;
        }
        public override async Task<TestRedirectionListResponseItem[]> ExecuteAsync(TestRedirectionListQuery message)
        {
            var redirections = (await repository.Query<TestPageRedirection>()
                .Where(x => x.TestName == message.TestName
                         && x.ScenarioNames == "")
                .ToArrayAsync())
                .Select(x => new WithIndex(x.IndexInTest, x));

            var emailsRead = (await repository.Query<TestEmailRead>()
                .Where(x => x.TestName == message.TestName
                         && x.ScenarioNames == "")
                .ToArrayAsync())
                .Select(x => new WithIndex(x.IndexInTest, x));

            var items = redirections.Concat(emailsRead).OrderBy(x => x.Index).Select(x => x.Object).ToArray();
            var pathItems = new List<TestRedirectionListResponseItem>();
            foreach (var item in items)
            {
                if (item is TestPageRedirection redirection)
                {
                    PageLink pageFrom = null;
                    if (redirection.FromPageName != null)
                        pageFrom = new PageLink(
                                        new PagePageQuery() { PageName = redirection.FromPageName },
                                        redirection.FromPageName
                                        );
                    var pageTo = new PageLink(
                                    new PagePageQuery() { PageName = redirection.ToPageName },
                                    redirection.ToPageName
                                    );

                    pathItems.Add(
                        new TestRedirectionListResponseItem
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
            return pathItems.ToArray();
        }
    }
}