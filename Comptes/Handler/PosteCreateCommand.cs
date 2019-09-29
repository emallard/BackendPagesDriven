using System;
using System.Threading.Tasks;
using CocoriCore;

namespace Comptes
{
    public class PosteCreateCommand : IMessage<ID<Poste>>
    {
        public string Nom;
    }

    public class PosteCreateCommandHandler : MessageHandler<PosteCreateCommand, ID<Poste>>
    {
        private readonly IRepository repository;

        public PosteCreateCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async override Task<ID<Poste>> ExecuteAsync(PosteCreateCommand message)
        {
            var poste = new Poste()
            {
                Nom = message.Nom
            };
            await repository.InsertAsync(poste);
            return poste.Id;
        }
    }
}