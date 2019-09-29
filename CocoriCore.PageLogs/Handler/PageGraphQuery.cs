using System;
using System.Threading.Tasks;
using CocoriCore.Page;

namespace CocoriCore.PageLogs
{
    public class PageGraphQuery : IMessage<PageGraphResponse>
    {
    }

    public class PageGraphResponse
    {
        public bool IsSvg = true;
        public string Svg;
    }

    public class PageGraphHandler : MessageHandler<PageGraphQuery, PageGraphResponse>
    {
        private readonly IRepository repository;
        private readonly PageGraphBuilderFromDb builderFromDb;
        private readonly PageGraphFormatter pageGraphFormatter;

        public PageGraphHandler(
            IRepository repository,
            PageGraphBuilderFromDb builderFromDb,
            PageGraphFormatter pageGraphFormatter)
        {
            this.repository = repository;
            this.builderFromDb = builderFromDb;
            this.pageGraphFormatter = pageGraphFormatter;
        }

        public override async Task<PageGraphResponse> ExecuteAsync(PageGraphQuery message)
        {
            var graph = await builderFromDb.Build();
            var svg = pageGraphFormatter.LinksAndForms(graph);
            svg = svg.Substring(svg.IndexOf("<svg"));
            return new PageGraphResponse() { Svg = svg };
        }
    }
}