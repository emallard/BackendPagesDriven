using System;
using System.Threading.Tasks;

namespace CocoriCore
{
    public class AsyncCallHandler<TPageQuery, T> : MessageHandler<AsyncCall<TPageQuery, T>, T>
    {
        private readonly IExecuteHandler executeHandler;
        private readonly IPageMapper mapper;

        public AsyncCallHandler(IExecuteHandler executeHandler, IPageMapper mapper)
        {
            this.executeHandler = executeHandler;
            this.mapper = mapper;
        }

        public override async Task<T> ExecuteAsync(AsyncCall<TPageQuery, T> asyncCallMessage)
        {
            Type type = mapper.GetIntermediateType<TPageQuery, T>();
            var message = (IMessage)mapper.Map(type, asyncCallMessage.PageQuery);
            var response = await executeHandler.ExecuteAsync(message);
            return mapper.Map<T>(message, response);
        }
    }
}