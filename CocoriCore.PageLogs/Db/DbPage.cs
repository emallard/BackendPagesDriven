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
            var redirection = new TestPageRedirection()
            {
                TestName = context.TestName,
                IndexInTest = context.IndexInTest,
                UserName = context.UserName,
                FromPageName = null,
                MemberName = null,
                ToPageName = l.PageQuery.GetType().GetFriendlyName(),
                IsForm = false,
                IsLink = false
            };

            await repository.InsertAsync(redirection);
            context.IndexInTest++;

            var page = new TestPage()
            {
                TestName = context.TestName,
                IndexInTest = context.IndexInTest + 1,
                UserName = context.UserName,
                PageName = l.PageQuery.GetType().GetFriendlyName()
            };

            await repository.InsertAsync(page);

            context.PageName = page.PageName;
            context.Page = page;
            context.Redirection = redirection;
            context.PageMemberName = null;
            context.MessageName = null;
        }

        public async Task Insert(DbInsertContext context, LogFollow l)
        {
            var redirection = new TestPageRedirection()
            {
                TestName = context.TestName,
                IndexInTest = context.IndexInTest,
                UserName = context.UserName,
                FromPageName = context.PageName,
                ToPageName = l.PageQuery.GetType().GetFriendlyName(),
                MemberName = l.MemberName,
                IsForm = false,
                IsLink = true
            };

            await repository.InsertAsync(redirection);
            context.IndexInTest++;

            var page = new TestPage()
            {
                TestName = context.TestName,
                IndexInTest = context.IndexInTest,
                UserName = context.UserName,
                PageName = l.PageQuery.GetType().GetFriendlyName()
            };


            await repository.InsertAsync(page);

            context.PageName = page.PageName;
            context.Page = page;
            context.Redirection = redirection;
            context.PageMemberName = null;
            context.MessageName = null;
        }

        public async Task Insert(DbInsertContext context, LogSubmitRedirect l)
        {
            var redirection = new TestPageRedirection()
            {
                TestName = context.TestName,
                IndexInTest = context.IndexInTest,
                UserName = context.UserName,
                FromPageName = context.PageName,
                ToPageName = l.PageQuery.GetType().GetFriendlyName(),
                IsForm = true,
                IsLink = false,
                MemberName = context.PageMemberName
            };

            await repository.InsertAsync(redirection);
            context.IndexInTest++;

            var page = new TestPage()
            {
                TestName = context.TestName,
                IndexInTest = context.IndexInTest,
                UserName = context.UserName,
                PageName = l.PageQuery.GetType().GetFriendlyName(),
            };

            await repository.InsertAsync(page);
            context.PageName = page.PageName;
            context.Page = page;
            context.Redirection = redirection;
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

            context.Redirection.ToPageHasAssert = true;
            await repository.UpdateAsync(context.Redirection);
        }
    }
}