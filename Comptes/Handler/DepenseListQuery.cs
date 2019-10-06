using System;
using System.Linq;
using System.Threading.Tasks;
using CocoriCore;
using CocoriCore.Linq.Async;

namespace Comptes
{
    public class DepenseListQuery : IMessage<DepenseListResponseItem[]>
    {
    }

    public class DepenseListResponseItem
    {
        public ID<Depense> Id { get; set; }
        public ID<Poste> IdPoste;
        public string NomPoste;
        public string Description;
        public double Montant;
        public DateTime Date;
    }

    public class DepenseListHandler : MessageHandler<DepenseListQuery, DepenseListResponseItem[]>
    {
        private readonly IRepository repository;

        public DepenseListHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async override Task<DepenseListResponseItem[]> ExecuteAsync(DepenseListQuery message)
        {
            var depenses = await repository.Query<Depense>().ToArrayAsync();
            var listIdPostes = depenses.Select(x => x.IdPoste).Distinct().ToArray();
            var postes = await repository.Query<Poste>().Where(x => listIdPostes.Contains(x.Id)).ToArrayAsync();

            var items = depenses.Select(x => new DepenseListResponseItem()
            {
                Id = x.Id,
                IdPoste = x.IdPoste,
                NomPoste = postes.First(y => y.Id == x.IdPoste).Nom,
                Description = x.Description,
                Montant = x.Montant,
                Date = x.Date
            }).ToArray();

            return items;
        }
    }
}