using System.Linq;
using System.Threading.Tasks;
using CocoriCore;
using CocoriCore.Linq.Async;

namespace Comptes
{
    public class DepenseCreateInitQuery : IMessage<DepenseCreateInitResponse>
    {

    }

    public class DepenseCreateInitResponse
    {
        public ValueLabel<ID<Poste>> Poste;
    }

    public class DepenseCreateInitHandler : MessageHandler<DepenseCreateInitQuery, DepenseCreateInitResponse>
    {
        private readonly IRepository repository;

        public DepenseCreateInitHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async override Task<DepenseCreateInitResponse> ExecuteAsync(DepenseCreateInitQuery message)
        {
            var poste = await repository.Query<Poste>().FirstOrDefaultAsync();
            if (poste != null)
                return new DepenseCreateInitResponse()
                {
                    Poste = new ValueLabel<ID<Poste>>()
                    {
                        Value = poste.Id,
                        Label = poste.Nom
                    }
                };
            else
                return new DepenseCreateInitResponse()
                {
                };

        }
    }
}