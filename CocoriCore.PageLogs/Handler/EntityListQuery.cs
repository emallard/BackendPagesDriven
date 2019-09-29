using System;
using System.Linq;
using System.Threading.Tasks;
using CocoriCore.Linq.Async;
using CocoriCore.Page;

namespace CocoriCore.PageLogs
{
    public class EntityListQuery : IMessage<string[]>
    {
    }
    public class EntityListHandler : MessageHandler<EntityListQuery, string[]>
    {
        private readonly IRepository repository;

        public EntityListHandler(IRepository repository)
        {
            this.repository = repository;
        }
        public override async Task<string[]> ExecuteAsync(EntityListQuery message)
        {
            return (await repository.Query<TestRepositoryOperation>().ToArrayAsync())
                .Select(x => x.EntityName).Distinct().ToArray();
        }
    }
}