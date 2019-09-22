using System;
using System.Threading.Tasks;
using CocoriCore;

namespace Comptes.Api
{
    public class Load<T> : IMessage<T> where T : class, IEntity
    {
        public Guid Id;
    }

    public class LoadHandler<T> : MessageHandler<Load<T>, T> where T : class, IEntity
    {
        private readonly IRepository repository;

        public LoadHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async override Task<T> ExecuteAsync(Load<T> message)
        {
            return await repository.LoadAsync<T>(message.Id);
        }
    }
}
/*
builder.Get<Load<Vendeur>>("/api/vendeurs/:id");
builder.Get<Load<Annonce>>("/api/annnonces/:id");
builder.Get<Load<Utilisateur>>("/api/utilisateur/:id");
*/
