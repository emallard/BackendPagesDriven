using System.Linq;
using System.Threading.Tasks;
using CocoriCore.Linq.Async;

namespace CocoriCore
{
    public class ListQuery<T> : IMessage<T[]>
    {

    }

    public class ListQueryHandler<T> : MessageHandler<ListQuery<T>, T[]>
    {
        private readonly IRepository repository;
        private readonly INewMapper mapper;

        public ListQueryHandler(IRepository repository, INewMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public override async Task<T[]> ExecuteAsync(ListQuery<T> message)
        {
            var entityType = mapper.GetViewEntityType<T>();
            var entityArray = await repository.Query(entityType).ToArrayAsync();
            return entityArray.Select(x => mapper.View<T>((IEntity)x)).ToArray();
        }
    }
}