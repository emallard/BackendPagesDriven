using System;
using System.Linq;
using System.Threading.Tasks;
using CocoriCore.Page;

namespace CocoriCore.PageLogs
{

    public partial class DbBuilder
    {
        private readonly IRepository repository;
        private readonly DbPage dbPage;
        private readonly DbEmail dbEmail;
        private readonly DbEntity dbEntity;
        private readonly DbMessage dbMessage;

        public DbBuilder(
            IRepository repository,
            DbPage dbPage,
            DbEmail dbEmail,
            DbEntity dbEntity,
            DbMessage dbMessage)
        {
            this.repository = repository;
            this.dbPage = dbPage;
            this.dbEmail = dbEmail;
            this.dbEntity = dbEntity;
            this.dbMessage = dbMessage;
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
            var context = new DbInsertContext()
            {
                TestName = test.TestName,
                UserName = userName,
                IndexInTest = 0
            };
            foreach (var o in userLogs)
                await AddLog(o, context);
        }

        public async Task AddLog(UserLog o, DbInsertContext context)
        {
            if (o is LogAsyncCall logAsyncCall)
                await dbPage.Insert(context, logAsyncCall);
            if (o is LogDisplay logDisplay)
                await dbPage.Insert(context, logDisplay);
            if (o is LogEmailRead logEmailRead)
                await dbEmail.Insert(context, logEmailRead);
            if (o is LogEmailSent logEmailSent)
                await dbEmail.Insert(context, logEmailSent);
            if (o is LogFollow logFollow)
                await dbPage.Insert(context, logFollow);
            if (o is LogMessageBus logMessageBus)
                await dbMessage.Insert(context, logMessageBus);
            if (o is LogRepo logRepo)
                await dbEntity.Insert(context, logRepo);
            if (o is LogSubmit logSubmit)
                await dbPage.Insert(context, logSubmit);
            if (o is LogSubmitRedirect logSubmitRedirect)
                await dbPage.Insert(context, logSubmitRedirect);



            context.IndexInTest++;
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
    }
}