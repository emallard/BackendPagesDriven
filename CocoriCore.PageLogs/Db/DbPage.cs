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
            context.PageMemberName = null;
            context.MessageName = null;
        }

        public async Task Insert(DbInsertContext context, LogFollow l)
        {
            await repository.InsertAsync(new TestPageRedirection()
            {
                TestName = context.TestName,
                IndexInTest = context.IndexInTest,
                UserName = context.UserName,
                PageName = context.PageName,
                ToPageName = l.PageQuery.GetType().Name,
                MemberName = l.MemberName,
                IsForm = false,
                IsLink = true
            });
            context.IndexInTest++;

            await repository.InsertAsync(new TestPage()
            {
                TestName = context.TestName,
                IndexInTest = context.IndexInTest,
                UserName = context.UserName,
                PageName = l.PageQuery.GetType().Name
            });

            context.PageName = l.PageQuery.GetType().Name;
            context.PageMemberName = null;
            context.MessageName = null;
        }

        public async Task Insert(DbInsertContext context, LogSubmitRedirect l)
        {
            await repository.InsertAsync(new TestPageRedirection()
            {
                TestName = context.TestName,
                IndexInTest = context.IndexInTest,
                UserName = context.UserName,
                PageName = context.PageName,
                ToPageName = l.PageQuery.GetType().Name,
                IsForm = true,
                IsLink = false,
                MemberName = context.PageMemberName
            });
            context.IndexInTest++;

            await repository.InsertAsync(new TestPage()
            {
                TestName = context.TestName,
                IndexInTest = context.IndexInTest,
                UserName = context.UserName,
                PageName = l.PageQuery.GetType().Name,
            });
            context.PageName = l.PageQuery.GetType().Name;
            context.PageMemberName = null;
            context.MessageName = null;
        }

        public async Task Insert(DbInsertContext context, LogSubmit l)
        {
            await Task.CompletedTask;
            context.PageMemberName = l.MemberName;
        }

        public async Task Insert(DbInsertContext context, LogAsyncCall l)
        {
            await Task.CompletedTask;
            context.PageMemberName = l.MemberName;
        }
    }
}