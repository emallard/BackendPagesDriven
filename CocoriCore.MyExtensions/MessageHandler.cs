using System.Threading.Tasks;
using CocoriCore;

namespace CocoriCore
{
    public abstract class MessageHandler<TQuery, TResponse> : IHandler<TQuery, TResponse> where TQuery : IMessage<TResponse>
    {
        public async Task<object> HandleAsync(IMessage message)
        {
            return await ExecuteAsync((TQuery)message);
        }

        public abstract Task<TResponse> ExecuteAsync(TQuery message);
    }
}