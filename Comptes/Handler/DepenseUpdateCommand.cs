using System.Threading.Tasks;
using CocoriCore;

namespace Comptes
{
    public class DepenseUpdateCommand : IMessage<Void>
    {
        public ID<Depense> Id { get; set; }
        public ID<Poste> IdPoste;
        public string Description;
        public double Montant;
    }

    public class DepenseUpdateCommandHandler : MessageHandler<DepenseUpdateCommand, Void>
    {
        private readonly IRepository repository;

        public DepenseUpdateCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async override Task<Void> ExecuteAsync(DepenseUpdateCommand message)
        {
            var depense = await repository.LoadAsync<Depense>(message.Id);

            depense.IdPoste = message.IdPoste;
            depense.Description = message.Description;
            depense.Montant = message.Montant;

            await repository.UpdateAsync(depense);

            return new Void();
        }
    }
}