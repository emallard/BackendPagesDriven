using System;
using System.Threading.Tasks;
using CocoriCore;

namespace CocoriCore.Mapper
{

    public class MessageBus : IMessageBus
    {
        private readonly HandlerFinder handlerTypes;
        private readonly IFactory factory;
        private readonly INewMapper mapper;

        //private readonly IRepository repository;

        public MessageBus(
            HandlerFinder handlerTypes,
            IFactory factory,
            INewMapper mapper
            //IRepository repository,
            //IClaimsProvider authenticator
            )
        {
            this.handlerTypes = handlerTypes;
            this.factory = factory;
            this.mapper = mapper;
            //this.authenticator = authenticator;
        }

        public async Task<object> ExecuteAsync(IMessage message)
        {
            /*
            object response;
            if (!mapper.TryHandle(message, out response))
                response = await FindHandlerAndExecute(message);
            */
            var response = await FindHandlerAndExecute(message);
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