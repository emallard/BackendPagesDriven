using System;
using System.Linq;
using System.Threading.Tasks;
using CocoriCore.Page;

namespace CocoriCore.PageLogs
{

    public class LogDatabaseBuilder
    {
        private readonly IRepository repository;

        public LogDatabaseBuilder(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task AddTest(Type testClass, string methodName, object[] userLogs)
        {
            var test = new Test()
            {
                TestClass = testClass,
                TestMethod = methodName,
                TestName = testClass.FullName + "." + methodName
            };

            await repository.InsertAsync(test);

            var groupedByUser = userLogs.OfType<UserLog>().GroupBy(x => x.UserName).Distinct().ToArray();
            foreach (var group in groupedByUser)
            {
                await AddLogs(group.Key, group.ToArray(), test);
            }
        }

        public async Task AddLogs(string userName, UserLog[] userLogs, Test test)
        {
            var context = new InsertContext()
            {
                Test = test
            };
            foreach (var o in userLogs)
                await AddLog(o, context);
        }

        public async Task AddLog(UserLog o, InsertContext context)
        {
            if (o is LogDisplay logDisplay)
                await Insert(context, logDisplay);
            /*
            if (o is LogFollow logFollow)
            {
                var entity = new LogDbPage()
                {
                    TestId = context.Test.Id,
                    PageQuery = logFollow.PageQuery.GetType().Name,
                    PageName = logFollow.PageResponse.GetType().Name,
                };
                await repository.InsertAsync(entity);
            }
            if (o is LogEmailRead logEmailRead)
            {
                var entity = new LogDbEmailRead()
                {
                    TestId = context.Test.Id,
                    MessageName = logEmailRead.MailMessage.GetType().Name

                };
                await repository.InsertAsync(entity);
            }
            if (o is LogEmailSend logEmailSend)
            {
                var entity = new LogDbEmailRead()
                {
                    TestId = context.Test.Id,
                    MessageName = logEmailRead.MailMessage.GetType().Name

                };
                await repository.InsertAsync(entity);
            }
            */

        }

        private async Task Insert(InsertContext context, LogDisplay l)
        {
            var pageType = await GetPageType(l.PageQuery, l.PageResponse);
            await repository.InsertAsync(new TestPage()
            {
                TestId = context.Test.Id,
                UserId = context.User.Id,
                PageTypeId = pageType.Id,
                PageQuery = l.PageQuery,
                HasAsserts = false,
            });
        }

        private async Task<PageType> GetPageType(object pageQuery, object pageResponse)
        {
            var pageType = await repository.LoadAsync<PageType>(p => p.QueryType, pageQuery.GetType());
            if (pageType == null)
            {
                pageType = new PageType()
                {
                    QueryType = pageQuery.GetType(),
                    ResponseType = pageResponse.GetType(),
                    Name = pageResponse.GetType().Name
                };
                await repository.InsertAsync(pageType);
            }
            return pageType;
        }
    }

    public class InsertContext
    {
        public Test Test;
        public TestUser User;
        public TestPage Page;
        public TestMessage Handler;
    }
}