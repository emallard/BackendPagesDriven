using System;
using System.Linq;
using System.Threading.Tasks;
using CocoriCore;
//using CocoriCore.Mapper;
using CocoriCore.Page;

namespace Comptes.Api
{
    public class Graph_GET : IMessage<SvgResponse>
    {
        public string Q;
    }


    public class Graph_GETHandler : MessageHandler<Graph_GET, SvgResponse>
    {
        private readonly PageGraphBuilder builder;
        private readonly PageGraphFormatter formatter;
        //private readonly MapperGraphBuilder mapperGraphBuilder;
        //private readonly MapperGraphFormatter mapperGraphFormatter;

        public Graph_GETHandler(
            PageGraphBuilder builder,
            PageGraphFormatter formatter
            //MapperGraphBuilder mapperGraphBuilder,
            //MapperGraphFormatter mapperGraphFormatter
            )
        {
            this.builder = builder;
            this.formatter = formatter;
            //this.mapperGraphBuilder = mapperGraphBuilder;
            //this.mapperGraphFormatter = mapperGraphFormatter;
        }

        public override async Task<SvgResponse> ExecuteAsync(Graph_GET message)
        {
            await Task.CompletedTask;

            string svg = "";

            if (message.Q == "mapper")
            {
                var assembly = Comptes.AssemblyInfo.Assembly;
                //var graph = mapperGraphBuilder.Build(assembly);
                //svg = mapperGraphFormatter.Format(graph);
            }
            else
            {
                Func<PageNode, bool> predicate = x => true; ;
                if (message.Q == "users")
                {
                    predicate = x => x.ParameterizedUrl == "/api"
                                  || x.ParameterizedUrl.StartsWith("/api/users");
                }

                var assembly = Comptes.AssemblyInfo.Assembly;
                var graph = builder.Build(assembly, predicate);
                svg = formatter.LinksAndForms(graph);
            }

            return new SvgResponse()
            {
                Svg = svg
            };
        }
    }
}