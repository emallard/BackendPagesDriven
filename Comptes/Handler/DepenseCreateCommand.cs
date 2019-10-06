using System;
using System.Threading.Tasks;
using CocoriCore;

namespace Comptes
{
    public class DepenseCreateCommand : IMessage<ID<Depense>>
    {
        public ID<Poste> IdPoste;
        public string Description;
        public double Montant;
        public DateTime Date;
    }

    public class DepenseCreateCommandHandler : MessageHandler<DepenseCreateCommand, ID<Depense>>
    {
        private readonly IRepository repository;

        public DepenseCreateCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async override Task<ID<Depense>> ExecuteAsync(DepenseCreateCommand message)
        {
            var Depense = new Depense()
            {
                IdPoste = message.IdPoste,
                Description = message.Description,
                Montant = message.Montant,
                Date = message.Date
            };
            await repository.InsertAsync(Depense);
            return Depense.Id;
        }
    }
}