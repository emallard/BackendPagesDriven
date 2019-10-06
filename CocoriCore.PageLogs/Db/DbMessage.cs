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
            var messageName = l.Message.GetType().GetFriendlyName();
            await repository.InsertAsync(new TestMessage()
            {
                TestName = context.TestName,
                IndexInTest = context.IndexInTest,
                UserName = context.UserName,
                ScenarioNames = context.ScenarioNames,
                PageName = context.PageName,
                MessageName = messageName
            });
            context.MessageName = messageName;
        }
    }
}