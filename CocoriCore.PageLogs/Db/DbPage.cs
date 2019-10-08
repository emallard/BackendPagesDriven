using System.Linq;
using System.Threading.Tasks;
using CocoriCore.Page;

namespace CocoriCore.PageLogs
{
    public class DbPage
    {
        private readonly IRepository repository;
        private readonly RouteToUrl routeToUrl;

        public DbPage(
            IRepository repository,
            RouteToUrl routeToUrl)
        {
            this.repository = repository;
            this.routeToUrl = routeToUrl;
        }
        public async Task Insert(DbInsertContext context, LogDisplay l)
        {
            var redirection = new TestPageRedirection()
            {
                TestName = context.TestName,
                IndexInTest = context.IndexInTest,
                UserName = context.UserName,
                ScenarioNames = context.ScenarioNames,
                FromPageName = null,
                MemberName = null,
                ToPageName = l.PageQuery.GetType().GetFriendlyName(),
                IsForm = false,
                IsLink = false
            };

            await repository.InsertAsync(redirection);
            context.IndexInTest++;

            await InsertPage(context, l.PageQuery, redirection);
        }



        public async Task Insert(DbInsertContext context, LogFollow l)
        {
            var redirection = new TestPageRedirection()
            {
                TestName = context.TestName,
                IndexInTest = context.IndexInTest,
                UserName = context.UserName,
                ScenarioNames = context.ScenarioNames,
                FromPageName = context.PageName,
                ToPageName = l.PageQuery.GetType().GetFriendlyName(),
                MemberName = l.MemberName,
                IsForm = false,
                IsLink = true
            };

            await repository.InsertAsync(redirection);
            context.IndexInTest++;

            await InsertPage(context, l.PageQuery, redirection);
        }

        public async Task Insert(DbInsertContext context, LogSubmitRedirect l)
        {
            var redirection = new TestPageRedirection()
            {
                TestName = context.TestName,
                IndexInTest = context.IndexInTest,
                UserName = context.UserName,
                ScenarioNames = context.ScenarioNames,
                FromPageName = context.PageName,
                ToPageName = l.PageQuery.GetType().GetFriendlyName(),
                IsForm = true,
                IsLink = false,
                MemberName = context.PageMemberName
            };

            await repository.InsertAsync(redirection);
            context.IndexInTest++;

            await InsertPage(context, l.PageQuery, redirection);
        }
        private async Task InsertPage(DbInsertContext context, IMessage pageQuery, TestPageRedirection redirection)
        {
            var page = new TestPage()
            {
                TestName = context.TestName,
                IndexInTest = context.IndexInTest + 1,
                UserName = context.UserName,
                ScenarioNames = context.ScenarioNames,
                PageName = pageQuery.GetType().GetFriendlyName(),
                PageUrl = routeToUrl.ToParameterizedUrlShort(pageQuery)
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