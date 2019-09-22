using System;
using System.Threading.Tasks;
using CocoriCore;

namespace Comptes
{

    public class MessageBus : IMessageBus
    {
        private readonly HandlerFinder handlerTypes;
        private readonly IFactory factory;
        private readonly IPageMapper mapper;

        //private readonly IRepository repository;

        public MessageBus(
            HandlerFinder handlerTypes,
            IFactory factory,
            IPageMapper mapper
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

            if (message is IQuery)
            {
                // repository.SetReadOnly();
            }

            if (message is ICommand)
            {
                // repository.SetReadWrite();
            }

            /*
            if (message is IUserMessage)
            {
                var userClaims = authenticator.GetClaims<UserClaims>();
            }
            
                        if (message is IProfilMessage)
                        {
                            var userClaims = authenticator.GetClaims<UserClaims>();
                            var profile = repository.Loadsync<Profile>(message.Id);
                            if (profile.IdUtilisateur != userClaims.Id)
                                throw new AuthenticationException("");
                        }*/
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