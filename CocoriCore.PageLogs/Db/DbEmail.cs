using System.Threading.Tasks;
using CocoriCore.Page;

namespace CocoriCore.PageLogs
{
    public class DbEmail
    {
        private readonly IRepository repository;

        public DbEmail(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task Insert(DbInsertContext context, LogEmailRead l)
        {

            await repository.InsertAsync(new TestEmailRead()
            {
                TestName = context.TestName,
                IndexInTest = context.IndexInTest,
                UserName = context.UserName,
                ScenarioNames = context.ScenarioNames,
                EmailName = l.MailMessage.Body.GetType().GetFriendlyName()
            });
        }

        public async Task Insert(DbInsertContext context, LogEmailSent l)
        {
            await repository.InsertAsync(new TestEmailSent()
            {
                TestName = context.TestName,
                IndexInTest = context.IndexInTest,
                UserName = context.UserName,
                ScenarioNames = context.ScenarioNames,
                PageName = context.PageName,
                MessageName = context.MessageName,
                EmailName = l.MailMessage.Body.GetType().GetFriendlyName()
            });
        }
    }
}