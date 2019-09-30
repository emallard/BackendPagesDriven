using System.Threading.Tasks;
using CocoriCore;

namespace Comptes
{
    public class PosteViewQuery : IMessage<PosteViewResponse>
    {
        public ID<Poste> Id;
    }

    public class PosteViewResponse
    {
        public ID<Poste> Id { get; set; }
        public string Nom;
    }

    public class PosteViewHandler : MessageHandler<PosteViewQuery, PosteViewResponse>
    {
        private readonly IRepository repository;

        public PosteViewHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async override Task<PosteViewResponse> ExecuteAsync(PosteViewQuery message)
        {
            var poste = await repository.LoadAsync(message.Id);

            return new PosteViewResponse()
            {
                Id = poste.Id,
                Nom = poste.Nom,
            };
        }
    }
}