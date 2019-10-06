using System;
using System.Threading.Tasks;
using CocoriCore;

namespace Comptes
{
    public class DepenseUpdateInitQuery : IMessage<DepenseUpdateInitResponse>
    {
        public ID<Depense> Id;
    }

    public class DepenseUpdateInitResponse
    {
        public ValueLabel<ID<Poste>> Poste;
        public string Description;
        public double Montant;
        public DateTime Date;
    }

    public class DepenseUpdateInitHandler : MessageHandler<DepenseUpdateInitQuery, DepenseUpdateInitResponse>
    {
        private readonly IRepository repository;

        public DepenseUpdateInitHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async override Task<DepenseUpdateInitResponse> ExecuteAsync(DepenseUpdateInitQuery message)
        {
            var depense = await repository.LoadAsync(message.Id);
            var poste = await repository.LoadAsync(depense.IdPoste);

            return new DepenseUpdateInitResponse()
            {
                Poste = new ValueLabel<ID<Poste>>() { Value = poste.Id, Label = poste.Nom },
                Description = depense.Description,
                Montant = depense.Montant,
                Date = depense.Date
            };
        }
    }
}