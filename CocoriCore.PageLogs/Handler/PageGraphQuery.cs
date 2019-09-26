using System;
using System.Threading.Tasks;
using CocoriCore.Page;

namespace CocoriCore.PageLogs
{
    public class PageGraphQuery : IMessage<SvgResponse>
    {
    }

    public class PageGraphHandler : MessageHandler<PageGraphQuery, SvgResponse>
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

        public override Task<SvgResponse> ExecuteAsync(PageGraphQuery message)
        {
            throw new Exception();
        }
    }
}