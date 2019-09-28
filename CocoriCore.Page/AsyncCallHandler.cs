using System;
using System.Threading.Tasks;

namespace CocoriCore
{
    public class AsyncCallHandler<TQuery, TModel> : MessageHandler<AsyncCall<TQuery, TModel>, TModel>
        where TQuery : IMessage
    {
        private readonly IExecuteHandler executeHandler;
        private readonly IPageMapper mapper;

        public AsyncCallHandler(IExecuteHandler executeHandler, IPageMapper mapper)
        {
            this.executeHandler = executeHandler;
            this.mapper = mapper;
        }

        public override async Task<TModel> ExecuteAsync(AsyncCall<TQuery, TModel> asyncCallMessage)
        {
            var response = await executeHandler.ExecuteAsync(asyncCallMessage.Query);
            return mapper.Map<TModel>(asyncCallMessage.Query, response);
        }
    }
}