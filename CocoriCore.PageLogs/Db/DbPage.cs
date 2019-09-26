using System.Threading.Tasks;
using CocoriCore.Page;

namespace CocoriCore.PageLogs
{
    public class DbPage
    {
        private readonly IRepository repository;

        public DbPage(IRepository repository)
        {
            this.repository = repository;
        }
        public async Task Insert(DbInsertContext context, LogDisplay l)
        {
            await repository.InsertAsync(new TestPage()
            {
                TestName = context.TestName,
                IndexInTest = context.IndexInTest,
                UserName = context.UserName,
                PageName = l.PageQuery.GetType().Name
            });
            context.PageName = l.PageQuery.GetType().Name;
            context.MessageName = null;
        }

        public async Task Insert(DbInsertContext context, LogFollow l)
        {
            await repository.InsertAsync(new TestPage()
            {
                TestName = context.TestName,
                IndexInTest = context.IndexInTest,
                UserName = context.UserName,
                PageName = l.PageQuery.GetType().Name
            });
            context.PageName = l.PageQuery.GetType().Name;
            context.MessageName = null;
        }

        public async Task Insert(DbInsertContext context, LogSubmitRedirect l)
        {
            await repository.InsertAsync(new TestPage()
            {
                TestName = context.TestName,
                IndexInTest = context.IndexInTest,
                UserName = context.UserName,
                PageName = l.PageQuery.GetType().Name,
            });
            context.PageName = l.PageQuery.GetType().Name;
            context.MessageName = null;
        }
    }
}