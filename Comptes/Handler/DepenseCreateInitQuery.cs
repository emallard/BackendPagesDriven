using System;
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
        public DateTime Date;
    }

    public class DepenseCreateInitHandler : MessageHandler<DepenseCreateInitQuery, DepenseCreateInitResponse>
    {
        private readonly IRepository repository;
        private readonly IClock clock;

        public DepenseCreateInitHandler(
            IRepository repository,
            IClock clock)
        {
            this.repository = repository;
            this.clock = clock;
        }

        public async override Task<DepenseCreateInitResponse> ExecuteAsync(DepenseCreateInitQuery message)
        {
            var poste = await repository.Query<Poste>().FirstOrDefaultAsync();
            var result = new DepenseCreateInitResponse()
            {
                Date = clock.Today
            };

            if (poste != null)
                result.Poste = new ValueLabel<ID<Poste>>()
                {
                    Value = poste.Id,
                    Label = poste.Nom
                };

            return result;

        }
    }
}