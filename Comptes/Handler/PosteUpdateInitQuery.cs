using System.Threading.Tasks;
using CocoriCore;

namespace Comptes
{
    public class PosteUpdateInitQuery : IMessage<PosteUpdateInitResponse>
    {
        public ID<Poste> Id;
    }

    public class PosteUpdateInitResponse
    {
        public string Nom;
    }

    public class PosteUpdateInitHandler : MessageHandler<PosteUpdateInitQuery, PosteUpdateInitResponse>
    {
        private readonly IRepository repository;

        public PosteUpdateInitHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async override Task<PosteUpdateInitResponse> ExecuteAsync(PosteUpdateInitQuery message)
        {
            var poste = await repository.LoadAsync(message.Id);
            return new PosteUpdateInitResponse()
            {
                Nom = poste.Nom
            };
        }
    }
}