using System;
using System.Linq;
using System.Threading.Tasks;
using CocoriCore.Linq.Async;
using CocoriCore.Page;

namespace CocoriCore.PageLogs
{
    public class PageListQuery : IMessage<string[]>
    {
    }
    public class PageListHandler : MessageHandler<PageListQuery, string[]>
    {
        private readonly IRepository repository;

        public PageListHandler(IRepository repository)
        {
            this.repository = repository;
        }
        public override async Task<string[]> ExecuteAsync(PageListQuery message)
        {
            return (await repository.Query<TestPage>().ToArrayAsync())
                .Select(x => x.PageName).Distinct().ToArray();
        }
    }
}