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
            logger.Log(new LogMessageBus()
            {
                Message = message
            });

            var response = await messageBus.ExecuteAsync(message);
            return response;
        }
    }
}