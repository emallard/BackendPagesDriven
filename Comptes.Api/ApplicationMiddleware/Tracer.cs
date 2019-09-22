using System.Threading.Tasks;
using CocoriCore;

namespace Comptes.Api
{
    public class Tracer : ITracer
    {
        public async Task Trace(object obj)
        {
            await Task.CompletedTask;
        }
    }
}