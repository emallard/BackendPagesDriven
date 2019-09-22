using System;
using System.Threading.Tasks;

namespace CocoriCore
{

    public class CallHandler<TMessage, TMessageResponse> :
            MessageHandler<
                Call<TMessage, TMessageResponse>,
                TMessageResponse>
        where TMessage : IMessage<TMessageResponse>
    {
        private readonly IExecuteHandler executeHandler;

        public CallHandler(IExecuteHandler executeHandler)
        {
            this.executeHandler = executeHandler;
        }

        public override async Task<TMessageResponse> ExecuteAsync(
            Call<TMessage, TMessageResponse> message)
        {
            return await executeHandler.ExecuteAsync(message.Message);
        }
    }
}