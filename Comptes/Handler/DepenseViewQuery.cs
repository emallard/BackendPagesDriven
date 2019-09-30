using System.Threading.Tasks;
using CocoriCore;

namespace Comptes
{
    public class DepenseViewQuery : IMessage<DepenseViewResponse>
    {
        public ID<Depense> Id;
    }

    public class DepenseViewResponse
    {
        public ID<Depense> Id { get; set; }
        public ID<Poste> IdPoste;
        public string NomPoste;
        public string Description;
        public double Montant;
    }

    public class DepenseViewHandler : MessageHandler<DepenseViewQuery, DepenseViewResponse>
    {
        private readonly IRepository repository;

        public DepenseViewHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async override Task<DepenseViewResponse> ExecuteAsync(DepenseViewQuery message)
        {
            var depense = await repository.LoadAsync(message.Id);
            var poste = await repository.LoadAsync(depense.IdPoste);

            return new DepenseViewResponse()
            {
                Id = depense.Id,
                IdPoste = depense.IdPoste,
                NomPoste = poste.Nom,
                Description = depense.Description,
                Montant = depense.Montant
            };
        }
    }
}