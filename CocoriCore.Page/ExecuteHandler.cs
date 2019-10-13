using System;
using System.Threading.Tasks;
using CocoriCore;

namespace CocoriCore.Page
{

    public class ExecuteHandler : IExecuteHandler
    {
        private readonly HandlerFinder handlerTypes;
        private readonly IFactory factory;
        private readonly IPageMapper mapper;

        //private readonly IRepository repository;

        public ExecuteHandler(
            HandlerFinder handlerTypes,
            IFactory factory,
            IPageMapper mapper)
        {
            this.handlerTypes = handlerTypes;
            this.factory = factory;
            this.mapper = mapper;
        }

        public async Task<T> ExecuteAsync<T>(IMessage<T> message)
        {
            var h = factory.Create(this.handlerTypes.GetHandlerType(message));
            var response = await ((IHandler)h).HandleAsync(message);
            return (T)response;
        }

        public async Task<object> ExecuteAsync(IMessage message)
        {
            object response;
            if (!mapper.TryHandle(message, out response))
            {
                var h = factory.Create(this.handlerTypes.GetHandlerType(message));
                response = await ((IHandler)h).HandleAsync(message);

            }
            return response;
        }
    }
}