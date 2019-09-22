using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using CocoriCore;
using CocoriCore.Router;

namespace Comptes.Api
{

    public class ApiFootprintQuery : IQuery
    {
    }

    public class ApiFootprintHandler : QueryHandler<ApiFootprintQuery, ApiFootprint>
    {
        private readonly IRouter router;
        private readonly FootprintGenerator footprintGenerator;

        public ApiFootprintHandler(
            IRouter router)
        {
            this.router = router;
            this.footprintGenerator = new FootprintGenerator();
        }

        public override async Task<ApiFootprint> ExecuteAsync(ApiFootprintQuery query)
        {
            var routes = ((CocoriCore.Router.Router)this.router).AllRoutes;
            await Task.CompletedTask;
            return this.footprintGenerator.Generate(
                routes
                .Where(r => r.MessageType != typeof(ApiFootprintQuery))
                .Select(r => r.GetDescriptor()),
                Comptes.AssemblyInfo.Assembly
                );
        }
    }
}