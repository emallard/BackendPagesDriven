using System.Threading.Tasks;

namespace CocoriCore.Page
{
    public class MessageBusSpy<T> : IMessageBus where T : IMessageBus
    {
        private readonly T messageBus;
        private readonly ICurrentUserLogger logger;

        public MessageBusSpy(
            T messageBus,
            ICurrentUserLogger logger)
        {
            this.messageBus = messageBus;
            this.logger = logger;
        }

        public async Task<object> ExecuteAsync(IMessage message)
        {
            var loggedMessage = message;

            if (message is IAsyncCall asyncCall)
            {
                logger.Log(new LogAsyncCall() { MemberName = asyncCall.GetMemberName() });
                loggedMessage = asyncCall.GetMessage();
            }

            if (message is IForm form)
            {
                logger.Log(new LogSubmit() { MemberName = form.GetMemberName() });
                loggedMessage = form.GetCommand();
            }


            logger.Log(new LogMessageBus()
            {
                Message = loggedMessage
            });

            var response = await messageBus.ExecuteAsync(message);
            return response;
        }
    }
}