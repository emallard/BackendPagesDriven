using System;
using System.Threading.Tasks;
using CocoriCore;

namespace MultiUser
{

    public class MessageBus : IMessageBus
    {
        private readonly HandlerFinder handlerTypes;
        private readonly IFactory factory;
        private readonly IPageMapper mapper;

        public MessageBus(
            HandlerFinder handlerTypes,
            IFactory factory,
            IPageMapper mapper
            )
        {
            this.handlerTypes = handlerTypes;
            this.factory = factory;
            this.mapper = mapper;
        }

        public async Task<object> ExecuteAsync(IMessage message)
        {
            object response;
            if (!mapper.TryHandle(message, out response))
                response = await FindHandlerAndExecute(message);

            if (message is ICommand)
            {
                try
                {
                    // repository.CommitAsync();
                }
                catch (Exception e)
                {
                    // repository.RollbackAsync();
                    throw e;
                }
            }

            return response;
        }

        public async Task<object> FindHandlerAndExecute(IMessage message)
        {
            var h = factory.Create(this.handlerTypes.GetHandlerType(message));
            var response = await ((IHandler)h).HandleAsync(message);
            return response;
        }
    }
}