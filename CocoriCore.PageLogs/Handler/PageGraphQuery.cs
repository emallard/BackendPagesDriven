using System;
using System.Threading.Tasks;
using CocoriCore.Page;

namespace CocoriCore.PageLogs
{
    public class PageGraphQuery : IMessage<string>
    {
    }

    public class PageGraphHandler : MessageHandler<PageGraphQuery, string>
    {
        private readonly IRepository repository;
        private readonly PageGraphFormatter pageGraphFormatter;

        public PageGraphHandler(
            IRepository repository,
            PageGraphFormatter pageGraphFormatter)
        {
            this.repository = repository;
            this.pageGraphFormatter = pageGraphFormatter;
        }

        public override async Task<string> ExecuteAsync(PageGraphQuery message)
        {
            await Task.CompletedTask;
            return "<svg></svg>";
        }
    }
}