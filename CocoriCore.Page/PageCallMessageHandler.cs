using System.Threading.Tasks;

namespace CocoriCore
{

    public class PageCallMessageHandler<TPage, TPageGet, TMessage, TMessageResponse, TFormResponse> :
            MessageHandler<
                PageCallMessage<TPage, TPageGet, TMessage, TMessageResponse, TFormResponse>,
                TFormResponse>
        where TMessage : IMessage<TMessageResponse>
        where TPageGet : IMessage
    {
        private readonly IExecuteHandler executeHandler;

        public PageCallMessageHandler(IExecuteHandler executeHandler)
        {
            this.executeHandler = executeHandler;
        }

        public override async Task<TFormResponse> ExecuteAsync(
            PageCallMessage<TPage, TPageGet, TMessage, TMessageResponse, TFormResponse> message)
        {
            var page = await executeHandler.ExecuteAsync(message.PageGet);

            //var form = message.Member.Compile()(page);
            var form = message.PageMember(page);
            var messageResponse = await executeHandler.ExecuteAsync(message.Message);
            return form.Translate(message.Message, messageResponse);
        }
    }
}