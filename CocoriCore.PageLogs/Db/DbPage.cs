using System.Linq;
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
            var page = new TestPage()
            {
                TestName = context.TestName,
                IndexInTest = context.IndexInTest,
                UserName = context.UserName,
                PageName = l.PageQuery.GetType().FullName
            };

            await repository.InsertAsync(page);
            context.PageName = page.PageName;
            context.Page = page;
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
                ToPageName = l.PageQuery.GetType().FullName,
                MemberName = l.MemberName,
                IsForm = false,
                IsLink = true
            });
            context.IndexInTest++;

            var page = new TestPage()
            {
                TestName = context.TestName,
                IndexInTest = context.IndexInTest,
                UserName = context.UserName,
                PageName = l.PageQuery.GetType().FullName
            };


            await repository.InsertAsync(page);

            context.PageName = page.PageName;
            context.Page = page;
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
                ToPageName = l.PageQuery.GetType().FullName,
                IsForm = true,
                IsLink = false,
                MemberName = context.PageMemberName
            });
            context.IndexInTest++;

            var page = new TestPage()
            {
                TestName = context.TestName,
                IndexInTest = context.IndexInTest,
                UserName = context.UserName,
                PageName = l.PageQuery.GetType().FullName,
            };

            await repository.InsertAsync(page);
            context.PageName = page.PageName;
            context.Page = page;
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

        public async Task Insert(DbInsertContext context, LogAssert l)
        {
            var page = context.Page;
            page.HasAssert = true;
            await repository.UpdateAsync(page);
        }
    }
}