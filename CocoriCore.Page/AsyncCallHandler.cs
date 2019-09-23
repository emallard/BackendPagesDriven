using System;
using System.Threading.Tasks;

namespace CocoriCore
{
    public class AsyncCallHandler<T> : MessageHandler<AsyncCall<T>, T>
    {
        private readonly IExecuteHandler executeHandler;
        private readonly IPageMapper mapper;

        public AsyncCallHandler(IExecuteHandler executeHandler, IPageMapper mapper)
        {
            this.executeHandler = executeHandler;
            this.mapper = mapper;
        }

        public override async Task<T> ExecuteAsync(AsyncCall<T> asyncCallMessage)
        {
            Type type = mapper.GetIntermediateType(asyncCallMessage.PageQuery.GetType(), typeof(T));
            var message = (IMessage)mapper.Map(type, asyncCallMessage.PageQuery);
            var response = await executeHandler.ExecuteAsync(message);
            return mapper.Map<T>(message, response);
        }
    }
}