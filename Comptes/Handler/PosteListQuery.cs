using System.Linq;
using System.Threading.Tasks;
using CocoriCore;
using CocoriCore.Linq.Async;

namespace Comptes
{
    public class PosteListQuery : IMessage<PosteListResponseItem[]>
    {
    }

    public class PosteListResponseItem
    {
        public ID<Poste> Id { get; set; }
        public string Nom;
    }

    public class PosteListHandler : MessageHandler<PosteListQuery, PosteListResponseItem[]>
    {
        private readonly IRepository repository;

        public PosteListHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async override Task<PosteListResponseItem[]> ExecuteAsync(PosteListQuery message)
        {
            var postes = await repository.Query<Poste>().ToArrayAsync();

            var items = postes.Select(x => new PosteListResponseItem()
            {
                Id = x.Id,
                Nom = x.Nom,
            }).ToArray();

            return items;
        }
    }
}