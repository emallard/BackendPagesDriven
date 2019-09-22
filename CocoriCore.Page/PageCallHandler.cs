using System;
using System.Threading.Tasks;

namespace CocoriCore
{

    public class PageCallHandler<TPageGet, TMessage, TMessageResponse, TPageResponse> :
            MessageHandler<
                PageCall<TPageGet, TMessage, TMessageResponse, TPageResponse>,
                TPageResponse>
        where TMessage : IMessage<TMessageResponse>
        where TPageGet : IMessage
    {
        private readonly IExecuteHandler executeHandler;

        public PageCallHandler(IExecuteHandler executeHandler)
        {
            this.executeHandler = executeHandler;
        }

        public override async Task<TPageResponse> ExecuteAsync(
            PageCall<TPageGet, TMessage, TMessageResponse, TPageResponse> message)
        {

            var page = await executeHandler.ExecuteAsync(message.PageMessage);
            var fieldInfo = page.GetType().GetField(message.MemberName);
            var pageCall = (PageCall<TPageGet, TMessage, TMessageResponse, TPageResponse>)fieldInfo.InvokeGetter(page);
            var messageResponse = await executeHandler.ExecuteAsync(message.Message);
            return pageCall.Translate(message.Message, messageResponse);
        }
    }
}