using System.Threading.Tasks;
using CocoriCore;

namespace Comptes
{
    public class PosteUpdateCommand : IMessage<Empty>
    {
        public ID<Poste> Id { get; set; }
        public string Nom;
    }

    public class PosteUpdateCommandHandler : MessageHandler<PosteUpdateCommand, Empty>
    {
        private readonly IRepository repository;

        public PosteUpdateCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async override Task<Empty> ExecuteAsync(PosteUpdateCommand message)
        {
            var posteupdate = await repository.LoadAsync<Poste>(message.Id);

            posteupdate.Nom = message.Nom;

            await repository.UpdateAsync(posteupdate);

            return new Empty();
        }
    }
}