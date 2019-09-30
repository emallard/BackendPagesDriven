using System.Linq;
using System.Threading.Tasks;
using CocoriCore;
using CocoriCore.Linq.Async;

namespace Comptes
{
    public class DepenseCreateDefaultValueQuery : IMessage<DepenseCreateDefaultValueResponse>
    {

    }

    public class DepenseCreateDefaultValueResponse
    {
        public ID<Poste> IdPoste;
        public string NomPoste;
    }

    public class DepenseCreateDefaultValueHandler : MessageHandler<DepenseCreateDefaultValueQuery, DepenseCreateDefaultValueResponse>
    {
        private readonly IRepository repository;

        public DepenseCreateDefaultValueHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async override Task<DepenseCreateDefaultValueResponse> ExecuteAsync(DepenseCreateDefaultValueQuery message)
        {
            var poste = await repository.Query<Poste>().FirstOrDefaultAsync();
            if (poste != null)
                return new DepenseCreateDefaultValueResponse()
                {
                    IdPoste = poste.Id,
                    NomPoste = poste.Nom
                };
            else
                return new DepenseCreateDefaultValueResponse()
                {
                };

        }
    }
}