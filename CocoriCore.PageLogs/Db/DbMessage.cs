using System.Threading.Tasks;
using CocoriCore.Page;

namespace CocoriCore.PageLogs
{
    public class DbMessage
    {
        private readonly IRepository repository;

        public DbMessage(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task Insert(DbInsertContext context, LogMessageBus l)
        {
            await repository.InsertAsync(new TestMessage()
            {
                TestName = context.TestName,
                IndexInTest = context.IndexInTest,
                UserName = context.UserName,
                PageName = context.PageName,
                MessageName = l.Message.GetType().Name
            });
            context.MessageName = l.Message.GetType().Name;
        }
    }
}