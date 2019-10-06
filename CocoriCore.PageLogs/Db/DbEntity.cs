using System.Threading.Tasks;
using CocoriCore.Page;

namespace CocoriCore.PageLogs
{
    public class DbEntity
    {
        private readonly IRepository repository;

        public DbEntity(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task Insert(DbInsertContext context, LogRepo l)
        {
            await repository.InsertAsync(new TestRepositoryOperation()
            {
                TestName = context.TestName,
                IndexInTest = context.IndexInTest,
                UserName = context.UserName,
                ScenarioNames = context.ScenarioNames,
                PageName = context.PageName,
                MessageName = context.MessageName,
                EntityName = l.EntityType.GetFriendlyName(),
                Operation = l.Operation
            });
        }
    }
}