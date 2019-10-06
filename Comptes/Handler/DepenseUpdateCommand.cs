using System;
using System.Threading.Tasks;
using CocoriCore;

namespace Comptes
{
    public class DepenseUpdateCommand : IMessage<Empty>
    {
        public ID<Depense> Id { get; set; }
        public ID<Poste> IdPoste;
        public string Description;
        public double Montant;
        public DateTime Date;
    }

    public class DepenseUpdateCommandHandler : MessageHandler<DepenseUpdateCommand, Empty>
    {
        private readonly IRepository repository;

        public DepenseUpdateCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async override Task<Empty> ExecuteAsync(DepenseUpdateCommand message)
        {
            var depense = await repository.LoadAsync<Depense>(message.Id);

            depense.IdPoste = message.IdPoste;
            depense.Description = message.Description;
            depense.Montant = message.Montant;
            depense.Date = message.Date;

            await repository.UpdateAsync(depense);

            return new Empty();
        }
    }
}