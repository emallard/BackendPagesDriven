using System.Threading.Tasks;

namespace CocoriCore
{
    public interface IExecuteHandler
    {
        Task<T> ExecuteAsync<T>(IMessage<T> message);

        Task<object> ExecuteAsync(IMessage message);
    }
}